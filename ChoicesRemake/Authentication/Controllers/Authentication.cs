using Authentication.Settings;
using AuthenticationIdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web.Resource;
using Microsoft.IdentityModel.Tokens;
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
        private readonly UserManager<ApplicationUser> manager;
        private readonly JWTSettings jwtOptions;




        public Authentication(ILogger<Authentication> logger, UserManager<ApplicationUser> userManager, IOptions<JWTSettings> options)
            => (_logger, manager, jwtOptions) = (logger, userManager, options.Value);

        [NonAction]
        public async Task<ApplicationUser?> Register(ApplicationUser user)
        {

            var _user = new ApplicationUser()
            {
                UserName = user.Email,
                Email = user.Email,
                FirstName = user.FirstName,
                Surname = user.Surname,

            };

            var registerState = await manager.CreateAsync(_user, user.password);
            if (registerState.Succeeded)
            {
                return _user;
            }

            return null;
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await Register(user);

                if (result != null)
                {
                    Response.Headers.Add("Bearer-Token", GenerateJwt(result));
                    return Created(string.Empty, string.Empty);
                }

                return Problem("Error in user creation", null, 500);

            }

            return BadRequest("User details provided incorrectly");
        }

        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var _user = new ApplicationUser()
                {
                    UserName = user.Email,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Surname = user.Surname,

                };

                var registerState = await manager.CreateAsync(_user, user.password);
                if (registerState.Succeeded)
                {
                    Response.Headers.Add("Bearer-Token", GenerateJwt(_user));
                    return Created(string.Empty, string.Empty);
                }

                return Problem("Error in user creation", null, 500);

            }

            return BadRequest("User details provided incorrectly");
        }

        [HttpPost("registerVendor")]
        public async Task<IActionResult> RegisterVendor([FromBody] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var _user = new ApplicationUser()
                {
                    UserName = user.Email,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Surname = user.Surname,

                };

                var registerState = await manager.CreateAsync(_user, user.password);
                if (registerState.Succeeded)
                {
                    Response.Headers.Add("Bearer-Token", GenerateJwt(_user));
                    return Created(string.Empty, string.Empty);
                }

                return Problem("Error in user creation", null, 500);

            }

            return BadRequest("User details provided incorrectly");
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var _user = manager.Users.SingleOrDefault(u => u.Email == user.Email);
                if (_user is null || _user == new ApplicationUser())
                {
                    return NotFound("User not found");
                }
                var loginState = await manager.CheckPasswordAsync(_user, user.password);

                if (loginState)
                {
                    //TODO send role to kafka authorization and also send jwt
                    Response.Headers.Add("Bearer-Token", GenerateJwt(_user));
                    return Ok();

                }
                return BadRequest("Email or Password incorrect");

            }

            return Problem("User details provided incorrectly", null, 500);
        }

        [HttpGet("checkToken")]
        public IActionResult CheckToken()
        {
            Microsoft.Extensions.Primitives.StringValues tokens;
            Request.Headers.TryGetValue("Bearer-Token", out tokens);
            var token = tokens.ToString();
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            var claims = JsonSerializer.Serialize(jsonToken.Claims);
            return new JsonResult(claims);

        }


        [NonAction]
        private string GenerateJwt(ApplicationUser user)
        {
            var claim = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var secret = jwtOptions.Secret;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, jwtOptions.Algorithm);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtOptions.ExpirationInDays));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Issuer,
                claim,
                expires: expires,
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
