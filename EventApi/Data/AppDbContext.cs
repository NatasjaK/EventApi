using EventApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;
        public DbSet<Venue> Venues { get; set; } = null!;
        public DbSet<Package> Packages { get; set; } = null!;
        public DbSet<Merchandise> Merchandises { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<CalendarEntry> CalendarEntries { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<decimal>().HavePrecision(18, 2);
        }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Event>().HasIndex(e => e.Date);
            b.Entity<Booking>().HasIndex(x => x.BookingDate);

            b.Entity<Booking>()
                .HasOne(bk => bk.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(bk => bk.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Entity<Booking>()
                .HasOne(bk => bk.Event)
                .WithMany(e => e.Bookings)
                .HasForeignKey(bk => bk.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Ticket>()
                .HasOne(t => t.Booking)
                .WithMany(bk => bk.Tickets)
                .HasForeignKey(t => t.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId)          
                .OnDelete(DeleteBehavior.ClientSetNull);

            b.Entity<Invoice>()
                .HasOne(i => i.User)
                .WithMany(u => u.Invoices)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Entity<Invoice>()
                .HasOne(i => i.Event)
                .WithMany(e => e.Invoices)
                .HasForeignKey(i => i.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Entity<Feedback>()
                .HasOne(f => f.Event)
                .WithMany(e => e.Feedbacks)
                .HasForeignKey(f => f.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Package>()
                .HasOne(p => p.Event)
                .WithMany(e => e.Packages)
                .HasForeignKey(p => p.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Merchandise>()
                .HasOne(m => m.Package)
                .WithMany(p => p.Merchandises)
                .HasForeignKey(m => m.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<CalendarEntry>()
                .HasOne(c => c.Event)
                .WithMany(e => e.CalendarEntries)
                .HasForeignKey(c => c.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany(v => v.Events)
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.SetNull);

            b.Entity<Transaction>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Transactions)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Entity<Message>().Property(m => m.IsRead).HasDefaultValue(false);
            b.Entity<Booking>().Property(bk => bk.Status).HasDefaultValue("Pending");

            b.Entity<User>().Property(u => u.Name)
                .HasMaxLength(120).IsRequired();

            b.Entity<Event>().Property(e => e.Title)
                .HasMaxLength(200).IsRequired();

            b.Entity<Venue>().Property(v => v.Name)
                .HasMaxLength(150).IsRequired();

            b.Entity<Ticket>().Property(t => t.SeatNumber)
                .HasMaxLength(20);
        }
    }
}
