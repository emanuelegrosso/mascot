using MascotBooking.Server.Models;

namespace MascotBooking.Server.Data;

/// <summary>
/// Servizio per importare dati da Notion o altre fonti esterne
/// </summary>
public static class NotionDataImporter
{
    /// <summary>
    /// Importa dati da un formato JSON strutturato (es. export da Notion)
    /// </summary>
    public static void ImportFromJson(AppDbContext context, string jsonData)
    {
        // TODO: Implementare parsing JSON e importazione
        // Esempio struttura attesa:
        /*
        {
            "customers": [
                {
                    "phone": "3331234567",
                    "email": "email@example.com",
                    "fullName": "Nome Cognome",
                    ...
                }
            ],
            "boats": [...],
            "bookings": [...]
        }
        */
    }

    /// <summary>
    /// Importa dati da CSV
    /// </summary>
    public static void ImportFromCsv(AppDbContext context, string csvData)
    {
        // TODO: Implementare parsing CSV
    }

    /// <summary>
    /// Crea clienti da lista strutturata
    /// </summary>
    public static List<Customer> CreateCustomersFromList(List<Dictionary<string, string>> customerData)
    {
        var customers = new List<Customer>();

        foreach (var data in customerData)
        {
            var customer = new Customer
            {
                Phone = data.GetValueOrDefault("phone", "") ?? "",
                Email = data.GetValueOrDefault("email"),
                FullName = data.GetValueOrDefault("fullName"),
                DateOfBirth = ParseDate(data.GetValueOrDefault("dateOfBirth")),
                PlaceOfBirth = data.GetValueOrDefault("placeOfBirth"),
                Address = data.GetValueOrDefault("address"),
                PostalCode = data.GetValueOrDefault("postalCode"),
                City = data.GetValueOrDefault("city"),
                Country = data.GetValueOrDefault("country", "Italia"),
                TaxCode = data.GetValueOrDefault("taxCode"),
                DocumentNumber = data.GetValueOrDefault("documentNumber"),
                DocumentType = ParseDocumentType(data.GetValueOrDefault("documentType")),
                HasDocument = bool.Parse(data.GetValueOrDefault("hasDocument", "false") ?? "false"),
                HasNauticalLicense = bool.Parse(data.GetValueOrDefault("hasNauticalLicense", "false") ?? "false"),
                CreatedAt = DateTime.UtcNow
            };

            customers.Add(customer);
        }

        return customers;
    }

    /// <summary>
    /// Crea barche da lista strutturata
    /// </summary>
    public static List<Boat> CreateBoatsFromList(List<Dictionary<string, string>> boatData)
    {
        var boats = new List<Boat>();

        foreach (var data in boatData)
        {
            var boat = new Boat
            {
                Name = data.GetValueOrDefault("name", "") ?? "",
                Type = ParseBoatType(data.GetValueOrDefault("type", "Gommone") ?? "Gommone"),
                Horsepower = int.Parse(data.GetValueOrDefault("horsepower", "0") ?? "0"),
                Capacity = int.Parse(data.GetValueOrDefault("capacity", "0") ?? "0"),
                DailyPrice = ParseDecimal(data.GetValueOrDefault("dailyPrice")),
                DailyPriceHigh = ParseDecimal(data.GetValueOrDefault("dailyPriceHigh")),
                AdultPrice = ParseDecimal(data.GetValueOrDefault("adultPrice")),
                ChildPrice = ParseDecimal(data.GetValueOrDefault("childPrice")),
                IsActive = bool.Parse(data.GetValueOrDefault("isActive", "true") ?? "true")
            };

            boats.Add(boat);
        }

        return boats;
    }

    private static DateTime? ParseDate(string? dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
            return null;

        if (DateTime.TryParse(dateString, out DateTime date))
            return date;

        return null;
    }

    private static DocumentType? ParseDocumentType(string? type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return null;

        return type.ToLower() switch
        {
            "carta d'identità" or "cartaidentita" or "ci" => DocumentType.CartaIdentita,
            "passaporto" or "passport" => DocumentType.Passaporto,
            "patente di guida" or "patenteguida" or "patente" => DocumentType.PatenteGuida,
            _ => null
        };
    }

    private static BoatType ParseBoatType(string type)
    {
        return type.ToLower() switch
        {
            "yacht" or "barca" => BoatType.Yacht,
            _ => BoatType.Gommone
        };
    }

    private static decimal? ParseDecimal(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (decimal.TryParse(value, out decimal result))
            return result;

        return null;
    }
}
