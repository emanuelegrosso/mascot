using Microsoft.EntityFrameworkCore;
using MascotBooking.Server.Data;
using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public class BoatService : IBoatService
{
    private readonly AppDbContext _context;

    public BoatService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Boat>> GetAllBoatsAsync()
    {
        return await _context.Boats
            .OrderBy(b => b.Type)
            .ThenBy(b => b.Horsepower)
            .ThenBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<List<Boat>> GetActiveBoatsAsync()
    {
        return await _context.Boats
            .Where(b => b.IsActive)
            .OrderBy(b => b.Type)
            .ThenBy(b => b.Horsepower)
            .ThenBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<Boat?> GetBoatByIdAsync(int id)
    {
        return await _context.Boats.FindAsync(id);
    }

    public async Task<Boat> CreateBoatAsync(Boat boat)
    {
        _context.Boats.Add(boat);
        await _context.SaveChangesAsync();
        return boat;
    }

    public async Task<Boat> UpdateBoatAsync(Boat boat)
    {
        _context.Boats.Update(boat);
        await _context.SaveChangesAsync();
        return boat;
    }

    public async Task DeleteBoatAsync(int id)
    {
        var boat = await GetBoatByIdAsync(id);
        if (boat == null)
        {
            throw new ArgumentException("Barca non trovata.");
        }

        // Controlla se ci sono prenotazioni associate
        var hasBookings = await _context.Bookings
            .AnyAsync(b => b.BoatId == id);

        if (hasBookings)
        {
            throw new InvalidOperationException("Impossibile eliminare la barca: ci sono prenotazioni associate. Disattiva la barca invece di eliminarla.");
        }

        // Controlla se ci sono prezzi stagionali associati (verranno eliminati automaticamente per cascade delete)
        // Ma è meglio essere espliciti
        var hasPrices = await _context.BoatPrices
            .AnyAsync(bp => bp.BoatId == id);

        // Rimuovi i prezzi se esistono (cascade delete gestito da EF)
        if (hasPrices)
        {
            var prices = await _context.BoatPrices
                .Where(bp => bp.BoatId == id)
                .ToListAsync();
            _context.BoatPrices.RemoveRange(prices);
        }

        _context.Boats.Remove(boat);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsBoatAvailableAsync(int boatId, DateTime date)
    {
        var boat = await GetBoatByIdAsync(boatId);
        if (boat == null || !boat.IsActive)
            return false;

        // Controlla se ci sono prenotazioni per questa data
        var hasBooking = await _context.Bookings
            .Include(b => b.BookingDates)
            .AnyAsync(b => b.BoatId == boatId && 
                          b.BookingDates.Any(bd => bd.Date == date && !bd.IsReturned));

        return !hasBooking;
    }
}
