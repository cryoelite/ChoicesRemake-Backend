using Authentication.Models;
using AuthenticationIdentityModel;
using KafkaService.Models;
using KafkaService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StaticAssets;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("auth")]
    public class Authentication : ControllerBase
    {
        private readonly ILogger<Authentication> _logger;
        private readonly JWTSettings jwtOptions;
        private readonly KafkaProducer kafkaProducer;
        private readonly UserManager<ApplicationUser> manager;

        public Authentication(ILogger<Authentication> logger, UserManager<ApplicationUser> userManager, JWTSettings options, KafkaProducer producer)
            => (_logger, manager, jwtOptions, kafkaProducer) = (logger, userManager, options, producer);

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginUser user)
        {
            if (ModelState.IsValid)
            {
                var userStatus = await doesUserExist(user.Email);
                if (!userStatus)
                {
                    return NotFound("User not found");
                }
                var _user = await manager.FindByEmailAsync(user.Email);
                var loginState = await manager.CheckPasswordAsync(_user, user.Password);

                if (loginState)
                {
                    var kafkaData = new KafkaData(InvocationType.invokeAndReturn, MethodNames.getRole);
                    kafkaData.AddHeader(ClaimTypes.Email, user.Email);
                    var result = await kafkaProducer.SendAndReceiveData(kafkaData);
                    if (result.message.Value != ResultStatus.success)
                    {
                        return Problem("Internal Server error in getting login data", statusCode: 500);
                    }
                    var role = result.GetCustomHeader(Role.role);
                    if (role == null)
                    {
                        _logger.LogError("Login data doesn't have Role header in Kafka");
                        return Problem("Internal Server error in getting login data", statusCode: 500);
                    }
                    Response.Headers.Add(CustomHeader.bearerToken, GenerateJwt(_user, role));
                    return Ok();
                }
                return BadRequest("Email or Password incorrect");
            }

            return Problem("User details provided incorrectly", null, 500);
        }

        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUser user)
        {
            return await Register(user, Role.admin);
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUser user)
        {
            return await Register(user, Role.user);
        }

        [HttpPost("registerVendor")]
        public async Task<IActionResult> RegisterVendor([FromBody] RegisterUser user)
        {
            return await Register(user, Role.vendor);
        }

        [HttpGet("verifyToken")]
        public IActionResult VerifyToken([FromHeader(Name = CustomHeader.bearerToken)] string token)
        {
            try
            {
                var signingKey = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
                var encKey = Encoding.UTF8.GetBytes(jwtOptions.EncryptionKey);
                var byteArray = new byte[32];
                Array.Copy(encKey, byteArray, 32);

                var encSigningKey = new SymmetricSecurityKey(signingKey);
                var encEncKey = new SymmetricSecurityKey(byteArray);
                var handler = new JwtSecurityTokenHandler();
                var claim = handler.ValidateToken(token, new TokenValidationParameters()
                { TokenDecryptionKey = encEncKey, IssuerSigningKey = encSigningKey, ValidAudience = jwtOptions.Issuer, ValidIssuer = jwtOptions.Issuer }, out SecurityToken securityToken);

                var claims = claim.Claims.ToList();
                var sb = new StringBuilder();
                foreach (var elem in claims)
                {
                    sb.AppendLine(elem.ToString());
                }
                var jsonClaims = JsonSerializer.Serialize(sb.ToString());
                return new JsonResult(jsonClaims);
            }
            catch
            {
                return BadRequest("Failed to verify token");
            }
        }

        [NonAction]
        private async Task<bool> doesUserExist(string username)
        {
            var userStatus = await manager.FindByEmailAsync(username);
            return userStatus != null;
        }

        [NonAction]
        private string GenerateJwt(ApplicationUser user, string role)
        {
            var claim = new Dictionary<string, object> {
                { JwtRegisteredClaimNames.Sub, user.Id.ToString() },
                { ClaimTypes.Name, user.FirstName},
                { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() },
                { ClaimTypes.Email, user.Email.ToString()},
                { ClaimTypes.Role, role}
            };
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtOptions.ExpirationInDays));

            var signingKey = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
            var encKey = Encoding.UTF8.GetBytes(jwtOptions.EncryptionKey);
            var byteArray = new byte[32];
            Array.Copy(encKey, byteArray, 32);

            var encSigningKey = new SigningCredentials(new SymmetricSecurityKey(signingKey), jwtOptions.SigningAlgorithm);
            var encEncKey = new EncryptingCredentials(new SymmetricSecurityKey(byteArray), jwtOptions.KeyWrapAlgorithm, jwtOptions.DataEncryptionAlgorithm);

            var token = new SecurityTokenDescriptor()
            {
                Claims = claim,
                EncryptingCredentials = encEncKey,
                SigningCredentials = encSigningKey,
                Audience = jwtOptions.Issuer,
                Issuer = jwtOptions.Issuer,
                IssuedAt = DateTime.UtcNow,
                Expires = expires,
            };
            var secureToken = new JwtSecurityTokenHandler().CreateEncodedJwt(token);
            return secureToken;
        }

        [NonAction]
        private async Task<IActionResult> Register(RegisterUser user, string role)
        {
            if (ModelState.IsValid)
            {
                if (await doesUserExist(user.Email))
                {
                    return Problem("User already exists", statusCode: 409);
                }
                if (role == Role.admin || role == Role.vendor)
                {
                    var result = await VerifyAdminRole();
                    if (result != null)
                    {
                        return result;
                    }
                }
                //Could store role in the jwt itself, but in a seperate api for no reason xd
                var _user = new ApplicationUser()
                {
                    UserName = user.Email,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Surname = user.LastName,
                };

                var registerState = await manager.CreateAsync(_user, user.Password);
                if (registerState.Succeeded)
                {
                    var token = GenerateJwt(_user, role);

                    var kafkaData = new KafkaData(InvocationType.justInvoke, MethodNames.addUser);
                    kafkaData.AddHeader(CustomHeader.bearerToken, token);
                    kafkaData.AddHeader(Role.role, role);

                    await kafkaProducer.SendData(kafkaData);

                    Response.Headers.Add(CustomHeader.bearerToken, token);
                    return Created(string.Empty, string.Empty);
                }
                var error = registerState.Errors.FirstOrDefault();
                return Problem($"Error in user creation {error?.Description}", null, 400);
            }

            return BadRequest("User details provided incorrectly");
        }

        [NonAction]
        private async Task<IActionResult?> VerifyAdminRole()
        {
            Microsoft.Extensions.Primitives.StringValues backerToken;
            var backerResult = Request.Headers.TryGetValue(CustomHeader.backerToken, out backerToken);
            if (!backerResult)
            {
                return Problem($"For registering a user of role {Role.admin}, a valid admin bearer-token is needed in the header {CustomHeader.backerToken}", null, 401);
            }

            var kafkaData = new KafkaData(InvocationType.invokeAndReturn, MethodNames.verifyAdmin);
            kafkaData.AddHeader(CustomHeader.backerToken, backerToken);

            var result = await kafkaProducer.SendAndReceiveData(kafkaData);
            if (result.message.Value == ResultStatus.unavailable)
            {
                return Problem($"Internal server error in processing the request", null, 500);
            }
            else if (result.message.Value != ResultStatus.success)
            {
                return Problem($"Invalid backer token or backer role not Admin", null, 401);
            }
            return null;
        }
    }
}