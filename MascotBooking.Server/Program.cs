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
    
    // In development, check if required tables exist, if not recreate database
    if (app.Environment.IsDevelopment())
    {
        try
        {
            db.Database.OpenConnection();
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                // Check if Skippers table exists
                command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Skippers'";
                var result = command.ExecuteScalar();
                var skippersTableExists = Convert.ToInt32(result) > 0;
                
                // Check if BoatPrices table exists
                command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='BoatPrices'";
                result = command.ExecuteScalar();
                var boatPricesTableExists = Convert.ToInt32(result) > 0;
                
                // Check if Damages table exists
                command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Damages'";
                result = command.ExecuteScalar();
                var damagesTableExists = Convert.ToInt32(result) > 0;
                
                bool needsRecreation = false;
                
                // Check if SkipperId column exists in Bookings
                if (skippersTableExists)
                {
                    command.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Bookings') WHERE name='SkipperId'";
                    var columnResult = command.ExecuteScalar();
                    var columnExists = Convert.ToInt32(columnResult) > 0;
                    
                    if (!columnExists)
                    {
                        needsRecreation = true;
                    }
                }
                else
                {
                    needsRecreation = true;
                }
                
                // Check if payment columns exist in Bookings
                command.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Bookings') WHERE name='AccontoLasciato'";
                var accontoColumnResult = command.ExecuteScalar();
                var accontoColumnExists = Convert.ToInt32(accontoColumnResult) > 0;
                
                if (!accontoColumnExists)
                {
                    needsRecreation = true;
                }
                
                // If BoatPrices table doesn't exist, recreate database
                if (!boatPricesTableExists)
                {
                    needsRecreation = true;
                }
                
                // If Damages table doesn't exist, recreate database
                if (!damagesTableExists)
                {
                    needsRecreation = true;
                }
                
                db.Database.CloseConnection();
                
                if (needsRecreation)
                {
                    db.Database.EnsureDeleted();
                }
            }
        }
        catch
        {
            // If check fails, just recreate
            try { db.Database.CloseConnection(); } catch { }
            db.Database.EnsureDeleted();
        }
    }
    
    db.Database.EnsureCreated();
    
    // Seed data if database is empty
    if (!db.Customers.Any() && !db.Boats.Any() && !db.Skippers.Any() && !db.BoatPrices.Any())
    {
        SeedData.Initialize(db);
    }
    else
    {
        // Add current week bookings even if database already exists
        SeedData.AddCurrentWeekBookings(db);
    }
}

app.Run();
