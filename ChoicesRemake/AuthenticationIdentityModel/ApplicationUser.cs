using System;
using Microsoft.AspNetCore.Identity;


namespace AuthenticationIdentityModel
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }

        [PersonalData]
        public string Surname { get; set; }

    }
}
