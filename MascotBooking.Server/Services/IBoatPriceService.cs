using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public interface IBoatPriceService
{
    Task<List<BoatPrice>> GetPricesByBoatAsync(int boatId);
    Task<BoatPrice?> GetPriceForMonthAsync(int boatId, int month);
    Task<decimal> GetPriceForDateAsync(int boatId, DateTime date, bool isPerPerson = false);
    Task<BoatPrice> CreateOrUpdatePriceAsync(BoatPrice price);
    Task DeletePriceAsync(int priceId);
    Task BulkUpdatePricesAsync(int boatId, Dictionary<int, decimal> monthlyPrices, bool isPerPerson = false);
    Task<Dictionary<int, decimal>> GetPriceMatrixForBoatAsync(int boatId, bool isPerPerson = false);
}
