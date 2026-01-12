@echo off
REM ============================================
REM MascotBooking - Docker Logs
REM ============================================
echo.
echo Showing logs for MascotBooking container...
echo Press Ctrl+C to exit
echo.

set CONTAINER_NAME=mascotbooking-server

docker logs -f %CONTAINER_NAME%
