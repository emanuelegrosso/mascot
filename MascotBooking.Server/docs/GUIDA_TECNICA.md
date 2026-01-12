# 🔧 Guida Tecnica - Nautica Mascot

Documentazione tecnica completa del sistema di gestione prenotazioni.

## Architettura

### Stack Tecnologico

- **.NET 10**: Framework principale
- **Blazor Server**: UI framework (server-side rendering)
- **Entity Framework Core 10**: ORM per database
- **SQLite**: Database locale
- **Bootstrap 5**: Framework CSS
- **Open Iconic**: Libreria icone

### Pattern Architetturali

- **Repository Pattern**: Servizi come repository
- **Dependency Injection**: Tutti i servizi registrati in Program.cs
- **Service Layer**: Logica business separata dalla UI
- **Component-Based UI**: Razor Components riutilizzabili

## Struttura Progetto

```
MascotBooking.Server/
├── Data/
│   ├── AppDbContext.cs          # Context EF Core
│   └── SeedData.cs              # Dati iniziali
├── Models/
│   ├── Booking.cs               # Prenotazione + BookingDate
│   ├── Customer.cs               # Cliente
│   ├── Boat.cs                   # Natante/Barca
│   └── Enums.cs                  # Enumerazioni
├── Services/
│   ├── IBookingService.cs       # Interfaccia servizio prenotazioni
│   ├── BookingService.cs        # Implementazione
│   ├── ICustomerService.cs      # Interfaccia servizio clienti
│   ├── CustomerService.cs      # Implementazione
│   ├── ICalendarService.cs      # Interfaccia servizio calendario
│   ├── CalendarService.cs      # Implementazione
│   ├── IPriceCalculatorService.cs
│   ├── PriceCalculatorService.cs
│   ├── IGlobalSearchService.cs
│   └── GlobalSearchService.cs
├── Pages/
│   ├── Dashboard.razor         # Dashboard principale
│   ├── BookingForm.razor        # Form nuova prenotazione
│   ├── Calendar.razor           # Calendario 4 viste
│   └── CustomerCard.razor       # Scheda cliente
├── Shared/
│   ├── MainLayout.razor         # Layout principale
│   └── NavMenu.razor            # Menu navigazione
└── wwwroot/
    └── css/
        └── site.css             # Stili custom
```

## Modelli Dati

### Booking (Prenotazione)

```csharp
public class Booking
{
    public int Id { get; set; }
    public BookingType Type { get; set; }           // Escursione/Gommone
    public int CustomerId { get; set; }
    public int BoatId { get; set; }
    public int Adults { get; set; }
    public int? Children { get; set; }
    public decimal StandardPrice { get; set; }
    public decimal ActualPrice { get; set; }
    public BookingStatus Status { get; set; }
    public string? Notes { get; set; }
    public List<BookingDate> BookingDates { get; set; }
}
```

### BookingDate (Data Prenotazione)

```csharp
public class BookingDate
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public bool IsReturned { get; set; }
    public DateTime? ReturnedAt { get; set; }
}
```

### Customer (Cliente)

```csharp
public class Customer
{
    public int Id { get; set; }
    public string Phone { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PlaceOfBirth { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? TaxCode { get; set; }
    public string? DocumentNumber { get; set; }
    public DocumentType? DocumentType { get; set; }
    public bool HasDocument { get; set; }
    public bool HasNauticalLicense { get; set; }
    public string? DocumentFileUrl { get; set; }
    public string? LicenseFileUrl { get; set; }
}
```

### Boat (Natante/Barca)

```csharp
public class Boat
{
    public int Id { get; set; }
    public string Name { get; set; }
    public BoatType Type { get; set; }              // Gommone/Yacht
    public int Horsepower { get; set; }
    public int Capacity { get; set; }
    public decimal? AdultPrice { get; set; }        // Per escursioni
    public decimal? ChildPrice { get; set; }        // Per escursioni
    public decimal? DailyPrice { get; set; }        // Per gommoni
    public decimal? DailyPriceHigh { get; set; }     // Alta stagione
    public bool IsActive { get; set; }
    public bool RequiresLicense => Horsepower >= 100;
}
```

## Database

### SQLite

- **Percorso**: `data/mascot.db`
- **Creazione**: Automatica al primo avvio
- **Migrations**: EF Core Migrations (EnsureCreated)

### Relazioni

- `Booking` → `Customer` (Many-to-One)
- `Booking` → `Boat` (Many-to-One)
- `Booking` → `BookingDate` (One-to-Many)
- `Customer` → `Booking` (One-to-Many)
- `Boat` → `Booking` (One-to-Many)

### Seed Data

Dati iniziali caricati automaticamente:
- 5 clienti di esempio
- 5 natanti/barche
- 5 prenotazioni di esempio
- Dati per tutto dicembre (generati automaticamente)

## Servizi

### IBookingService / BookingService

**Metodi principali:**
- `GetAllBookingsAsync()`: Tutte le prenotazioni
- `GetBookingByIdAsync(int id)`: Prenotazione per ID
- `CreateBookingAsync(Booking booking)`: Crea nuova prenotazione
- `UpdateBookingAsync(Booking booking)`: Aggiorna prenotazione
- `GetBookingsByDateAsync(DateTime date)`: Prenotazioni per data
- `GetBookingsByDateRangeAsync(DateTime start, DateTime end)`: Prenotazioni per range
- `RequiresNauticalLicenseAsync(BookingType type, int boatId)`: Verifica patente richiesta
- `MarkBookingDateAsReturnedAsync(int bookingId, DateTime date)`: Segna come rientrato

### ICustomerService / CustomerService

**Metodi principali:**
- `GetAllCustomersAsync()`: Tutti i clienti
- `GetCustomerByIdAsync(int id)`: Cliente per ID
- `GetCustomerByPhoneAsync(string phone)`: Cliente per telefono
- `CreateCustomerAsync(Customer customer)`: Crea nuovo cliente
- `UpdateCustomerAsync(Customer customer)`: Aggiorna cliente
- `SearchCustomersAsync(string searchTerm)`: Ricerca clienti
- `GetLastBookingAsync(int customerId)`: Ultima prenotazione cliente

### ICalendarService / CalendarService

**Metodi principali:**
- `GetDailyViewAsync(DateTime date)`: Vista giornaliera
- `GetWeeklyViewAsync(DateTime startDate)`: Vista settimanale
- `GetMonthlyViewAsync(DateTime month)`: Vista mensile
- `GetAnnualViewAsync(int year)`: Vista annuale
- `GetBookingsForTimeSlotAsync(DateTime date, TimeSpan? start, TimeSpan? end)`: Prenotazioni per slot

### IPriceCalculatorService / PriceCalculatorService

**Metodi principali:**
- `CalculateStandardPriceAsync(...)`: Calcola prezzo standard
- `IsHighSeason(DateTime date)`: Verifica alta stagione

**Logica calcolo:**
- Escursione: `(adulti × prezzo_adulto + bambini × prezzo_bambino) × n°_date`
- Gommone: `prezzo_giornaliero × n°_date` (considera stagione)
- Alta stagione: Aprile-Settembre (mesi 4-9)

### IGlobalSearchService / GlobalSearchService

**Metodi principali:**
- `SearchAsync(string searchTerm)`: Ricerca unificata

**Cerca in:**
- Clienti: nome, telefono, email, codice fiscale
- Prenotazioni: ID, cliente, natante, data, note
- Barche: nome, ID, CV, capacità

**Risultati:**
- Lista `SearchResult` con tipo, icona, URL navigazione
- Massimo 5 risultati per tipo entità

## Componenti Razor

### Dashboard.razor

**Funzionalità:**
- Riepilogo giornaliero
- Card statistiche cliccabili
- Ricerca globale integrata
- Lista prossimi rientri

**Servizi utilizzati:**
- `IBookingService`
- `ICustomerService`
- `IGlobalSearchService`

### BookingForm.razor

**Funzionalità:**
- Toggle tipo servizio
- Selezione date multiple
- Calcolo prezzi automatico
- Validazione patente
- Form cliente integrato

**Logica:**
- Debounce per calcolo prezzi
- Validazione form completa
- Salvataggio cliente + prenotazione

### Calendar.razor

**Funzionalità:**
- 4 viste (Giornaliera, Settimanale, Mensile, Annuale)
- Codifica cromatica
- Tooltip informativi
- Legenda colori
- Navigazione date

**Parametri:**
- `date`: Data iniziale (query/route)
- `view`: Tipo vista (query/route)
- `bookingId`: ID prenotazione da mostrare (query)

### CustomerCard.razor

**Funzionalità:**
- Anagrafica completa
- Gestione documenti
- Upload file
- Ultima prenotazione

**Parametri:**
- `customerId`: ID cliente da caricare (query/route)

## Stili CSS

### Tema Nautico

**Colori principali:**
```css
--nautica-primary: #1e3a8a
--nautica-secondary: #3b82f6
--nautica-accent: #0ea5e9
--nautica-light: #dbeafe
--nautica-dark: #0c4a6e
```

### Colori Prenotazioni

Definiti in `site.css`:
- `.booking-40cv`: #1E90FF
- `.booking-100cv`: #DC143C
- `.booking-150cv`: #32CD32
- `.booking-other`: #FFA500
- `.booking-escursione`: #9370DB

### Componenti Custom

- `.dashboard-card`: Card con hover effect
- `.search-results-dropdown`: Dropdown risultati ricerca
- `.booking-tooltip`: Tooltip prenotazioni
- `.booking-status-waiting`: Bordo tratteggiato per "In attesa"

## Configurazione

### Program.cs

**Servizi registrati:**
```csharp
builder.Services.AddDbContext<AppDbContext>(...)
builder.Services.AddScoped<IBookingService, BookingService>()
builder.Services.AddScoped<ICustomerService, CustomerService>()
builder.Services.AddScoped<ICalendarService, CalendarService>()
builder.Services.AddScoped<IPriceCalculatorService, PriceCalculatorService>()
builder.Services.AddScoped<IGlobalSearchService, GlobalSearchService>()
```

**Database initialization:**
- `EnsureCreated()` al primo avvio
- `SeedData.Initialize()` se database vuoto

### appsettings.json

Configurazione standard Blazor Server. Nessuna configurazione custom necessaria.

## Deployment

### Sviluppo Locale

```bash
dotnet run
```

### Produzione

**Considerazioni:**
- Database: migrare da SQLite a PostgreSQL/SQL Server
- Autenticazione: implementare Identity o altro sistema
- Upload file: configurare storage (Azure Blob, AWS S3, etc.)
- HTTPS: configurare certificati
- Hosting: Azure App Service, AWS, VPS, etc.

## Estensioni Future

- Autenticazione e autorizzazione
- Export report (PDF, Excel)
- Notifiche email/SMS
- Integrazione pagamenti
- App mobile
- API REST per integrazioni esterne
