using Microsoft.EntityFrameworkCore;
using MascotBooking.Server.Data;
using MascotBooking.Server.Models;

namespace MascotBooking.Server.Services;

public class PriceCalculatorService : IPriceCalculatorService
{
    private readonly AppDbContext _context;
    private readonly IBoatPriceService _boatPriceService;

    public PriceCalculatorService(AppDbContext context, IBoatPriceService boatPriceService)
    {
        _context = context;
        _boatPriceService = boatPriceService;
    }

    public async Task<decimal> CalculateStandardPriceAsync(
        BookingType type,
        int boatId,
        List<DateTime> dates,
        int adults,
        int? children = null,
        int? skipperId = null)
    {
        var boat = await _context.Boats.FindAsync(boatId);
        if (boat == null) return 0;

        decimal total = 0;

        foreach (var date in dates)
        {
            if (type == BookingType.Escursione)
            {
                // Prezzo per persona - cerca prima nei prezzi stagionali
                var adultPrice = await _boatPriceService.GetPriceForDateAsync(boatId, date, isPerPerson: true);
                
                if (adultPrice > 0)
                {
                    total += adultPrice * adults;
                    
                    // Per bambini usa metà prezzo adulto se non specificato
                    if (children.HasValue && children.Value > 0)
                    {
                        var childPrice = await _boatPriceService.GetPriceForDateAsync(boatId, date, isPerPerson: true);
                        // Se non c'è prezzo specifico per bambini, usa metà del prezzo adulto
                        total += (childPrice > 0 ? childPrice : adultPrice / 2) * children.Value;
                    }
                }
                else
                {
                    // Fallback ai prezzi standard
                    if (boat.AdultPrice.HasValue)
                        total += boat.AdultPrice.Value * adults;
                    
                    if (children.HasValue && boat.ChildPrice.HasValue)
                        total += boat.ChildPrice.Value * children.Value;
                }
            }
            else // Gommone
            {
                // Prezzo giornaliero fisso - cerca prima nei prezzi stagionali
                var dailyPrice = await _boatPriceService.GetPriceForDateAsync(boatId, date, isPerPerson: false);
                
                if (dailyPrice == 0)
                {
                    // Fallback ai prezzi standard
                    var isHighSeason = IsHighSeason(date);
                    dailyPrice = isHighSeason && boat.DailyPriceHigh.HasValue
                        ? boat.DailyPriceHigh.Value
                        : boat.DailyPrice ?? 0;
                }
                
                total += dailyPrice;
            }
        }

        // Aggiungi costo skipper se presente
        if (skipperId.HasValue)
        {
            var skipperPrice = await GetSkipperDailyPriceAsync(skipperId.Value);
            total += skipperPrice * dates.Count;
        }

        return total;
    }

    public async Task<decimal> GetSkipperDailyPriceAsync(int skipperId)
    {
        // Per ora costo fisso skipper: 50€/giorno
        // In futuro si può aggiungere campo DailyPrice al modello Skipper
        return 50m;
    }

    public bool IsHighSeason(DateTime date)
    {
        // Alta stagione: Aprile - Settembre (mesi 4-9)
        var month = date.Month;
        return month >= 4 && month <= 9;
    }
}
