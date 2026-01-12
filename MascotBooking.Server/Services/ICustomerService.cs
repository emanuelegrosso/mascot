using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public interface ICustomerService
{
    Task<List<Customer>> GetAllCustomersAsync();
    Task<Customer?> GetCustomerByIdAsync(int id);
    Task<Customer?> GetCustomerByPhoneAsync(string phone);
    Task<Customer> CreateCustomerAsync(Customer customer);
    Task<Customer> UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(int id);
    Task<List<Customer>> SearchCustomersAsync(string searchTerm);
    Task<Booking?> GetLastBookingAsync(int customerId);
}
