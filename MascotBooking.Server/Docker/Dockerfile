# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.csproj ./
COPY *.sln ./

# Restore dependencies
RUN dotnet restore

# Copy all source files
COPY . ./

# Build and publish
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Create data directory for SQLite database
RUN mkdir -p /app/data

# Copy published files from build stage
COPY --from=build /app/publish .

# Copy entrypoint script from build context
COPY Docker/entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

# Expose port (Render.io will map to PORT env var at runtime)
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application using entrypoint script
ENTRYPOINT ["/app/entrypoint.sh"]
