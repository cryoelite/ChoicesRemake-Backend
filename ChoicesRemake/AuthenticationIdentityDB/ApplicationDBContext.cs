using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationIdentityDB
{
    public class ApplicationDBContext<TUser> : IdentityUserContext<TUser> where TUser: IdentityUser
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext<IdentityUser>> options)
       : base(options) { }

    }
}
