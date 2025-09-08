namespace EventApi.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public decimal Amount { get; set; }
        public string InvoiceNumber { get; set; }
    }
}
