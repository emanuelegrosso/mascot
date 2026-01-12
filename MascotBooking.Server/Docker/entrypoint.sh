#!/bin/sh
# Entrypoint script for Render.io - handles PORT environment variable

# Use PORT environment variable if set, otherwise default to 8080
PORT=${PORT:-8080}

# Set ASPNETCORE_URLS to use the PORT
export ASPNETCORE_URLS="http://+:${PORT}"

# Execute the application
exec dotnet MascotBooking.Server.dll
