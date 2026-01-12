namespace MascotBooking.Server.Models;

public enum BookingType
{
    Escursione,
    Gommone
}

public enum BookingStatus
{
    Opzionata,
    Confermata,
    Pagata,
    Ritirato,
    Rientrato,
    Riconsegnato, // Sinonimo di Rientrato, aggiunto per compatibilità con nuove specifiche
    Completata,
    Annullata,
    InAttesa // Mantenuto per retrocompatibilità
}

public enum DocumentType
{
    CartaIdentita,
    Passaporto,
    PatenteGuida
}

public enum BoatType
{
    Gommone,
    Yacht
}

public enum CalendarViewType
{
    Giornaliera,
    Settimanale,
    Mensile,
    Annuale
}
