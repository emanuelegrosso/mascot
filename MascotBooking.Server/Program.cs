using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using MascotBooking.Server.Data;
using MascotBooking.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add HttpClient for API calls (configured for Blazor Server)
builder.Services.AddScoped<HttpClient>(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient
    {
        BaseAddress = new Uri(navigationManager.BaseUri)
    };
});

// Database
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "data", "mascot.db");
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// Services
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();
builder.Services.AddScoped<IGlobalSearchService, GlobalSearchService>();
builder.Services.AddScoped<ISkipperService, SkipperService>();
builder.Services.AddScoped<IBoatService, BoatService>();
builder.Services.AddScoped<IBoatPriceService, BoatPriceService>();
builder.Services.AddScoped<IDamageService, DamageService>();

// Storage Service - Choose one: Local, GoogleDrive, or DropboxStorageService
// Configure in appsettings.json under "Storage" section
var storageProvider = builder.Configuration["Storage:Provider"] ?? "Local";
if (storageProvider.Equals("Local", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<IStorageService, LocalStorageService>();
}
else if (storageProvider.Equals("GoogleDrive", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<IStorageService, GoogleDriveStorageService>();
}
else if (storageProvider.Equals("Dropbox", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<IStorageService, DropboxStorageService>();
}
else
{
    // Fallback: use a mock service that doesn't actually upload (for development)
    builder.Services.AddScoped<IStorageService, MockStorageService>();
}

// Add controllers for API endpoints
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// Serve local storage files if Local storage is configured
if (app.Configuration["Storage:Provider"]?.Equals("Local", StringComparison.OrdinalIgnoreCase) == true)
{
    var storagePath = app.Configuration["Storage:Local:BasePath"];
    if (!string.IsNullOrEmpty(storagePath) && Directory.Exists(storagePath))
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(storagePath),
            RequestPath = "/storage"
        });
    }
}

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    bool needsRecreation = false;
    
    // Check if database exists and validate schema compatibility
    try
    {
        if (db.Database.CanConnect())
        {
            db.Database.OpenConnection();
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                // Validate critical columns that must exist for current schema version
                var criticalColumns = new Dictionary<string, string[]>
                {
                    { "Boats", new[] { "Id", "Name", "Code", "Type", "Horsepower", "Capacity", "IsActive" } },
                    { "Bookings", new[] { "Id", "CustomerId", "BoatId", "SkipperId", "AccontoLasciato", "Status" } },
                    { "Customers", new[] { "Id", "Phone" } },
                    { "Skippers", new[] { "Id", "Nome", "Cognome", "Telefono", "Attivo" } },
                    { "BookingDates", new[] { "Id", "BookingId", "Date" } },
                    { "BoatPrices", new[] { "Id", "BoatId", "Month", "Price" } }
                };
                
                // Check if all critical tables exist
                foreach (var table in criticalColumns.Keys)
                {
                    command.CommandText = $"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{table}'";
                    var tableExists = Convert.ToInt32(command.ExecuteScalar()) > 0;
                    
                    if (!tableExists)
                    {
                        logger.LogWarning("Table '{Table}' does not exist. Database will be recreated.", table);
                        needsRecreation = true;
                        break;
                    }
                    
                    // Check if all critical columns exist in this table
                    foreach (var column in criticalColumns[table])
                    {
                        command.CommandText = $"SELECT COUNT(*) FROM pragma_table_info('{table}') WHERE name='{column}'";
                        var columnExists = Convert.ToInt32(command.ExecuteScalar()) > 0;
                        
                        if (!columnExists)
                        {
                            logger.LogWarning("Column '{Column}' does not exist in table '{Table}'. Database will be recreated.", column, table);
                            needsRecreation = true;
                            break;
                        }
                    }
                    
                    if (needsRecreation) break;
                }
            }
            db.Database.CloseConnection();
        }
    }
    catch (Exception ex)
    {
        // If validation fails (e.g., schema mismatch), recreate database
        logger.LogWarning(ex, "Database schema validation failed. Database will be recreated.");
        try { db.Database.CloseConnection(); } catch { }
        needsRecreation = true;
    }
    
    // Recreate database if schema is incompatible
    if (needsRecreation)
    {
        try
        {
            logger.LogInformation("Recreating database due to schema incompatibility...");
            db.Database.EnsureDeleted();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting database. It may be locked or in use.");
            // Continue anyway - EnsureCreated will handle it
        }
    }
    
    // Ensure database is created with current schema
    try
    {
        db.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error creating database. Application may not work correctly.");
        throw;
    }
    
    // Seed data if database is empty
    if (!db.Customers.Any() && !db.Boats.Any() && !db.Skippers.Any() && !db.BoatPrices.Any())
    {
        logger.LogInformation("Seeding initial data...");
        SeedData.Initialize(db);
    }
    else
    {
        // Add current week bookings even if database already exists
        SeedData.AddCurrentWeekBookings(db);
    }
}

app.Run();
