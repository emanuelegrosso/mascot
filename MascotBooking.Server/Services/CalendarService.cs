using Microsoft.EntityFrameworkCore;
using MascotBooking.Server.Data;
using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public class CalendarService : ICalendarService
{
    private readonly AppDbContext _context;

    public CalendarService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, List<Booking>>> GetDailyViewAsync(DateTime date)
    {
        var bookings = await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Boat)
            .Include(b => b.BookingDates)
            .Where(b => b.BookingDates.Any(bd => bd.Date.Date == date.Date))
            .ToListAsync();

        var result = new Dictionary<string, List<Booking>>();
        foreach (var booking in bookings)
        {
            var key = $"{booking.Boat.Name} ({booking.Boat.Horsepower} CV)";
            if (!result.ContainsKey(key))
                result[key] = new List<Booking>();
            result[key].Add(booking);
        }

        return result;
    }

    public async Task<Dictionary<string, List<Booking>>> GetWeeklyViewAsync(DateTime startDate)
    {
        var endDate = startDate.AddDays(6);
        var bookings = await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Boat)
            .Include(b => b.BookingDates)
            .Where(b => b.BookingDates.Any(bd => bd.Date.Date >= startDate.Date && bd.Date.Date <= endDate.Date))
            .ToListAsync();

        var result = new Dictionary<string, List<Booking>>();
        foreach (var booking in bookings)
        {
            var key = $"{booking.Boat.Name} ({booking.Boat.Horsepower} CV)";
            if (!result.ContainsKey(key))
                result[key] = new List<Booking>();
            result[key].Add(booking);
        }

        return result;
    }

    public async Task<Dictionary<DateTime, int>> GetMonthlyViewAsync(DateTime month)
    {
        var startDate = new DateTime(month.Year, month.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var bookings = await _context.Bookings
            .Include(b => b.BookingDates)
            .Where(b => b.BookingDates.Any(bd => bd.Date.Date >= startDate.Date && bd.Date.Date <= endDate.Date))
            .ToListAsync();

        var result = new Dictionary<DateTime, int>();
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var count = bookings.Count(b => b.BookingDates.Any(bd => bd.Date.Date == date.Date));
            result[date] = count;
        }

        return result;
    }

    public async Task<Dictionary<int, int>> GetAnnualViewAsync(int year)
    {
        var startDate = new DateTime(year, 1, 1);
        var endDate = new DateTime(year, 12, 31);

        var bookings = await _context.Bookings
            .Include(b => b.BookingDates)
            .Where(b => b.BookingDates.Any(bd => bd.Date.Date >= startDate.Date && bd.Date.Date <= endDate.Date))
            .ToListAsync();

        var result = new Dictionary<int, int>();
        for (int month = 1; month <= 12; month++)
        {
            var count = bookings.Count(b => b.BookingDates.Any(bd => bd.Date.Year == year && bd.Date.Month == month));
            result[month] = count;
        }

        return result;
    }

    public async Task<List<Booking>> GetBookingsForTimeSlotAsync(DateTime date, TimeSpan? startTime, TimeSpan? endTime)
    {
        var query = _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Boat)
            .Include(b => b.BookingDates)
            .Where(b => b.BookingDates.Any(bd => bd.Date.Date == date.Date));

        if (startTime.HasValue)
        {
            query = query.Where(b => b.BookingDates.Any(bd => 
                bd.StartTime.HasValue && bd.StartTime.Value >= startTime.Value));
        }

        if (endTime.HasValue)
        {
            query = query.Where(b => b.BookingDates.Any(bd => 
                bd.EndTime.HasValue && bd.EndTime.Value <= endTime.Value));
        }

        return await query.ToListAsync();
    }
}
