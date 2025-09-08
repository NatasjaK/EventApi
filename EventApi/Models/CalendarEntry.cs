namespace EventApi.Models
{
    public class CalendarEntry
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public string Title { get; set; }
        public DateTime EndDate { get; set; }
    }
}
