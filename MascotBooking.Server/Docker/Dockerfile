# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY MascotBooking.Server/*.csproj MascotBooking.Server/
COPY MascotBooking.Server/*.sln MascotBooking.Server/

# Restore dependencies
RUN dotnet restore MascotBooking.Server/MascotBooking.Server.csproj

# Copy all source files
COPY MascotBooking.Server/ MascotBooking.Server/

# Build and publish
WORKDIR /src/MascotBooking.Server
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Create data directory for SQLite database
RUN mkdir -p /app/data

# Copy published files from build stage
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080
EXPOSE 8081

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "MascotBooking.Server.dll"]
