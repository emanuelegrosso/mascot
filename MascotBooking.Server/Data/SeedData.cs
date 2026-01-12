using MascotBooking.Server.Models;

namespace MascotBooking.Server.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.Customers.Any() || context.Boats.Any() || context.Skippers.Any())
            return; // Database already seeded

        // Seed Skippers
        var skippers = new List<Skipper>
        {
            new Skipper
            {
                Nome = "Mario",
                Cognome = "Rossi",
                Telefono = "3331112222",
                Email = "mario.rossi@skipper.it",
                NumeroDocumento = "AB1234567",
                TipoDocumento = DocumentType.CartaIdentita,
                NumeroPatenteNautica = "PN001234",
                DataScadenzaPatente = DateTime.Today.AddYears(2),
                Note = "Esperienza 10 anni, parla italiano, inglese, francese",
                Attivo = true,
                CreatedAt = DateTime.UtcNow
            },
            new Skipper
            {
                Nome = "Luca",
                Cognome = "Bianchi",
                Telefono = "3332223333",
                Email = "luca.bianchi@skipper.it",
                NumeroDocumento = "CD2345678",
                TipoDocumento = DocumentType.CartaIdentita,
                NumeroPatenteNautica = "PN002345",
                DataScadenzaPatente = DateTime.Today.AddMonths(6),
                Note = "Specializzato in gommoni potenti, parla italiano, inglese, spagnolo",
                Attivo = true,
                CreatedAt = DateTime.UtcNow
            },
            new Skipper
            {
                Nome = "Anna",
                Cognome = "Verdi",
                Telefono = "3333334444",
                Email = "anna.verdi@skipper.it",
                NumeroDocumento = "EF3456789",
                TipoDocumento = DocumentType.Passaporto,
                NumeroPatenteNautica = "PN003456",
                DataScadenzaPatente = DateTime.Today.AddDays(20), // In scadenza
                Note = "Esperienza 5 anni, parla italiano, inglese, tedesco",
                Attivo = true,
                CreatedAt = DateTime.UtcNow
            },
            new Skipper
            {
                Nome = "Paolo",
                Cognome = "Neri",
                Telefono = "3334445555",
                Email = "paolo.neri@skipper.it",
                NumeroDocumento = "GH4567890",
                TipoDocumento = DocumentType.CartaIdentita,
                NumeroPatenteNautica = "PN004567",
                DataScadenzaPatente = DateTime.Today.AddYears(1),
                Note = "Esperienza 8 anni, parla italiano, inglese",
                Attivo = false, // Disattivo per test
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Skippers.AddRange(skippers);
        context.SaveChanges();

        // Seed Boats (basati sul listino prezzi)
        var boats = new List<Boat>
        {
            new Boat
            {
                Name = "6mt 40CV",
                Type = BoatType.Gommone,
                Horsepower = 40,
                Capacity = 4,
                DailyPrice = 100m, // Prezzo standard sempre 100€
                DailyPriceHigh = 100m, // Prezzo standard sempre 100€
                IsActive = true
            },
            new Boat
            {
                Name = "6mt 100CV",
                Type = BoatType.Gommone,
                Horsepower = 100,
                Capacity = 5,
                DailyPrice = 100m,
                DailyPriceHigh = 100m,
                IsActive = true
            },
            new Boat
            {
                Name = "7mt 150CV",
                Type = BoatType.Gommone,
                Horsepower = 150,
                Capacity = 6,
                DailyPrice = 100m,
                DailyPriceHigh = 100m,
                IsActive = true
            },
            new Boat
            {
                Name = "7/7.5mt 200CV",
                Type = BoatType.Gommone,
                Horsepower = 200,
                Capacity = 7,
                DailyPrice = 100m,
                DailyPriceHigh = 100m,
                IsActive = true
            },
            new Boat
            {
                Name = "8mt 250CV",
                Type = BoatType.Gommone,
                Horsepower = 250,
                Capacity = 8,
                DailyPrice = 100m,
                DailyPriceHigh = 100m,
                IsActive = true
            },
            new Boat
            {
                Name = "10mt 300CV",
                Type = BoatType.Gommone,
                Horsepower = 300,
                Capacity = 10,
                DailyPrice = 100m,
                DailyPriceHigh = 100m,
                IsActive = true
            },
            new Boat
            {
                Name = "Escursione con Conducente e Pranzo",
                Type = BoatType.Yacht,
                Horsepower = 0,
                Capacity = 12,
                AdultPrice = 100m, // Prezzo standard sempre 100€
                ChildPrice = 50m, // Metà prezzo per bambini
                IsActive = true
            }
        };

        context.Boats.AddRange(boats);
        context.SaveChanges();

        // Seed Prezzi Stagionali (dal listino)
        var boatPrices = new List<BoatPrice>();
        
        // 6mt 40CV - Prezzi per mese (Maggio-Novembre)
        var boat40cv = boats.First(b => b.Horsepower == 40);
        boatPrices.AddRange(new[]
        {
            new BoatPrice { BoatId = boat40cv.Id, Month = 5, Price = 100m, IsPerPerson = false },  // Maggio
            new BoatPrice { BoatId = boat40cv.Id, Month = 6, Price = 100m, IsPerPerson = false },  // Giugno
            new BoatPrice { BoatId = boat40cv.Id, Month = 7, Price = 100m, IsPerPerson = false },  // Luglio
            new BoatPrice { BoatId = boat40cv.Id, Month = 8, Price = 100m, IsPerPerson = false },  // Agosto
            new BoatPrice { BoatId = boat40cv.Id, Month = 9, Price = 100m, IsPerPerson = false },  // Settembre
            new BoatPrice { BoatId = boat40cv.Id, Month = 10, Price = 100m, IsPerPerson = false }, // Ottobre
            new BoatPrice { BoatId = boat40cv.Id, Month = 11, Price = 100m, IsPerPerson = false }  // Novembre
        });

        // 6mt 100CV
        var boat100cv = boats.First(b => b.Horsepower == 100);
        boatPrices.AddRange(new[]
        {
            new BoatPrice { BoatId = boat100cv.Id, Month = 5, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat100cv.Id, Month = 6, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat100cv.Id, Month = 7, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat100cv.Id, Month = 8, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat100cv.Id, Month = 9, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat100cv.Id, Month = 10, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat100cv.Id, Month = 11, Price = 100m, IsPerPerson = false }
        });

        // 7mt 150CV
        var boat150cv = boats.First(b => b.Horsepower == 150);
        boatPrices.AddRange(new[]
        {
            new BoatPrice { BoatId = boat150cv.Id, Month = 5, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat150cv.Id, Month = 6, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat150cv.Id, Month = 7, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat150cv.Id, Month = 8, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat150cv.Id, Month = 9, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat150cv.Id, Month = 10, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat150cv.Id, Month = 11, Price = 100m, IsPerPerson = false }
        });

        // 7/7.5mt 200CV
        var boat200cv = boats.First(b => b.Horsepower == 200);
        boatPrices.AddRange(new[]
        {
            new BoatPrice { BoatId = boat200cv.Id, Month = 5, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat200cv.Id, Month = 6, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat200cv.Id, Month = 7, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat200cv.Id, Month = 8, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat200cv.Id, Month = 9, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat200cv.Id, Month = 10, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat200cv.Id, Month = 11, Price = 100m, IsPerPerson = false }
        });

        // 8mt 250CV
        var boat250cv = boats.First(b => b.Horsepower == 250);
        boatPrices.AddRange(new[]
        {
            new BoatPrice { BoatId = boat250cv.Id, Month = 5, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat250cv.Id, Month = 6, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat250cv.Id, Month = 7, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat250cv.Id, Month = 8, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat250cv.Id, Month = 9, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat250cv.Id, Month = 10, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat250cv.Id, Month = 11, Price = 100m, IsPerPerson = false }
        });

        // 10mt 300CV
        var boat300cv = boats.First(b => b.Horsepower == 300 && b.Type == BoatType.Gommone);
        boatPrices.AddRange(new[]
        {
            new BoatPrice { BoatId = boat300cv.Id, Month = 5, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat300cv.Id, Month = 6, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat300cv.Id, Month = 7, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat300cv.Id, Month = 8, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat300cv.Id, Month = 9, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat300cv.Id, Month = 10, Price = 100m, IsPerPerson = false },
            new BoatPrice { BoatId = boat300cv.Id, Month = 11, Price = 100m, IsPerPerson = false }
        });

        // Escursione con Conducente e Pranzo (per persona)
        var escursione = boats.First(b => b.Name.Contains("Escursione"));
        boatPrices.AddRange(new[]
        {
            new BoatPrice { BoatId = escursione.Id, Month = 5, Price = 100m, IsPerPerson = true },
            new BoatPrice { BoatId = escursione.Id, Month = 6, Price = 100m, IsPerPerson = true },
            new BoatPrice { BoatId = escursione.Id, Month = 7, Price = 100m, IsPerPerson = true },
            new BoatPrice { BoatId = escursione.Id, Month = 8, Price = 100m, IsPerPerson = true },
            new BoatPrice { BoatId = escursione.Id, Month = 9, Price = 100m, IsPerPerson = true },
            new BoatPrice { BoatId = escursione.Id, Month = 10, Price = 100m, IsPerPerson = true },
            new BoatPrice { BoatId = escursione.Id, Month = 11, Price = 100m, IsPerPerson = true }
        });

        context.BoatPrices.AddRange(boatPrices);
        context.SaveChanges();

        // Seed Customers
        var customers = new List<Customer>
        {
            new Customer
            {
                Phone = "3331234567",
                Email = "mario@email.it",
                FullName = "Mario Rossi",
                DateOfBirth = new DateTime(1985, 5, 15),
                PlaceOfBirth = "Roma",
                Address = "Via Roma 123",
                PostalCode = "00100",
                City = "Roma",
                Country = "Italia",
                TaxCode = "RSSMRA85E15H501X",
                DocumentNumber = "AB1234567",
                DocumentType = DocumentType.CartaIdentita,
                HasDocument = true,
                HasNauticalLicense = false,
                CreatedAt = DateTime.UtcNow
            },
            new Customer
            {
                Phone = "3478901234",
                Email = "luca@gmail.com",
                FullName = "Luca Bianchi",
                DateOfBirth = new DateTime(1990, 8, 22),
                PlaceOfBirth = "Milano",
                Address = "Corso Garibaldi 45",
                PostalCode = "20121",
                City = "Milano",
                Country = "Italia",
                TaxCode = "BNCLCU90M22F205Y",
                DocumentNumber = "CD2345678",
                DocumentType = DocumentType.CartaIdentita,
                HasDocument = true,
                HasNauticalLicense = true,
                CreatedAt = DateTime.UtcNow
            },
            new Customer
            {
                Phone = "3201122334",
                Email = "anna@verdi.it",
                FullName = "Anna Verdi",
                DateOfBirth = new DateTime(1988, 3, 10),
                PlaceOfBirth = "Napoli",
                Address = "Via Napoli 78",
                PostalCode = "80100",
                City = "Napoli",
                Country = "Italia",
                TaxCode = "VRDNNA88C50F839Z",
                DocumentNumber = "EF3456789",
                DocumentType = DocumentType.Passaporto,
                HasDocument = true,
                HasNauticalLicense = true,
                CreatedAt = DateTime.UtcNow
            },
            new Customer
            {
                Phone = "3398765432",
                Email = "giulia@neri.it",
                FullName = "Giulia Neri",
                DateOfBirth = new DateTime(1992, 11, 5),
                PlaceOfBirth = "Torino",
                Address = "Via Torino 12",
                PostalCode = "10100",
                City = "Torino",
                Country = "Italia",
                TaxCode = "NRIGLI92S45L219W",
                DocumentNumber = "GH4567890",
                DocumentType = DocumentType.CartaIdentita,
                HasDocument = true,
                HasNauticalLicense = true,
                CreatedAt = DateTime.UtcNow
            },
            new Customer
            {
                Phone = "3351234567",
                Email = "paolo@blu.it",
                FullName = "Paolo Blu",
                DateOfBirth = new DateTime(1987, 7, 20),
                PlaceOfBirth = "Palermo",
                Address = "Via Palermo 56",
                PostalCode = "90100",
                City = "Palermo",
                Country = "Italia",
                TaxCode = "BLUPLA87L20G273V",
                DocumentNumber = "IJ5678901",
                DocumentType = DocumentType.PatenteGuida,
                HasDocument = false,
                HasNauticalLicense = false,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Customers.AddRange(customers);
        context.SaveChanges();

        // Seed Sample Bookings
        var today = DateTime.Today;
        var boatL02 = boats.First(b => b.Name == "6mt 40CV");
        var boatAUR = boats.First(b => b.Name == "Escursione con Conducente e Pranzo");
        var boatL05 = boats.First(b => b.Name == "7mt 150CV");
        var boatL10 = boats.First(b => b.Name == "10mt 300CV");

        var bookings = new List<Booking>
        {
            new Booking
            {
                Type = BookingType.Gommone,
                CustomerId = customers[0].Id,
                BoatId = boatL02.Id,
                Adults = 4,
                Children = null,
                StandardPrice = 100m,
                ActualPrice = 100m,
                Status = BookingStatus.Opzionata,
                Notes = "Prenotazione standard",
                CreatedAt = DateTime.UtcNow,
                BookingDates = new List<BookingDate>
                {
                    new BookingDate
                    {
                        Date = today,
                        StartTime = new TimeSpan(10, 0, 0),
                        EndTime = new TimeSpan(14, 0, 0),
                        IsReturned = false
                    }
                }
            },
            new Booking
            {
                Type = BookingType.Escursione,
                CustomerId = customers[1].Id,
                BoatId = boatAUR.Id,
                Adults = 6,
                Children = 0,
                StandardPrice = 100m,
                ActualPrice = 100m,
                Status = BookingStatus.Opzionata,
                Notes = "Escursione condivisa",
                CreatedAt = DateTime.UtcNow,
                BookingDates = new List<BookingDate>
                {
                    new BookingDate
                    {
                        Date = today,
                        StartTime = new TimeSpan(9, 0, 0),
                        EndTime = new TimeSpan(13, 0, 0),
                        IsReturned = false
                    }
                }
            },
            new Booking
            {
                Type = BookingType.Escursione,
                CustomerId = customers[2].Id,
                BoatId = boatAUR.Id,
                Adults = 8,
                Children = 0,
                StandardPrice = 100m,
                ActualPrice = 100m,
                Status = BookingStatus.Opzionata,
                Notes = "Escursione esclusiva personalizzata",
                CreatedAt = DateTime.UtcNow,
                BookingDates = new List<BookingDate>
                {
                    new BookingDate
                    {
                        Date = today,
                        StartTime = new TimeSpan(15, 0, 0),
                        EndTime = new TimeSpan(19, 0, 0),
                        IsReturned = false
                    }
                }
            },
            new Booking
            {
                Type = BookingType.Gommone,
                CustomerId = customers[1].Id,
                BoatId = boatL05.Id,
                Adults = 3,
                Children = null,
                StandardPrice = 100m,
                ActualPrice = 100m,
                Status = BookingStatus.Confermata,
                Notes = "Gommone piccolo",
                CreatedAt = DateTime.UtcNow,
                BookingDates = new List<BookingDate>
                {
                    new BookingDate
                    {
                        Date = today.AddDays(-1),
                        StartTime = new TimeSpan(8, 0, 0),
                        EndTime = new TimeSpan(12, 0, 0),
                        IsReturned = true,
                        ReturnedAt = DateTime.UtcNow.AddHours(-2)
                    }
                }
            },
            new Booking
            {
                Type = BookingType.Gommone,
                CustomerId = customers[3].Id,
                BoatId = boatL10.Id,
                Adults = 7,
                Children = null,
                StandardPrice = 100m,
                ActualPrice = 100m,
                Status = BookingStatus.Opzionata,
                Notes = "Gommone grande",
                CreatedAt = DateTime.UtcNow,
                BookingDates = new List<BookingDate>
                {
                    new BookingDate
                    {
                        Date = today,
                        StartTime = new TimeSpan(14, 0, 0),
                        EndTime = new TimeSpan(18, 0, 0),
                        IsReturned = false
                    }
                }
            }
        };

        context.Bookings.AddRange(bookings);
        context.SaveChanges();

        // Seed December Bookings
        var decemberStart = new DateTime(DateTime.Today.Year, 12, 1);
        var decemberEnd = new DateTime(DateTime.Today.Year, 12, 31);
        var random = new Random();
        var decemberBookings = new List<Booking>();

        for (var date = decemberStart; date <= decemberEnd; date = date.AddDays(1))
        {
            // Skip weekends for some variety
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                // Weekend: more bookings
                var weekendCount = random.Next(3, 6);
                for (int i = 0; i < weekendCount; i++)
                {
                    var customer = customers[random.Next(customers.Count)];
                    var boat = boats[random.Next(boats.Count)];
                    var startHour = random.Next(8, 15);
                    var duration = random.Next(4, 8);
                    
                    decemberBookings.Add(new Booking
                    {
                        Type = boat.Type == BoatType.Gommone ? BookingType.Gommone : BookingType.Escursione,
                        CustomerId = customer.Id,
                        BoatId = boat.Id,
                        Adults = random.Next(2, boat.Capacity),
                        Children = random.Next(0, 3),
                        StandardPrice = boat.Type == BoatType.Gommone 
                            ? (boat.DailyPrice ?? 0) 
                            : (boat.AdultPrice ?? 0) * random.Next(2, 6),
                        ActualPrice = 0, // Will be set below
                        Status = (BookingStatus)random.Next(0, 4),
                        Notes = $"Prenotazione {date:dd/MM/yyyy}",
                        CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                        BookingDates = new List<BookingDate>
                        {
                            new BookingDate
                            {
                                Date = date,
                                StartTime = new TimeSpan(startHour, 0, 0),
                                EndTime = new TimeSpan(startHour + duration, 0, 0),
                                IsReturned = date < DateTime.Today && random.Next(0, 2) == 1
                            }
                        }
                    });
                }
            }
            else
            {
                // Weekday: fewer bookings
                var weekdayCount = random.Next(1, 4);
                for (int i = 0; i < weekdayCount; i++)
                {
                    var customer = customers[random.Next(customers.Count)];
                    var boat = boats[random.Next(boats.Count)];
                    var startHour = random.Next(9, 14);
                    var duration = random.Next(4, 7);
                    
                    decemberBookings.Add(new Booking
                    {
                        Type = boat.Type == BoatType.Gommone ? BookingType.Gommone : BookingType.Escursione,
                        CustomerId = customer.Id,
                        BoatId = boat.Id,
                        Adults = random.Next(2, boat.Capacity),
                        Children = random.Next(0, 2),
                        StandardPrice = boat.Type == BoatType.Gommone 
                            ? (boat.DailyPrice ?? 0) 
                            : (boat.AdultPrice ?? 0) * random.Next(2, 5),
                        ActualPrice = 0, // Will be set below
                        Status = (BookingStatus)random.Next(0, 4),
                        Notes = $"Prenotazione {date:dd/MM/yyyy}",
                        CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                        BookingDates = new List<BookingDate>
                        {
                            new BookingDate
                            {
                                Date = date,
                                StartTime = new TimeSpan(startHour, 0, 0),
                                EndTime = new TimeSpan(startHour + duration, 0, 0),
                                IsReturned = date < DateTime.Today && random.Next(0, 2) == 1
                            }
                        }
                    });
                }
            }
        }

        // Calculate actual prices
        foreach (var booking in decemberBookings)
        {
            booking.ActualPrice = booking.StandardPrice;
        }

        context.Bookings.AddRange(decemberBookings);
        context.SaveChanges();

        // Seed Current Week Bookings
        var currentDate = DateTime.Today;
        var currentWeekStart = currentDate.AddDays(-(int)currentDate.DayOfWeek + (int)DayOfWeek.Monday);
        if (currentWeekStart > currentDate)
        {
            currentWeekStart = currentWeekStart.AddDays(-7); // Se siamo domenica, prendi la settimana precedente
        }
        var currentWeekEnd = currentWeekStart.AddDays(6);
        var weekRandom = new Random(42); // Seed fisso per riproducibilità
        var currentWeekBookings = new List<Booking>();

        for (var date = currentWeekStart; date <= currentWeekEnd; date = date.AddDays(1))
        {
            var isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
            var isPast = date < currentDate;
            var isToday = date == currentDate;
            var isFuture = date > currentDate;

            if (isWeekend)
            {
                // Weekend: 4-7 prenotazioni
                var weekendCount = weekRandom.Next(4, 8);
                for (int i = 0; i < weekendCount; i++)
                {
                    var customer = customers[weekRandom.Next(customers.Count)];
                    var boat = boats[weekRandom.Next(boats.Count)];
                    var startHour = weekRandom.Next(8, 16);
                    var duration = weekRandom.Next(4, 8);
                    var endHour = Math.Min(startHour + duration, 20);
                    
                    // Stati realistici: passato = pagata/rientrata, oggi/futuro = confermata/in attesa
                    BookingStatus status;
                    if (isPast)
                    {
                        status = weekRandom.Next(0, 2) == 0 ? BookingStatus.Pagata : BookingStatus.Confermata;
                    }
                    else if (isToday)
                    {
                        status = weekRandom.Next(0, 3) switch
                        {
                            0 => BookingStatus.Confermata,
                            1 => BookingStatus.InAttesa,
                            _ => BookingStatus.Pagata
                        };
                    }
                    else
                    {
                        status = weekRandom.Next(0, 2) == 0 ? BookingStatus.Confermata : BookingStatus.InAttesa;
                    }

                    var standardPrice = boat.Type == BoatType.Gommone
                        ? (boat.DailyPrice ?? 0)
                        : (boat.AdultPrice ?? 0) * weekRandom.Next(2, 7);

                    currentWeekBookings.Add(new Booking
                    {
                        Type = boat.Type == BoatType.Gommone ? BookingType.Gommone : BookingType.Escursione,
                        CustomerId = customer.Id,
                        BoatId = boat.Id,
                        Adults = weekRandom.Next(2, Math.Min(boat.Capacity, 6)),
                        Children = boat.Type == BoatType.Yacht ? weekRandom.Next(0, 3) : null,
                        StandardPrice = standardPrice,
                        ActualPrice = standardPrice,
                        Status = status,
                        Notes = $"Prenotazione weekend {date:dd/MM/yyyy}",
                        CreatedAt = DateTime.UtcNow.AddDays(-weekRandom.Next(1, 15)),
                        BookingDates = new List<BookingDate>
                        {
                            new BookingDate
                            {
                                Date = date,
                                StartTime = new TimeSpan(startHour, 0, 0),
                                EndTime = new TimeSpan(endHour, 0, 0),
                                IsReturned = isPast && weekRandom.Next(0, 3) > 0, // 66% rientrate se passate
                                ReturnedAt = isPast && weekRandom.Next(0, 3) > 0 
                                    ? DateTime.UtcNow.AddHours(-weekRandom.Next(1, 12))
                                    : null
                            }
                        }
                    });
                }
            }
            else
            {
                // Feriali: 2-5 prenotazioni
                var weekdayCount = weekRandom.Next(2, 6);
                for (int i = 0; i < weekdayCount; i++)
                {
                    var customer = customers[weekRandom.Next(customers.Count)];
                    var boat = boats[weekRandom.Next(boats.Count)];
                    var startHour = weekRandom.Next(9, 15);
                    var duration = weekRandom.Next(4, 7);
                    var endHour = Math.Min(startHour + duration, 19);
                    
                    // Stati realistici
                    BookingStatus status;
                    if (isPast)
                    {
                        status = weekRandom.Next(0, 2) == 0 ? BookingStatus.Pagata : BookingStatus.Confermata;
                    }
                    else if (isToday)
                    {
                        status = weekRandom.Next(0, 3) switch
                        {
                            0 => BookingStatus.Confermata,
                            1 => BookingStatus.InAttesa,
                            _ => BookingStatus.Pagata
                        };
                    }
                    else
                    {
                        status = weekRandom.Next(0, 2) == 0 ? BookingStatus.Confermata : BookingStatus.InAttesa;
                    }

                    var standardPrice = boat.Type == BoatType.Gommone
                        ? (boat.DailyPrice ?? 0)
                        : (boat.AdultPrice ?? 0) * weekRandom.Next(2, 6);

                    currentWeekBookings.Add(new Booking
                    {
                        Type = boat.Type == BoatType.Gommone ? BookingType.Gommone : BookingType.Escursione,
                        CustomerId = customer.Id,
                        BoatId = boat.Id,
                        Adults = weekRandom.Next(2, Math.Min(boat.Capacity, 5)),
                        Children = boat.Type == BoatType.Yacht ? weekRandom.Next(0, 2) : null,
                        StandardPrice = standardPrice,
                        ActualPrice = standardPrice,
                        Status = status,
                        Notes = $"Prenotazione {date:dddd} {date:dd/MM/yyyy}",
                        CreatedAt = DateTime.UtcNow.AddDays(-weekRandom.Next(1, 20)),
                        BookingDates = new List<BookingDate>
                        {
                            new BookingDate
                            {
                                Date = date,
                                StartTime = new TimeSpan(startHour, 0, 0),
                                EndTime = new TimeSpan(endHour, 0, 0),
                                IsReturned = isPast && weekRandom.Next(0, 3) > 0,
                                ReturnedAt = isPast && weekRandom.Next(0, 3) > 0
                                    ? DateTime.UtcNow.AddHours(-weekRandom.Next(1, 10))
                                    : null
                            }
                        }
                    });
                }
            }
        }

        context.Bookings.AddRange(currentWeekBookings);
        context.SaveChanges();
    }
}
