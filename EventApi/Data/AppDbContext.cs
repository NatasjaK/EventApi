using EventApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Merchandise> Merchandises { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<CalendarEntry> CalendarEntries { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany(e => e.Bookings)
                .HasForeignKey(b => b.EventId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Booking)
                .WithMany(b => b.Tickets)
                .HasForeignKey(t => t.BookingId);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.User)
                .WithMany(u => u.Invoices)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Event)
                .WithMany(e => e.Invoices)
                .HasForeignKey(i => i.EventId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Event)
                .WithMany(e => e.Feedbacks)
                .HasForeignKey(f => f.EventId);

            modelBuilder.Entity<Package>()
                .HasOne(p => p.Event)
                .WithMany(e => e.Packages)
                .HasForeignKey(p => p.EventId);

            modelBuilder.Entity<Merchandise>()
                .HasOne(m => m.Package)
                .WithMany(p => p.Merchandises)
                .HasForeignKey(m => m.PackageId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<CalendarEntry>()
                .HasOne(c => c.Event)
                .WithMany(e => e.CalendarEntries)
                .HasForeignKey(c => c.EventId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Transactions)
                .HasForeignKey(t => t.EventId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany(v => v.Events)
                .HasForeignKey(e => e.VenueId);
        }
    }
}
