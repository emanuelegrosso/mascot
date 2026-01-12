using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public interface IBoatService
{
    Task<List<Boat>> GetAllBoatsAsync();
    Task<List<Boat>> GetActiveBoatsAsync();
    Task<Boat?> GetBoatByIdAsync(int id);
    Task<Boat> CreateBoatAsync(Boat boat);
    Task<Boat> UpdateBoatAsync(Boat boat);
    Task DeleteBoatAsync(int id);
    Task<bool> IsBoatAvailableAsync(int boatId, DateTime date);
}
