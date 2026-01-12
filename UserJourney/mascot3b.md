# Specifiche Prototipo – Versione 2 (Completa)  
**Destinatario**: Agente AI per progettazione UI/UX e logica applicativa  
**Data riferimento**: 11 gennaio 2026  

---

## 1. Sezione: Nuova Prenotazione

### 1.1 Selezione del servizio
- L’utente deve poter scegliere tra:
  - **Escursione** (con tariffa per persona)
  - **Noleggio gommone** (con tariffa giornaliera fissa, legata alla potenza)
- Il tipo di servizio selezionato determina i campi obbligatori successivi.

### 1.2 Selezione date multipla
- Il campo “Date” deve permettere la selezione di **una o più date** (non necessariamente consecutive).
- Modalità di input: calendario integrato con selezione multipla (click singoli o range).
- Ogni data selezionata è trattata come un’unità di prenotazione indipendente ai fini del calcolo del prezzo.
- Il numero di partecipanti è **globale** (unico per tutte le date).

### 1.3 Partecipanti
- Due campi numerici:
  - `Adulti` (obbligatorio, ≥0)
  - `Bambini` (facoltativo, ≥0)
- Validazione: almeno un partecipante totale (adulti + bambini ≥ 1).

### 1.4 Calcolo prezzi
- Ogni escursione/gommone ha un prezzo standard associato:
  - Escursione: prezzo per adulto e per bambino.
  - Gommone: prezzo giornaliero fisso (indipendente dal numero di persone).
- **Prezzo standard applicato**:
  - Campo in sola lettura.
  - Calcolato come:  
    `Totale = Σ [per ogni data (prezzo unitario × giorni)]`  
    (per escursioni: `(adulti × prezzo_adulto + bambini × prezzo_bambino) × n°_date`)  
    (per gommoni: `prezzo_giornaliero × n°_date`)
- **Prezzo reale**:
  - Campo numerico modificabile.
  - Inizialmente precompilato con il valore del “Prezzo standard applicato”.
  - Può essere modificato liberamente dallo staff (sconti, sovrapprezzi, accordi personalizzati).
  - Non viene sovrascritto automaticamente dopo la prima modifica manuale.

### 1.5 Dati cliente (nella prenotazione)
I seguenti campi devono essere presenti nel form:
- `Telefono` (obbligatorio, formato telefono)
- `Email` (facoltativo, formato email)
- `Documento d’identità` (obbligatorio):
  - Checkbox “Presente” + campo testo per numero/tipo oppure upload file.
- `Patente nautica` (condizionale):
  - Obbligatoria **solo** se il gommone selezionato ha potenza ≥100 CV.
  - Altrimenti, non richiesta (solo documento d’identità).
- `Note` (testo libero, facoltativo)

#### Logica condizionale patente
- Se servizio = **Gommone**:
  - Se potenza < 100 CV → richiesto solo documento d’identità.
  - Se potenza ≥ 100 CV → richiesti **documento + patente nautica**.
- Se servizio = **Escursione** → richiesto solo documento d’identità.
- Mostrare messaggio contestuale dinamico in base alla selezione.

### 1.6 Skipper
- Checkbox: **“Includi skipper”**
- Se selezionato, deve influenzare:
  - Il prezzo (se previsto)
  - La visualizzazione nel calendario (vedi §3.4)

---

## 2. Scheda Cliente

### 2.1 Campi anagrafici aggiuntivi (tutti editabili)
- Data di nascita (date picker)
- Luogo di nascita (testo libero)
- Domicilio (indirizzo completo: via, civico, CAP, città, nazione)
- Codice fiscale (testo, validazione opzionale per formato IT)
- Numero documento (es. “AB1234567”)
- Tipo documento (dropdown: Carta d’identità / Passaporto / Patente di guida)

### 2.2 Ultima prenotazione (sola lettura)
Visualizzare in modo sintetico:
- Data
- Tipo (Escursione / Gommone)
- Nome servizio selezionato
- Prezzo totale
- Stato (Confermata / Pagata / Annullata / In attesa)

### 2.3 Prenotazioni in corso
- Mostrare una sezione distinta chiamata **“Prenotazioni in corso”**.
- Contenuto: elenco di tutte le prenotazioni future non ancora concluse (stato ≠ “Completata”, “Annullata”).
- Per ogni prenotazione mostrare:
  - Data(e)
  - Tipo servizio (es. “Escursione: Grotte Blu” o “Gommone 100 CV”)
  - Numero partecipanti (per escursioni) o durata (per gommoni)
  - Stato attuale
  - Eventuale indicazione “Skipper incluso”
- Ogni riga deve essere **cliccabile** e collegata direttamente alla relativa visualizzazione nel **calendario** (apre la vista giornaliera della data corrispondente con la prenotazione evidenziata).

### 2.4 Comportamento generale
- I dati della scheda cliente devono essere **riutilizzabili** durante la creazione di una nuova prenotazione (es. ricerca cliente → popolazione automatica dei campi).
- Tutti i campi devono essere **copiabili/modificabili** direttamente dall’interfaccia.

---

## 3. Calendario delle Prenotazioni

### 3.1 Modalità di visualizzazione
L’utente deve poter alternare tra quattro viste tramite pulsanti/tabs:
- **Giornaliera**: elenco orario delle prenotazioni di un singolo giorno.
- **Settimanale**: griglia 7 giorni × fasce orarie (es. 8:00–20:00).
- **Mensile**: calendario classico con indicatori visivi per carico giornaliero.
- **Annuale**: vista per mese, con indicatori di saturazione (basso/medio/alto).

### 3.2 Codifica cromatica per tipo di servizio
Le prenotazioni devono essere visualizzate con **colori distinti in base al tipo e alla potenza dell’imbarcazione**, secondo la seguente legenda:

| Tipo di servizio | Potenza motore | Colore esadecimale | Nome simbolico |
|------------------|----------------|--------------------|----------------|
| Escursione       | N/A            | `#9370DB`          | Viola          |
| Gommone          | 40 CV          | `#1E90FF`          | Blu            |
| Gommone          | 100 CV         | `#DC143C`          | Rosso          |
| Gommone          | 150 CV         | `#32CD32`          | Verde          |
| Gommone          | 200 CV         | `#FF8C00`          | Arancione scuro|
| Gommone          | 250 CV         | `#8B4513`          | Marrone        |
| Gommone          | 400 CV         | `#2F2F2F`          | Grigio scuro   |
| Altro / Non definito | —         | `#A9A9A9`          | Grigio neutro  |

> **Note**:
> - Il colore deve essere applicato a blocchi/eventi in tutte le viste.
> - Al passaggio del mouse (o tap su mobile), mostrare tooltip con:  
>   _“[Nome cliente] – [Tipo servizio] – [Potenza CV, se applicabile]”_

### 3.3 Indicatori di carico – Viste Mensile e Annuale
In **vista mensile** e **annuale**, ogni giorno deve mostrare un aggregato visivo suddiviso per tipologia, es.:
