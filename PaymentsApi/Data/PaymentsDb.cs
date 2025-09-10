using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PaymentsApi.Models;

namespace PaymentsApi.Data
{
    public class PaymentsDb : DbContext
    {
        public PaymentsDb(DbContextOptions<PaymentsDb> opt) : base(opt) { }
        public DbSet<Transaction> Transactions => Set<Transaction>();
    }
}
