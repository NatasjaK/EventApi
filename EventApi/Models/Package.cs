namespace EventApi.Models
{
    public class Package
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public string Title { get; set; }
        public decimal Price { get; set; }

        public ICollection<Merchandise> Merchandises { get; set; }
    }
}
