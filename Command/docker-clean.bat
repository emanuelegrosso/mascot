@echo off
REM ============================================
REM MascotBooking - Docker Clean (Stop and Remove)
REM ============================================
echo.
echo Cleaning up MascotBooking Docker resources...
echo.

set CONTAINER_NAME=mascotbooking-server
set IMAGE_NAME=mascotbooking:latest

echo [1/3] Stopping container...
docker stop %CONTAINER_NAME% 2>nul

echo [2/3] Removing container...
docker rm %CONTAINER_NAME% 2>nul
if errorlevel 1 (
    echo Container does not exist.
) else (
    echo Container removed.
)

echo [3/3] Removing image (optional - uncomment to remove)...
REM docker rmi %IMAGE_NAME% 2>nul

echo.
echo Cleanup completed!
echo.
pause
