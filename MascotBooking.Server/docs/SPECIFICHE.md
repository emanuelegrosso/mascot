# 📋 Specifiche Funzionali - Nautica Mascot

Documento completo delle specifiche funzionali implementate nel sistema di gestione prenotazioni.

## 1. Nuova Prenotazione

### 1.1 Selezione del Servizio
- **Toggle** tra due tipi:
  - **Escursione**: tariffa per persona
  - **Noleggio Gommone**: tariffa giornaliera fissa legata alla potenza
- Il tipo selezionato determina i campi obbligatori successivi

### 1.2 Selezione Date Multiple
- Campo "Date" permette selezione di **una o più date** (non necessariamente consecutive)
- Input tramite date picker con aggiunta manuale
- Ogni data selezionata è un'unità di prenotazione indipendente
- Numero partecipanti: **globale** per tutte le date selezionate

### 1.3 Partecipanti
- **Adulti**: campo numerico obbligatorio (≥0)
- **Bambini**: campo numerico opzionale (≥0)
- Validazione: almeno un partecipante totale (adulti + bambini ≥ 1)

### 1.4 Calcolo Prezzi

#### Prezzo Standard (sola lettura)
Calcolato automaticamente:
- **Escursione**: `(adulti × prezzo_adulto + bambini × prezzo_bambino) × n°_date`
- **Gommone**: `prezzo_giornaliero × n°_date`
  - Considera stagione alta/bassa (Aprile-Settembre = alta)

#### Prezzo Reale (modificabile)
- Inizialmente precompilato con prezzo standard
- Modificabile liberamente dallo staff
- **Non viene sovrascritto** dopo modifica manuale

### 1.5 Dati Cliente

#### Campi Obbligatori
- **Telefono**: formato telefono
- **Email**: opzionale, formato email
- **Documento d'identità**: obbligatorio
  - Checkbox "Presente"
  - Campo numero documento
  - Tipo documento (dropdown)

#### Patente Nautica (Condizionale)
- **Obbligatoria** solo se:
  - Servizio = Gommone
  - Potenza ≥ 100 CV
- Altrimenti non richiesta
- Messaggio contestuale dinamico

#### Note
- Campo testo libero opzionale

## 2. Scheda Cliente

### 2.1 Campi Anagrafici

Tutti i campi sono editabili:
- Nome completo
- Telefono *
- Email
- Data di nascita
- Luogo di nascita
- Indirizzo completo (via, CAP, città, nazione)
- Codice fiscale
- Numero documento
- Tipo documento (Carta d'identità / Passaporto / Patente di guida)

### 2.2 Documenti

#### Documento d'Identità
- Checkbox "Presente"
- Upload file (PDF, JPG, PNG)
- Visualizzazione documento caricato

#### Patente Nautica
- Checkbox "Presente"
- Upload file (PDF, JPG, PNG)
- Visualizzazione patente caricata

### 2.3 Ultima Prenotazione

Visualizzazione sintetica (sola lettura):
- Data prenotazione
- Tipo (Escursione / Gommone)
- Nome servizio/natante
- Prezzo totale
- Stato (Confermata / Pagata / Annullata / In attesa)

### 2.4 Comportamento
- Dati riutilizzabili durante creazione prenotazione
- Ricerca cliente → popolamento automatico form
- Tutti i campi copiabili/modificabili

## 3. Calendario delle Prenotazioni

### 3.1 Modalità di Visualizzazione

Quattro viste disponibili:

#### Giornaliera
- Elenco orario prenotazioni di un singolo giorno
- Navigazione data con date picker
- Lista prenotazioni con dettagli

#### Settimanale
- Griglia 7 giorni × natanti/barche
- Badge colorati per prenotazione
- Mostra numero persone nel badge
- Tooltip informativi

#### Mensile
- Calendario classico con indicatori visivi
- Indicatori colorati per carico giornaliero
- Click su giorno → vista giornaliera

#### Annuale
- Vista per mese
- Indicatori saturazione (basso/medio/alto)
- Badge colorati per conteggio

### 3.2 Codifica Cromatica

| Tipo | Potenza | Colore | Codice | Nome |
|------|---------|--------|--------|------|
| Gommone | 40 CV | Blu Dodger | #1E90FF | Blu |
| Gommone | 100 CV | Rosso Cremisi | #DC143C | Rosso |
| Gommone | 150 CV | Verde Lime | #32CD32 | Verde |
| Gommone | Altre | Arancione | #FFA500 | Arancione |
| Escursione | N/A | Viola Medio | #9370DB | Viola |

**Note:**
- Colori applicati a blocchi/eventi in tutte le viste
- Legenda accessibile tramite pulsante "Legenda Colori"
- Tooltip al passaggio mouse: "[Nome cliente] – [Tipo servizio] – [Potenza CV]"

### 3.3 Dati Visualizzati

Per ogni prenotazione:
- Nome cliente
- Tipo servizio (es. "Gommone 100 CV" o "Escursione")
- Numero partecipanti (escursioni) o durata (gommoni)
- Stato con codifica colore:
  - **Confermata**: Blu
  - **Pagata**: Verde
  - **In attesa**: Giallo (bordo tratteggiato)
  - **Annullata**: Rosso

### 3.4 Interazioni

- Click su prenotazione → dettaglio completo
- Click su giorno (vista mensile) → vista giornaliera
- Navigazione tra date/settimane/mesi/anni

## 4. Dashboard

### 4.1 Riepilogo Giornaliero

- **Prenotazioni Oggi**: conteggio totale
- **Natanti in Giro**: conteggio natanti non rientrati
- **Fatturato Stimato**: somma prezzi prenotazioni oggi

### 4.2 Prossimi Rientri

Lista ultime 5 prenotazioni attive:
- Nome cliente
- Natante/Barca
- Orario rientro previsto
- Tipo servizio
- Stato

### 4.3 Ricerca Globale

Campo di ricerca unificato che cerca in:
- **Clienti**: nome, telefono, email, codice fiscale
- **Prenotazioni**: ID, cliente, natante, data, note
- **Barche**: nome, ID, CV, capacità

**Caratteristiche:**
- Ricerca minima 2 caratteri
- Debounce 300ms
- Risultati con icone e badge tipo entità
- Navigazione diretta alla maschera relativa
- Supporto tastiera (Enter, Escape, Frecce)

## 5. Regole di Business

| Condizione | Requisito |
|-----------|----------|
| Gommone < 100 CV | Solo documento d'identità richiesto |
| Gommone ≥ 100 CV | Documento + patente nautica obbligatori |
| Escursione | Solo documento d'identità richiesto |
| Prenotazione multi-data | Prezzo = somma costi per ogni data |
| Modifica "Prezzo reale" | Non sovrascrivere dopo intervento manuale |
| Visualizzazione calendario | Colore basato su tipo e potenza imbarcazione |

## 6. Validazioni

### Prenotazione
- Almeno una data selezionata
- Almeno un partecipante (adulti + bambini ≥ 1)
- Natante/Barca selezionato
- Telefono cliente obbligatorio
- Documento d'identità presente
- Patente nautica se richiesta (≥100 CV)
- Prezzo reale > 0

### Cliente
- Telefono obbligatorio
- Email formato valido (se presente)
- Codice fiscale formato IT (opzionale)

### Natante
- Validazione posti: persone ≤ capacità natante

## 7. Stati Prenotazione

- **Confermata**: prenotazione confermata
- **Pagata**: prenotazione pagata
- **In attesa**: prenotazione in attesa conferma
- **Annullata**: prenotazione annullata

## 8. Note Implementative

- Priorità: chiarezza funzionale > stile grafico
- Utente target: staff interno (non clienti finali)
- GDPR: campi sensibili con visualizzazione controllata
- Supporto flussi: cliente esistente → popolamento automatico form
- Legenda cromatica sempre accessibile in calendario
