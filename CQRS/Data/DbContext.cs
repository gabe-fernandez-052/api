using Microsoft.EntityFrameworkCore;

namespace CQRS.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options)
            : base(options) { }

        public DbSet<Player> Players { get; set; }
    }
}