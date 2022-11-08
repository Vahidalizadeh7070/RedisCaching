using Microsoft.EntityFrameworkCore;

namespace RedisCaching.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 

        }
        public DbSet<News> News { get; set; }
    }
}