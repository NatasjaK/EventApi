namespace EventApi.Models
{
    public class Merchandise
    {
        public int Id { get; set; }

        public int PackageId { get; set; }
        public Package Package { get; set; }

        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
