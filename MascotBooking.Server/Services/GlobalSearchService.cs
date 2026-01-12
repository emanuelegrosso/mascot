using Microsoft.EntityFrameworkCore;
using MascotBooking.Server.Data;
using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public class GlobalSearchService : IGlobalSearchService
{
    private readonly AppDbContext _context;

    public GlobalSearchService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SearchResult>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 2)
            return new List<SearchResult>();

        var term = searchTerm.ToLower();
        var results = new List<SearchResult>();

        // Search Customers
        var customers = await _context.Customers
            .Where(c => 
                (c.FullName != null && c.FullName.ToLower().Contains(term)) ||
                c.Phone.Contains(term) ||
                (c.Email != null && c.Email.ToLower().Contains(term)) ||
                (c.TaxCode != null && c.TaxCode.ToLower().Contains(term)))
            .Take(5)
            .ToListAsync();

        foreach (var customer in customers)
        {
            results.Add(new SearchResult
            {
                Id = customer.Id.ToString(),
                Title = customer.FullName ?? customer.Phone,
                Subtitle = $"Telefono: {customer.Phone}" + (customer.Email != null ? $" | Email: {customer.Email}" : ""),
                Type = SearchResultType.Customer,
                Icon = "oi-person",
                NavigationUrl = $"/customer?customerId={customer.Id}"
            });
        }

        // Search Bookings
        var bookings = await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Boat)
            .Where(b => 
                b.Id.ToString().Contains(term) ||
                (b.Customer.FullName != null && b.Customer.FullName.ToLower().Contains(term)) ||
                b.Customer.Phone.Contains(term) ||
                b.Boat.Name.ToLower().Contains(term) ||
                (b.Notes != null && b.Notes.ToLower().Contains(term)))
            .OrderByDescending(b => b.CreatedAt)
            .Take(5)
            .ToListAsync();

        foreach (var booking in bookings)
        {
            var dateStr = booking.BookingDates.FirstOrDefault()?.Date.ToString("dd/MM/yyyy") ?? "N/A";
            results.Add(new SearchResult
            {
                Id = booking.Id.ToString(),
                Title = $"Prenotazione #{booking.Id} - {booking.Customer.FullName}",
                Subtitle = $"{booking.Type} | {booking.Boat.Name} | {dateStr}",
                Type = SearchResultType.Booking,
                Icon = "oi-calendar",
                NavigationUrl = $"/calendar?bookingId={booking.Id}"
            });
        }

        // Search Boats
        var boats = await _context.Boats
            .Where(b => 
                b.Name.ToLower().Contains(term) ||
                b.Horsepower.ToString().Contains(term) ||
                b.Capacity.ToString().Contains(term))
            .Take(5)
            .ToListAsync();

        foreach (var boat in boats)
        {
            results.Add(new SearchResult
            {
                Id = boat.Id.ToString(),
                Title = boat.Name,
                Subtitle = $"{boat.Horsepower} CV | {boat.Capacity} posti | {boat.Type}",
                Type = SearchResultType.Boat,
                Icon = boat.Type == BoatType.Gommone ? "oi-boat" : "oi-star",
                NavigationUrl = $"/booking?boatId={boat.Id}"
            });
        }

        return results.OrderBy(r => r.Type).ThenBy(r => r.Title).ToList();
    }
}
