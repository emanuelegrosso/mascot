@echo off
REM ============================================
REM MascotBooking - Docker Update and Run
REM ============================================
echo.
echo ============================================
echo MascotBooking - Docker Update and Run
echo ============================================
echo.

REM Set variables
set PROJECT_DIR=C:\EG\MARTA\Mascot\MascotBooking.Server
set CONTAINER_NAME=mascotbooking-server
set IMAGE_NAME=mascotbooking:latest
set PORT=5000

REM Change to project directory
cd /d "%PROJECT_DIR%"
if errorlevel 1 (
    echo ERROR: Cannot change to directory %PROJECT_DIR%
    pause
    exit /b 1
)

echo [1/5] Stopping existing container (if running)...
docker stop %CONTAINER_NAME% 2>nul
if errorlevel 1 (
    echo Container not running or does not exist - continuing...
)

echo [2/5] Removing existing container...
docker rm %CONTAINER_NAME% 2>nul
if errorlevel 1 (
    echo Container does not exist - continuing...
)

echo [3/5] Building Docker image...
docker build -f Docker/Dockerfile -t %IMAGE_NAME% .
if errorlevel 1 (
    echo ERROR: Docker build failed!
    pause
    exit /b 1
)

echo [4/5] Starting container...
docker run -d -p %PORT%:8080 -v mascot-data:/app/data --name %CONTAINER_NAME% %IMAGE_NAME%
if errorlevel 1 (
    echo ERROR: Failed to start container!
    pause
    exit /b 1
)

echo [5/5] Waiting for container to start...
timeout /t 5 /nobreak >nul

echo.
echo ============================================
echo Container started successfully!
echo ============================================
echo.
echo Container Name: %CONTAINER_NAME%
echo Image: %IMAGE_NAME%
echo Local URL: http://localhost:%PORT%
echo.
echo To view logs: docker logs -f %CONTAINER_NAME%
echo To stop: docker stop %CONTAINER_NAME%
echo To remove: docker rm %CONTAINER_NAME%
echo.

REM Show container status
echo Container Status:
docker ps --filter "name=%CONTAINER_NAME%"
echo.

REM Show last 10 lines of logs
echo Last 10 lines of logs:
docker logs --tail 10 %CONTAINER_NAME%
echo.

pause
