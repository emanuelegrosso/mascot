using Microsoft.EntityFrameworkCore;
using MascotBooking.Server.Data;
using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .OrderBy(c => c.FullName ?? c.Phone)
            .ToListAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.Bookings)
                .ThenInclude(b => b.Boat)
            .Include(c => c.Bookings)
                .ThenInclude(b => b.BookingDates)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer?> GetCustomerByPhoneAsync(string phone)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Phone == phone);
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        customer.CreatedAt = DateTime.UtcNow;
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await GetCustomerByIdAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Customer>> SearchCustomersAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        return await _context.Customers
            .Where(c => 
                (c.FullName != null && c.FullName.ToLower().Contains(term)) ||
                c.Phone.Contains(term) ||
                (c.Email != null && c.Email.ToLower().Contains(term)) ||
                (c.TaxCode != null && c.TaxCode.ToLower().Contains(term)))
            .ToListAsync();
    }

    public async Task<Booking?> GetLastBookingAsync(int customerId)
    {
        return await _context.Bookings
            .Include(b => b.Boat)
            .Include(b => b.BookingDates)
            .Where(b => b.CustomerId == customerId)
            .OrderByDescending(b => b.CreatedAt)
            .FirstOrDefaultAsync();
    }
}
