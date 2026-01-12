using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MascotBooking.Server.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(100)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(200)]
    public string? FullName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [StringLength(100)]
    public string? PlaceOfBirth { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(10)]
    public string? PostalCode { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? Country { get; set; }

    [StringLength(16)]
    public string? TaxCode { get; set; }

    [StringLength(50)]
    public string? DocumentNumber { get; set; }

    public DocumentType? DocumentType { get; set; }

    public bool HasDocument { get; set; }

    public bool HasNauticalLicense { get; set; }

    [StringLength(500)]
    public string? DocumentFileUrl { get; set; }

    [StringLength(500)]
    public string? LicenseFileUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigazione
    public List<Booking> Bookings { get; set; } = new();
}
