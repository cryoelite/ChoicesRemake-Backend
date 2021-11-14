using AuthorizationModel;
using System.Threading.Tasks;

namespace IAuthorizationRepository
{
    public interface IAuthorizationRepo
    {
        public Task<bool> addNewUser(UserRole userRole);

        public Task<string?> getUser(string username);

        public Task<bool> removeExistingUser(string username);

        public Task<bool> updateUser(UserRole userRole);
    }
}