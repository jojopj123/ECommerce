using Microsoft.EntityFrameworkCore;
using ShopNest.Services.User.Model;
using System.Collections.Generic;

namespace ShopNest.Services.User.Model
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<UserData> UserData { get; set; }

        public DbSet<UserDetails> UserDetails { get; set; }


       
    }
}



