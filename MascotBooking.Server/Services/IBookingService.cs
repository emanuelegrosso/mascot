using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public interface IBookingService
{
    Task<List<Booking>> GetAllBookingsAsync();
    Task<Booking?> GetBookingByIdAsync(int id);
    Task<Booking> CreateBookingAsync(Booking booking);
    Task<Booking> UpdateBookingAsync(Booking booking);
    Task DeleteBookingAsync(int id);
    Task<List<Booking>> GetBookingsByDateAsync(DateTime date);
    Task<List<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<bool> RequiresNauticalLicenseAsync(BookingType type, int boatId, int? skipperId = null);
    Task MarkBookingDateAsReturnedAsync(int bookingId, DateTime date);
    Task<bool> CanTransitionToStatusAsync(int bookingId, BookingStatus newStatus);
    Task<List<BookingStatus>> GetValidStatusTransitionsAsync(int bookingId);
    Task UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus);
}
