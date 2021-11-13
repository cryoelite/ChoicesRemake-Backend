using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationIdentityModel
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [NotMapped]
        public string password { get; set; }

        [PersonalData]
        public string Surname { get; set; }
    }
}