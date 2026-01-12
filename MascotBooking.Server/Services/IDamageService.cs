using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public interface IDamageService
{
    Task<List<Damage>> GetDamagesByDateAsync(DateTime date);
    Task<List<Damage>> GetAllDamagesAsync();
    Task<Damage?> GetDamageByIdAsync(int id);
    Task<Damage> CreateDamageAsync(Damage damage);
    Task<Damage> UpdateDamageAsync(Damage damage);
    Task DeleteDamageAsync(int id);
    Task<decimal> GetTotalDamageCostByDateAsync(DateTime date);
}
