# Docker Setup per MascotBooking

Questo documento descrive come utilizzare Docker per eseguire l'applicazione MascotBooking.

## Prerequisiti

- Docker Desktop installato e in esecuzione
- Docker Compose (incluso in Docker Desktop)

## Build e Esecuzione

### Opzione 1: Usando Docker Compose (Consigliato)

```bash
cd Docker
docker-compose up --build
```

L'applicazione sarà disponibile su:
- HTTP: http://localhost:8080

### Opzione 2: Usando Docker direttamente

#### Build dell'immagine

```bash
cd C:\EG\MARTA\Mascot
docker build -f Docker/Dockerfile -t mascotbooking:latest .
```

#### Esecuzione del container

```bash
docker run -d \
  --name mascotbooking-server \
  -p 8080:8080 \
  -v mascot-data:/app/data \
  -e ASPNETCORE_ENVIRONMENT=Production \
  mascotbooking:latest
```

## Volumi

Il database SQLite viene salvato in un volume Docker chiamato `mascot-data` per garantire la persistenza dei dati anche dopo la rimozione del container.

## Comandi Utili

### Visualizzare i log
```bash
docker-compose logs -f
```

### Fermare il container
```bash
docker-compose down
```

### Fermare e rimuovere i volumi
```bash
docker-compose down -v
```

### Eseguire comandi nel container
```bash
docker exec -it mascotbooking-server bash
```

### Rebuild senza cache
```bash
docker-compose build --no-cache
docker-compose up
```

## Note

- Il database SQLite viene creato automaticamente nella directory `/app/data` all'interno del container
- I dati vengono seedati automaticamente al primo avvio se il database è vuoto
- La porta 8080 è esposta per l'accesso HTTP
- Per HTTPS, configurare un reverse proxy (nginx, traefik, ecc.) davanti al container

## Troubleshooting

### Il container non si avvia
Controlla i log con:
```bash
docker-compose logs mascotbooking
```

### Problemi con i permessi del database
Assicurati che il volume abbia i permessi corretti. Se necessario, modifica i permessi:
```bash
docker exec -it mascotbooking-server chmod -R 755 /app/data
```

### Rebuild completo
Se hai problemi persistenti, fai un rebuild completo:
```bash
docker-compose down -v
docker-compose build --no-cache
docker-compose up
```
