using Microsoft.EntityFrameworkCore;

namespace PaymentsApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

}
