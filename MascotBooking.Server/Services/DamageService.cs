using Microsoft.EntityFrameworkCore;
using MascotBooking.Server.Data;
using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public class DamageService : IDamageService
{
    private readonly AppDbContext _context;

    public DamageService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Damage>> GetDamagesByDateAsync(DateTime date)
    {
        return await _context.Damages
            .Where(d => d.Date.Date == date.Date)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Damage>> GetAllDamagesAsync()
    {
        return await _context.Damages
            .OrderByDescending(d => d.Date)
            .ThenByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<Damage?> GetDamageByIdAsync(int id)
    {
        return await _context.Damages.FindAsync(id);
    }

    public async Task<Damage> CreateDamageAsync(Damage damage)
    {
        damage.CreatedAt = DateTime.UtcNow;
        _context.Damages.Add(damage);
        await _context.SaveChangesAsync();
        return damage;
    }

    public async Task<Damage> UpdateDamageAsync(Damage damage)
    {
        damage.UpdatedAt = DateTime.UtcNow;
        _context.Damages.Update(damage);
        await _context.SaveChangesAsync();
        return damage;
    }

    public async Task DeleteDamageAsync(int id)
    {
        var damage = await GetDamageByIdAsync(id);
        if (damage != null)
        {
            _context.Damages.Remove(damage);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<decimal> GetTotalDamageCostByDateAsync(DateTime date)
    {
        return await _context.Damages
            .Where(d => d.Date.Date == date.Date)
            .SumAsync(d => d.Cost);
    }
}
