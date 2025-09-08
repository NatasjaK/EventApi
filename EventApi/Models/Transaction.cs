namespace EventApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
