using Microsoft.EntityFrameworkCore;
using MascotBooking.Server.Data;
using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public class SkipperService : ISkipperService
{
    private readonly AppDbContext _context;

    public SkipperService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Skipper>> GetAllSkippersAsync()
    {
        return await _context.Skippers
            .OrderBy(s => s.Cognome)
            .ThenBy(s => s.Nome)
            .ToListAsync();
    }

    public async Task<List<Skipper>> GetActiveSkippersAsync()
    {
        return await _context.Skippers
            .Where(s => s.Attivo && s.DataScadenzaPatente >= DateTime.Today)
            .OrderBy(s => s.Cognome)
            .ThenBy(s => s.Nome)
            .ToListAsync();
    }

    public async Task<Skipper?> GetSkipperByIdAsync(int id)
    {
        return await _context.Skippers.FindAsync(id);
    }

    public async Task<Skipper> CreateSkipperAsync(Skipper skipper)
    {
        skipper.CreatedAt = DateTime.UtcNow;
        _context.Skippers.Add(skipper);
        await _context.SaveChangesAsync();
        return skipper;
    }

    public async Task<Skipper> UpdateSkipperAsync(Skipper skipper)
    {
        skipper.UpdatedAt = DateTime.UtcNow;
        _context.Skippers.Update(skipper);
        await _context.SaveChangesAsync();
        return skipper;
    }

    public async Task DeleteSkipperAsync(int id)
    {
        var skipper = await GetSkipperByIdAsync(id);
        if (skipper != null)
        {
            _context.Skippers.Remove(skipper);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsSkipperAvailableAsync(int skipperId, DateTime date)
    {
        // Per ora controllo solo se è attivo e ha patente valida
        // In futuro si può aggiungere controllo prenotazioni sovrapposte
        var skipper = await GetSkipperByIdAsync(skipperId);
        if (skipper == null || !skipper.Attivo)
            return false;

        return skipper.DataScadenzaPatente >= date;
    }

    public async Task<bool> IsSkipperLicenseValidAsync(int skipperId)
    {
        var skipper = await GetSkipperByIdAsync(skipperId);
        return skipper != null && skipper.DataScadenzaPatente >= DateTime.Today;
    }

    public async Task<List<Skipper>> GetSkippersWithExpiringLicenseAsync(int daysThreshold = 30)
    {
        var thresholdDate = DateTime.Today.AddDays(daysThreshold);
        return await _context.Skippers
            .Where(s => s.DataScadenzaPatente >= DateTime.Today && 
                       s.DataScadenzaPatente <= thresholdDate)
            .OrderBy(s => s.DataScadenzaPatente)
            .ToListAsync();
    }
}
