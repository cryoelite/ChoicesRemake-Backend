using AuthorizationDBLayer;
using AuthorizationModel;
using IAuthorizationRepository;
using System.Threading.Tasks;

namespace AuthorizationRepository
{
    public class AuthorizationRepo : IAuthorizationRepo
    {
        private AuthorizationDBContext adb;

        public AuthorizationRepo(AuthorizationDBContext dbContext) => (adb) = (dbContext);

        public async Task<bool> addNewUser(UserRole userRole)
        {
            try
            {
                await adb.AddAsync(userRole);
                await adb.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

#nullable enable

        public async Task<string?> getUserRole(string username)
        {
            var userRole = await adb.userRoles.FindAsync(username);
            return userRole?.role;
        }

#nullable disable

        public async Task<bool> removeExistingUser(string username)
        {
            var result = await adb.userRoles.FindAsync(username);
            try
            {
                if (result != null)
                {
                    adb.userRoles.Remove(result);
                    await adb.SaveChangesAsync();
                    return true;
                }
            }
            catch
            { }
            return false;
        }

        public async Task<bool> updateUserRole(UserRole userRole)
        {
            try
            {
                var result = adb.Update<UserRole>(userRole);
                await adb.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}