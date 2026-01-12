# 🔌 API e Servizi - Nautica Mascot

Documentazione completa delle interfacce e servizi del sistema.

## IBookingService

Servizio per la gestione delle prenotazioni.

### Metodi

#### GetAllBookingsAsync()
```csharp
Task<List<Booking>> GetAllBookingsAsync()
```
Restituisce tutte le prenotazioni ordinate per data di creazione (più recenti prima).

**Include:**
- Customer
- Boat
- BookingDates

#### GetBookingByIdAsync(int id)
```csharp
Task<Booking?> GetBookingByIdAsync(int id)
```
Restituisce una prenotazione per ID.

**Include:**
- Customer
- Boat
- BookingDates

#### CreateBookingAsync(Booking booking)
```csharp
Task<Booking> CreateBookingAsync(Booking booking)
```
Crea una nuova prenotazione.

**Comportamento:**
- Imposta `CreatedAt` automaticamente
- Salva anche le `BookingDates` associate

#### UpdateBookingAsync(Booking booking)
```csharp
Task<Booking> UpdateBookingAsync(Booking booking)
```
Aggiorna una prenotazione esistente.

**Comportamento:**
- Imposta `UpdatedAt` automaticamente

#### DeleteBookingAsync(int id)
```csharp
Task DeleteBookingAsync(int id)
```
Elimina una prenotazione (cascade su BookingDates).

#### GetBookingsByDateAsync(DateTime date)
```csharp
Task<List<Booking>> GetBookingsByDateAsync(DateTime date)
```
Restituisce tutte le prenotazioni per una data specifica.

#### GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
```csharp
Task<List<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
```
Restituisce tutte le prenotazioni in un range di date.

#### RequiresNauticalLicenseAsync(BookingType type, int boatId)
```csharp
Task<bool> RequiresNauticalLicenseAsync(BookingType type, int boatId)
```
Verifica se è richiesta la patente nautica.

**Logica:**
- Escursione → `false`
- Gommone < 100 CV → `false`
- Gommone ≥ 100 CV → `true`

#### MarkBookingDateAsReturnedAsync(int bookingId, DateTime date)
```csharp
Task MarkBookingDateAsReturnedAsync(int bookingId, DateTime date)
```
Segna una data prenotazione come rientrata.

**Comportamento:**
- Imposta `IsReturned = true`
- Imposta `ReturnedAt = DateTime.UtcNow`

---

## ICustomerService

Servizio per la gestione dei clienti.

### Metodi

#### GetAllCustomersAsync()
```csharp
Task<List<Customer>> GetAllCustomersAsync()
```
Restituisce tutti i clienti ordinati per nome.

#### GetCustomerByIdAsync(int id)
```csharp
Task<Customer?> GetCustomerByIdAsync(int id)
```
Restituisce un cliente per ID.

**Include:**
- Bookings (con Boat e BookingDates)

#### GetCustomerByPhoneAsync(string phone)
```csharp
Task<Customer?> GetCustomerByPhoneAsync(string phone)
```
Restituisce un cliente per numero di telefono.

#### CreateCustomerAsync(Customer customer)
```csharp
Task<Customer> CreateCustomerAsync(Customer customer)
```
Crea un nuovo cliente.

**Comportamento:**
- Imposta `CreatedAt` automaticamente

#### UpdateCustomerAsync(Customer customer)
```csharp
Task<Customer> UpdateCustomerAsync(Customer customer)
```
Aggiorna un cliente esistente.

**Comportamento:**
- Imposta `UpdatedAt` automaticamente

#### DeleteCustomerAsync(int id)
```csharp
Task DeleteCustomerAsync(int id)
```
Elimina un cliente (restrict su Bookings).

#### SearchCustomersAsync(string searchTerm)
```csharp
Task<List<Customer>> SearchCustomersAsync(string searchTerm)
```
Ricerca clienti per:
- Nome completo
- Telefono
- Email
- Codice fiscale

**Case-insensitive, partial match.**

#### GetLastBookingAsync(int customerId)
```csharp
Task<Booking?> GetLastBookingAsync(int customerId)
```
Restituisce l'ultima prenotazione di un cliente.

**Include:**
- Boat
- BookingDates

---

## ICalendarService

Servizio per le viste calendario.

### Metodi

#### GetDailyViewAsync(DateTime date)
```csharp
Task<Dictionary<string, List<Booking>>> GetDailyViewAsync(DateTime date)
```
Restituisce prenotazioni raggruppate per natante/barca per una data.

**Chiave**: `"{Nome Natante} ({CV} CV)"`

#### GetWeeklyViewAsync(DateTime startDate)
```csharp
Task<Dictionary<string, List<Booking>>> GetWeeklyViewAsync(DateTime startDate)
```
Restituisce prenotazioni raggruppate per natante/barca per una settimana (7 giorni da startDate).

#### GetMonthlyViewAsync(DateTime month)
```csharp
Task<Dictionary<DateTime, int>> GetMonthlyViewAsync(DateTime month)
```
Restituisce conteggio prenotazioni per ogni giorno del mese.

**Chiave**: `DateTime` (giorno)
**Valore**: `int` (numero prenotazioni)

#### GetAnnualViewAsync(int year)
```csharp
Task<Dictionary<int, int>> GetAnnualViewAsync(int year)
```
Restituisce conteggio prenotazioni per ogni mese dell'anno.

**Chiave**: `int` (mese 1-12)
**Valore**: `int` (numero prenotazioni)

#### GetBookingsForTimeSlotAsync(DateTime date, TimeSpan? startTime, TimeSpan? endTime)
```csharp
Task<List<Booking>> GetBookingsForTimeSlotAsync(DateTime date, TimeSpan? startTime, TimeSpan? endTime)
```
Restituisce prenotazioni per uno slot temporale specifico.

---

## IPriceCalculatorService

Servizio per il calcolo dei prezzi.

### Metodi

#### CalculateStandardPriceAsync(...)
```csharp
Task<decimal> CalculateStandardPriceAsync(
    BookingType type,
    int boatId,
    List<DateTime> dates,
    int adults,
    int? children = null)
```
Calcola il prezzo standard per una prenotazione.

**Logica Escursione:**
```
prezzo = Σ [per ogni data: (adulti × prezzo_adulto + bambini × prezzo_bambino)]
```

**Logica Gommone:**
```
prezzo = Σ [per ogni data: prezzo_giornaliero]
```
Considera stagione alta/bassa:
- Alta stagione (Aprile-Settembre): `DailyPriceHigh` se disponibile
- Bassa stagione: `DailyPrice`

#### IsHighSeason(DateTime date)
```csharp
bool IsHighSeason(DateTime date)
```
Verifica se una data è in alta stagione.

**Logica:**
- Alta stagione: mesi 4-9 (Aprile-Settembre)
- Bassa stagione: mesi 1-3, 10-12

---

## IGlobalSearchService

Servizio per la ricerca globale unificata.

### Metodi

#### SearchAsync(string searchTerm)
```csharp
Task<List<SearchResult>> SearchAsync(string searchTerm)
```
Ricerca unificata in tutte le entità.

**Parametri:**
- `searchTerm`: termine di ricerca (minimo 2 caratteri)

**Cerca in:**

**Clienti:**
- Nome completo
- Telefono
- Email
- Codice fiscale

**Prenotazioni:**
- ID prenotazione
- Nome cliente
- Telefono cliente
- Nome natante
- Note

**Barche:**
- Nome
- ID
- CV (potenza)
- Capacità

**Risultati:**
- Massimo 5 risultati per tipo entità
- Ordinati per tipo, poi per titolo

**Restituisce:**
```csharp
public class SearchResult
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public SearchResultType Type { get; set; }
    public string Icon { get; set; }
    public string NavigationUrl { get; set; }
}
```

**URL Navigazione:**
- Cliente: `/customer?customerId={id}`
- Prenotazione: `/calendar?bookingId={id}`
- Barca: `/booking?boatId={id}`

---

## Enumerazioni

### BookingType
```csharp
public enum BookingType
{
    Escursione,
    Gommone
}
```

### BookingStatus
```csharp
public enum BookingStatus
{
    Confermata,
    Pagata,
    Annullata,
    InAttesa
}
```

### DocumentType
```csharp
public enum DocumentType
{
    CartaIdentita,
    Passaporto,
    PatenteGuida
}
```

### BoatType
```csharp
public enum BoatType
{
    Gommone,
    Yacht
}
```

### CalendarViewType
```csharp
public enum CalendarViewType
{
    Giornaliera,
    Settimanale,
    Mensile,
    Annuale
}
```

### SearchResultType
```csharp
public enum SearchResultType
{
    Customer,
    Booking,
    Boat
}
```

---

## Esempi d'Uso

### Creare una Prenotazione

```csharp
var booking = new Booking
{
    Type = BookingType.Gommone,
    CustomerId = customerId,
    BoatId = boatId,
    Adults = 4,
    StandardPrice = 120m,
    ActualPrice = 120m,
    Status = BookingStatus.Confermata,
    BookingDates = new List<BookingDate>
    {
        new BookingDate
        {
            Date = DateTime.Today,
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(14, 0, 0)
        }
    }
};

await bookingService.CreateBookingAsync(booking);
```

### Calcolare Prezzo

```csharp
var dates = new List<DateTime> { DateTime.Today, DateTime.Today.AddDays(1) };
var price = await priceCalculator.CalculateStandardPriceAsync(
    BookingType.Gommone,
    boatId,
    dates,
    adults: 4,
    children: null
);
```

### Ricerca Globale

```csharp
var results = await globalSearchService.SearchAsync("Mario");
foreach (var result in results)
{
    Console.WriteLine($"{result.Type}: {result.Title}");
    // Navigate to: result.NavigationUrl
}
```

---

## Note Implementative

- Tutti i servizi sono **scoped** (una istanza per richiesta)
- Tutti i metodi sono **async** per performance
- Include automatici per evitare N+1 queries
- Validazioni business logic nei servizi
- Gestione errori: ritorna `null` se entità non trovata
