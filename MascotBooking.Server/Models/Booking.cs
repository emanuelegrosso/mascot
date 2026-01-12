using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MascotBooking.Server.Models;

public class Booking
{
    [Key]
    public int Id { get; set; }

    [Required]
    public BookingType Type { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int BoatId { get; set; }

    [Required]
    public int Adults { get; set; }

    public int? Children { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal StandardPrice { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal ActualPrice { get; set; }

    [Required]
    public BookingStatus Status { get; set; } = BookingStatus.Confermata;

    [StringLength(1000)]
    public string? Notes { get; set; }

    public int? SkipperId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigazione
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; } = null!;

    [ForeignKey("BoatId")]
    public Boat Boat { get; set; } = null!;

    [ForeignKey("SkipperId")]
    public Skipper? Skipper { get; set; }

    // Date multiple della prenotazione
    public List<BookingDate> BookingDates { get; set; } = new();
}

public class BookingDate
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int BookingId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public bool IsReturned { get; set; } = false;

    public DateTime? ReturnedAt { get; set; }

    // Navigazione
    [ForeignKey("BookingId")]
    public Booking Booking { get; set; } = null!;
}
