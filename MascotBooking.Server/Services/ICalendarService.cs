using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public interface ICalendarService
{
    Task<Dictionary<string, List<Booking>>> GetDailyViewAsync(DateTime date);
    Task<Dictionary<string, List<Booking>>> GetWeeklyViewAsync(DateTime startDate);
    Task<Dictionary<DateTime, int>> GetMonthlyViewAsync(DateTime month);
    Task<Dictionary<int, int>> GetAnnualViewAsync(int year);
    Task<List<Booking>> GetBookingsForTimeSlotAsync(DateTime date, TimeSpan? startTime, TimeSpan? endTime);
}
