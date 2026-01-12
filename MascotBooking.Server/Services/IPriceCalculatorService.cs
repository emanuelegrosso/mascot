using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public interface IPriceCalculatorService
{
    Task<decimal> CalculateStandardPriceAsync(
        BookingType type,
        int boatId,
        List<DateTime> dates,
        int adults,
        int? children = null,
        int? skipperId = null);
    
    bool IsHighSeason(DateTime date);
    
    Task<decimal> GetSkipperDailyPriceAsync(int skipperId);
}
