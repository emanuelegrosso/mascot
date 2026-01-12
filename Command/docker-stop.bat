@echo off
REM ============================================
REM MascotBooking - Docker Stop
REM ============================================
echo.
echo Stopping MascotBooking container...
echo.

set CONTAINER_NAME=mascotbooking-server

docker stop %CONTAINER_NAME%
if errorlevel 1 (
    echo Container not running or does not exist.
) else (
    echo Container stopped successfully.
)

echo.
pause
