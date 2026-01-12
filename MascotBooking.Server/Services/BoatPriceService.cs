using Microsoft.EntityFrameworkCore;
using MascotBooking.Server.Data;
using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public class BoatPriceService : IBoatPriceService
{
    private readonly AppDbContext _context;

    public BoatPriceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BoatPrice>> GetPricesByBoatAsync(int boatId)
    {
        return await _context.BoatPrices
            .Where(p => p.BoatId == boatId)
            .OrderBy(p => p.Month)
            .ToListAsync();
    }

    public async Task<BoatPrice?> GetPriceForMonthAsync(int boatId, int month)
    {
        return await _context.BoatPrices
            .FirstOrDefaultAsync(p => p.BoatId == boatId && p.Month == month);
    }

    public async Task<decimal> GetPriceForDateAsync(int boatId, DateTime date, bool isPerPerson = false)
    {
        var month = date.Month;
        var price = await _context.BoatPrices
            .FirstOrDefaultAsync(p => p.BoatId == boatId && 
                                 p.Month == month && 
                                 p.IsPerPerson == isPerPerson);

        if (price != null)
            return price.Price;

        // Fallback: usa DailyPrice o AdultPrice dal modello Boat
        var boat = await _context.Boats.FindAsync(boatId);
        if (boat == null) return 0;

        if (isPerPerson)
        {
            return boat.AdultPrice ?? 0;
        }
        else
        {
            // Usa DailyPriceHigh se alta stagione, altrimenti DailyPrice
            var isHighSeason = date.Month >= 4 && date.Month <= 9;
            return isHighSeason && boat.DailyPriceHigh.HasValue
                ? boat.DailyPriceHigh.Value
                : boat.DailyPrice ?? 0;
        }
    }

    public async Task<BoatPrice> CreateOrUpdatePriceAsync(BoatPrice price)
    {
        var existing = await _context.BoatPrices
            .FirstOrDefaultAsync(p => p.BoatId == price.BoatId && 
                                 p.Month == price.Month && 
                                 p.IsPerPerson == price.IsPerPerson);

        if (existing != null)
        {
            existing.Price = price.Price;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.BoatPrices.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }
        else
        {
            price.CreatedAt = DateTime.UtcNow;
            _context.BoatPrices.Add(price);
            await _context.SaveChangesAsync();
            return price;
        }
    }

    public async Task DeletePriceAsync(int priceId)
    {
        var price = await _context.BoatPrices.FindAsync(priceId);
        if (price != null)
        {
            _context.BoatPrices.Remove(price);
            await _context.SaveChangesAsync();
        }
    }

    public async Task BulkUpdatePricesAsync(int boatId, Dictionary<int, decimal> monthlyPrices, bool isPerPerson = false)
    {
        foreach (var kvp in monthlyPrices)
        {
            var price = new BoatPrice
            {
                BoatId = boatId,
                Month = kvp.Key,
                Price = kvp.Value,
                IsPerPerson = isPerPerson
            };
            await CreateOrUpdatePriceAsync(price);
        }
    }

    public async Task<Dictionary<int, decimal>> GetPriceMatrixForBoatAsync(int boatId, bool isPerPerson = false)
    {
        var prices = await _context.BoatPrices
            .Where(p => p.BoatId == boatId && p.IsPerPerson == isPerPerson)
            .ToListAsync();

        var matrix = new Dictionary<int, decimal>();
        for (int month = 1; month <= 12; month++)
        {
            var price = prices.FirstOrDefault(p => p.Month == month);
            matrix[month] = price?.Price ?? 0;
        }

        return matrix;
    }
}
