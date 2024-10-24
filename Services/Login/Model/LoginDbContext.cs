using Microsoft.EntityFrameworkCore;
using ShopNest.Services.User.Model;

namespace ShopNest.Services.Login.Model
{

    public class LoginDbContext : DbContext
    {
        public LoginDbContext(DbContextOptions<LoginDbContext> options) : base(options) { }
        public DbSet<UserList> UserList { get; set; }

    }
}
