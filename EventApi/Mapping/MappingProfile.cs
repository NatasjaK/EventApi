// AI-generated helper
using AutoMapper;
using EventApi.Data;
using EventApi.Dtos;
using EventApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Users
        CreateMap<User, UserReadDto>()
            .ForMember(d => d.BookingCount, o => o.MapFrom(s => s.Bookings.Count))
            .ForMember(d => d.InvoiceCount, o => o.MapFrom(s => s.Invoices.Count));
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();

        // Venues
        CreateMap<Venue, VenueReadDto>()
            .ForMember(d => d.EventCount, o => o.MapFrom(s => s.Events.Count));
        CreateMap<VenueCreateDto, Venue>();
        CreateMap<VenueUpdateDto, Venue>();

        // Events
        CreateMap<Event, EventReadDto>()
            .ForMember(d => d.VenueName, o => o.MapFrom(s => s.Venue != null ? s.Venue.Name : null));
        CreateMap<EventCreateDto, Event>();
        CreateMap<EventUpdateDto, Event>();

        // Bookings
        CreateMap<Booking, BookingReadDto>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.User != null ? s.User.Name : null))
            .ForMember(d => d.EventTitle, o => o.MapFrom(s => s.Event != null ? s.Event.Title : null))
            .ForMember(d => d.TicketCount, o => o.MapFrom(s => s.Tickets.Count));
        CreateMap<BookingCreateDto, Booking>();
        CreateMap<BookingUpdateDto, Booking>();

        // Tickets
        CreateMap<Ticket, TicketReadDto>();
        CreateMap<TicketCreateDto, Ticket>();
        CreateMap<TicketUpdateDto, Ticket>();

        // Invoices
        CreateMap<Invoice, InvoiceReadDto>()
            .ForMember(d => d.UserEmail, o => o.MapFrom(s => s.User != null ? s.User.Email : null))
            .ForMember(d => d.EventTitle, o => o.MapFrom(s => s.Event != null ? s.Event.Title : null));
        CreateMap<InvoiceCreateDto, Invoice>();
        CreateMap<InvoiceUpdateDto, Invoice>();

        // Feedbacks
        CreateMap<Feedback, FeedbackReadDto>()
            .ForMember(d => d.EventTitle, o => o.MapFrom(s => s.Event != null ? s.Event.Title : null));
        CreateMap<FeedbackCreateDto, Feedback>();
        CreateMap<FeedbackUpdateDto, Feedback>();

        // Packages
        CreateMap<Package, PackageReadDto>()
            .ForMember(d => d.MerchandiseCount, o => o.MapFrom(s => s.Merchandises.Count));
        CreateMap<PackageCreateDto, Package>();
        CreateMap<PackageUpdateDto, Package>();

        // Merchandises
        CreateMap<Merchandise, MerchandiseReadDto>();
        CreateMap<MerchandiseCreateDto, Merchandise>();
        CreateMap<MerchandiseUpdateDto, Merchandise>();

        // Messages
        CreateMap<Message, MessageReadDto>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.User != null ? s.User.Name : null));
        CreateMap<MessageCreateDto, Message>();
        CreateMap<MessageUpdateDto, Message>();

        // CalendarEntries
        CreateMap<CalendarEntry, CalendarEntryReadDto>()
            .ForMember(d => d.EventTitle, o => o.MapFrom(s => s.Event != null ? s.Event.Title : null));
        CreateMap<CalendarEntryCreateDto, CalendarEntry>();
        CreateMap<CalendarEntryUpdateDto, CalendarEntry>();

        // Transactions
        CreateMap<Transaction, TransactionReadDto>()
            .ForMember(d => d.EventTitle, o => o.MapFrom(s => s.Event != null ? s.Event.Title : null));
        CreateMap<TransactionCreateDto, Transaction>();
        CreateMap<TransactionUpdateDto, Transaction>();
    }
}
