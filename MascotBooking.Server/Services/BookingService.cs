using Microsoft.EntityFrameworkCore;
using MascotBooking.Server.Data;
using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public class BookingService : IBookingService
{
    private readonly AppDbContext _context;

    public BookingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Booking>> GetAllBookingsAsync()
    {
        return await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Boat)
            .Include(b => b.Skipper)
            .Include(b => b.BookingDates)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<Booking?> GetBookingByIdAsync(int id)
    {
        return await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Boat)
            .Include(b => b.Skipper)
            .Include(b => b.BookingDates)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        booking.CreatedAt = DateTime.UtcNow;
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<Booking> UpdateBookingAsync(Booking booking)
    {
        booking.UpdatedAt = DateTime.UtcNow;
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task DeleteBookingAsync(int id)
    {
        var booking = await GetBookingByIdAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Booking>> GetBookingsByDateAsync(DateTime date)
    {
        return await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Boat)
            .Include(b => b.Skipper)
            .Include(b => b.BookingDates)
            .Where(b => b.BookingDates.Any(bd => bd.Date.Date == date.Date))
            .ToListAsync();
    }

    public async Task<List<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Boat)
            .Include(b => b.Skipper)
            .Include(b => b.BookingDates)
            .Where(b => b.BookingDates.Any(bd => bd.Date.Date >= startDate.Date && bd.Date.Date <= endDate.Date))
            .ToListAsync();
    }

    public async Task<bool> RequiresNauticalLicenseAsync(BookingType type, int boatId, int? skipperId = null)
    {
        if (type == BookingType.Escursione)
            return false;

        var boat = await _context.Boats.FindAsync(boatId);
        if (boat == null) return false;

        // Se gommone < 100 CV: non serve patente
        if (boat.Horsepower < 100)
            return false;

        // Se gommone ≥ 100 CV:
        // - Senza skipper: cliente deve avere patente
        // - Con skipper: cliente non ha bisogno di patente
        if (skipperId.HasValue)
        {
            // Verifica che skipper sia valido e attivo
            var skipper = await _context.Skippers.FindAsync(skipperId.Value);
            if (skipper != null && skipper.Attivo && skipper.DataScadenzaPatente >= DateTime.Today)
                return false; // Con skipper valido, cliente non ha bisogno di patente
        }

        // Senza skipper o skipper non valido: cliente deve avere patente
        return true;
    }

    public async Task MarkBookingDateAsReturnedAsync(int bookingId, DateTime date)
    {
        var booking = await GetBookingByIdAsync(bookingId);
        if (booking != null)
        {
            var bookingDate = booking.BookingDates.FirstOrDefault(bd => bd.Date.Date == date.Date);
            if (bookingDate != null)
            {
                bookingDate.IsReturned = true;
                bookingDate.ReturnedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task<bool> CanTransitionToStatusAsync(int bookingId, BookingStatus newStatus)
    {
        var booking = await GetBookingByIdAsync(bookingId);
        if (booking == null) return false;

        var validTransitions = await GetValidStatusTransitionsAsync(bookingId);
        return validTransitions.Contains(newStatus);
    }

    public async Task<List<BookingStatus>> GetValidStatusTransitionsAsync(int bookingId)
    {
        var booking = await GetBookingByIdAsync(bookingId);
        if (booking == null) return new List<BookingStatus>();

        var currentStatus = booking.Status;
        var validTransitions = new List<BookingStatus>();

        switch (currentStatus)
        {
            case BookingStatus.Opzionata:
                validTransitions.AddRange(new[] { BookingStatus.Confermata, BookingStatus.Annullata });
                break;

            case BookingStatus.Confermata:
                validTransitions.AddRange(new[] { BookingStatus.Pagata, BookingStatus.Annullata });
                break;

            case BookingStatus.Pagata:
                if (booking.Type == BookingType.Gommone)
                {
                    validTransitions.Add(BookingStatus.Ritirato);
                }
                else // Escursione
                {
                    validTransitions.Add(BookingStatus.Completata);
                }
                break;

            case BookingStatus.Ritirato:
                validTransitions.Add(BookingStatus.Rientrato);
                break;

            case BookingStatus.Rientrato:
                validTransitions.Add(BookingStatus.Completata);
                break;

            case BookingStatus.InAttesa:
                validTransitions.AddRange(new[] { BookingStatus.Opzionata, BookingStatus.Confermata, BookingStatus.Annullata });
                break;

            // Annullata e Completata sono terminali - nessuna transizione
            case BookingStatus.Annullata:
            case BookingStatus.Completata:
                break;
        }

        return validTransitions;
    }

    public async Task UpdateBookingStatusAsync(int bookingId, BookingStatus newStatus)
    {
        var booking = await GetBookingByIdAsync(bookingId);
        if (booking == null) return;

        // Valida transizione
        if (!await CanTransitionToStatusAsync(bookingId, newStatus))
        {
            throw new InvalidOperationException($"Transizione da {booking.Status} a {newStatus} non valida");
        }

        booking.Status = newStatus;
        booking.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}
