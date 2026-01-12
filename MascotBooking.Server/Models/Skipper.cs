using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MascotBooking.Server.Models;

public class Skipper
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Cognome { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Email { get; set; }

    [Required]
    [StringLength(50)]
    public string NumeroDocumento { get; set; } = string.Empty;

    [Required]
    public DocumentType TipoDocumento { get; set; }

    [Required]
    [StringLength(50)]
    public string NumeroPatenteNautica { get; set; } = string.Empty;

    [Required]
    public DateTime DataScadenzaPatente { get; set; }

    [StringLength(1000)]
    public string? Note { get; set; }

    [Required]
    public bool Attivo { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigazione
    public List<Booking> Bookings { get; set; } = new();

    // Proprietà calcolate
    [NotMapped]
    public string FullName => $"{Nome} {Cognome}";

    [NotMapped]
    public bool PatenteValida => DataScadenzaPatente >= DateTime.Today;

    [NotMapped]
    public bool PatenteInScadenza => DataScadenzaPatente >= DateTime.Today && 
                                     DataScadenzaPatente <= DateTime.Today.AddDays(30);
}
