using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public interface IGlobalSearchService
{
    Task<List<SearchResult>> SearchAsync(string searchTerm);
}

public class SearchResult
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public SearchResultType Type { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string NavigationUrl { get; set; } = string.Empty;
}

public enum SearchResultType
{
    Customer,
    Booking,
    Boat
}
