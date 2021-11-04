using AuthorizationModel;
using System;
using System.Threading.Tasks;

namespace IAuthorizationRepository
{
    public interface IAuthorizationRepo
    {
        public Task<bool> addNewUser(UserRole userRole);
        public Task<bool> removeExistingUser(string username);
        public Task<string?> getUserRole(string username);
        public Task<bool> updateUserRole(UserRole userRole);
    }
}
