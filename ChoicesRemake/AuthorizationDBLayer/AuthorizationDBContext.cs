using AuthorizationModel;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuthorizationDBLayer
{
    public class AuthorizationDBContext: DbContext
    {
        public AuthorizationDBContext(DbContextOptions<AuthorizationDBContext> options)
            : base(options)
        {
        }
        public DbSet<UserRole> userRoles { get; set; } 
    }
}
