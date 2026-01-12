using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public interface ISkipperService
{
    Task<List<Skipper>> GetAllSkippersAsync();
    Task<List<Skipper>> GetActiveSkippersAsync();
    Task<Skipper?> GetSkipperByIdAsync(int id);
    Task<Skipper> CreateSkipperAsync(Skipper skipper);
    Task<Skipper> UpdateSkipperAsync(Skipper skipper);
    Task DeleteSkipperAsync(int id);
    Task<bool> IsSkipperAvailableAsync(int skipperId, DateTime date);
    Task<bool> IsSkipperLicenseValidAsync(int skipperId);
    Task<List<Skipper>> GetSkippersWithExpiringLicenseAsync(int daysThreshold = 30);
}
