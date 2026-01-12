using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MascotBooking.Server.Models;

public class Boat
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public BoatType Type { get; set; }

    [Required]
    public int Horsepower { get; set; }

    [Required]
    public int Capacity { get; set; }

    // Prezzi per escursioni (per persona)
    public decimal? AdultPrice { get; set; }
    public decimal? ChildPrice { get; set; }

    // Prezzo per gommoni (giornaliero fisso)
    public decimal? DailyPrice { get; set; }

    // Prezzo alta stagione (opzionale)
    public decimal? DailyPriceHigh { get; set; }

    [NotMapped]
    public bool RequiresLicense => Horsepower >= 100;

    public bool IsActive { get; set; } = true;

    // Navigazione
    public List<Booking> Bookings { get; set; } = new();
    public List<BoatPrice> Prices { get; set; } = new();
}
