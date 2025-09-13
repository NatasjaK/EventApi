using Microsoft.EntityFrameworkCore;
using PaymentsApi.Models;

namespace PaymentsApi.Data
{
    public class PaymentsDb : DbContext
    {
        public PaymentsDb(DbContextOptions<PaymentsDb> opt) : base(opt) { }

        public DbSet<Transaction> Transactions => Set<Transaction>();
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<decimal>().HavePrecision(18, 2);
        }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Transaction>(e =>
            {
                e.HasIndex(x => x.Date);
                e.HasIndex(x => x.EventId);
            });
        }
    }
}
