using JWT_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWT_API.Data
{
    public class ApiDbContext : IdentityDbContext
    {
        public virtual DbSet<ItemData> Items { get; set; }
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            :base(options)
        {

        }
    }
}
