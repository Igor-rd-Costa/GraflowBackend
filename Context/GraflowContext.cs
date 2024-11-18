

using Microsoft.EntityFrameworkCore;

namespace GraflowBackend.Context
{
    public class GraflowContext : DbContext
    {
        public GraflowContext(DbContextOptions<GraflowContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}