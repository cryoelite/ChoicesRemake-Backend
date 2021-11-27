using AuthorizationDBLayer;
using AuthorizationModel;
using IAuthorizationRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AuthorizationRepository
{
    public class AuthorizationRepo : IAuthorizationRepo
    {
        private AuthorizationDBContext adb;
        private ILogger<AuthorizationRepo> logger;

        public AuthorizationRepo(AuthorizationDBContext dbContext, ILogger<AuthorizationRepo> logger) => (adb, this.logger) = (dbContext, logger);

        public async Task<bool> addNewUser(UserRole userRole)
        {
            logger.LogInformation($"Adding new user with username {userRole.username}");
            try
            {
                await adb.AddAsync(userRole);
                await adb.SaveChangesAsync();
                logger.LogInformation($"Added new user successfully with username {userRole.username} and role {userRole.role}");

                return true;
            }
            catch (Exception e)
            {
                logger.LogInformation($"Failed in adding new user {userRole.username} with {e.Message}");

                return false;
            }
        }

        public async Task<string?> getUser(string username)
        {
            logger.LogInformation($"Retrieving user details for {username}");

            var userRole = await adb.userRoles.FindAsync(username);
            return userRole?.role;
        }

        public async Task<bool> removeExistingUser(string username)
        {
            logger.LogInformation($"Removing existing user with username {username}");

            var result = await adb.userRoles.FindAsync(username);
            try
            {
                if (result != null)
                {
                    adb.userRoles.Remove(result);
                    await adb.SaveChangesAsync();

                    logger.LogInformation($"Removed existing user {username} successfully");

                    return true;
                }
            }
            catch (Exception e)
            {
                logger.LogInformation($"Failed in removing user {username} with {e.Message}");
            }

            return false;
        }

        public async Task<bool> updateUser(UserRole userRole)
        {
            logger.LogInformation($"Updating {userRole.username}");

            try
            {
                var result = adb.Update<UserRole>(userRole);
                await adb.SaveChangesAsync();
                logger.LogInformation($"Updated {userRole.username} successfully");

                return true;
            }
            catch (Exception e)
            {
                logger.LogInformation($"Failed in updating {userRole.username} with {e.Message}");

                return false;
            }
        }
    }
}