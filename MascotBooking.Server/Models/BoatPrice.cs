using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MascotBooking.Server.Models;

public class BoatPrice
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int BoatId { get; set; }

    [Required]
    [Range(1, 12)]
    public int Month { get; set; } // 1 = Gennaio, 12 = Dicembre

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    // true = prezzo per persona (escursioni)
    // false = prezzo giornaliero fisso (gommoni)
    public bool IsPerPerson { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigazione
    [ForeignKey("BoatId")]
    public Boat Boat { get; set; } = null!;
}
