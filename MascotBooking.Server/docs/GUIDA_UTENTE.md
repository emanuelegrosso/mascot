# 👤 Guida Utente - Nautica Mascot

Manuale d'uso completo per lo staff del noleggio nautico.

## 🏠 Dashboard

La Dashboard è la pagina principale del sistema e fornisce una panoramica rapida della giornata.

### Statistiche Giornaliere

Tre card informative mostrano:
- **Prenotazioni Oggi**: numero totale prenotazioni per oggi
- **Natanti in Giro**: numero natanti attualmente in uso
- **Fatturato Stimato**: somma dei prezzi delle prenotazioni di oggi

**💡 Suggerimento**: Clicca su "Prenotazioni Oggi" o "Natanti in Giro" per aprire il calendario giornaliero.

### Ricerca Globale

Il campo di ricerca in alto permette di cercare rapidamente:
- **Clienti**: per nome, telefono, email
- **Prenotazioni**: per ID, cliente, data
- **Barche**: per nome, CV

**Come usare:**
1. Digita almeno 2 caratteri nel campo ricerca
2. I risultati appaiono automaticamente in un dropdown
3. Clicca su un risultato per aprire la maschera relativa
4. Usa le frecce su/giù per navigare, Enter per selezionare

### Prossimi Rientri

Lista delle prossime 5 prenotazioni attive con:
- Nome cliente e natante
- Orario rientro previsto
- Tipo servizio
- Stato prenotazione

## 📅 Nuova Prenotazione

### Passo 1: Selezione Tipo Servizio

Scegli tra:
- **Noleggio Gommone**: tariffa giornaliera fissa
- **Escursione**: tariffa per persona

### Passo 2: Selezione Date

1. Seleziona una data dal date picker
2. Clicca "Aggiungi Data" per aggiungere altre date
3. Puoi selezionare date multiple (anche non consecutive)
4. Rimuovi una data cliccando la "X" sul badge

**💡 Nota**: Ogni data è trattata come prenotazione separata per il calcolo del prezzo.

### Passo 3: Partecipanti

- **Adulti**: numero obbligatorio (minimo 0)
- **Bambini**: numero opzionale

**Validazione**: Deve esserci almeno un partecipante totale.

### Passo 4: Selezione Natante/Barca

Scegli il natante o la barca dal menu a tendina. Il sistema mostra:
- Nome natante
- Potenza (CV)
- Capacità (posti)

**⚠️ Attenzione**: Se selezioni un gommone ≥100 CV, sarà richiesta la patente nautica.

### Passo 5: Prezzo

- **Prezzo Standard**: calcolato automaticamente (sola lettura)
- **Prezzo Reale**: modificabile per sconti/sovrapprezzi

**💡 Nota**: Dopo aver modificato manualmente il prezzo reale, non verrà più sovrascritto automaticamente.

### Passo 6: Dati Cliente

#### Cliente Esistente
1. Seleziona il cliente dal menu "Cerca Cliente Esistente"
2. I dati vengono popolati automaticamente

#### Nuovo Cliente
Compila i campi:
- **Telefono** *: obbligatorio
- **Email**: opzionale
- **Documento d'identità** *: 
  - Spunta "Presente"
  - Inserisci numero documento
  - Seleziona tipo documento
- **Patente nautica** *: 
  - Obbligatoria solo per gommoni ≥100 CV
  - Spunta "Presente" se disponibile

### Passo 7: Note

Aggiungi eventuali note aggiuntive sulla prenotazione.

### Salvataggio

Clicca "Salva Prenotazione" per confermare. Il sistema:
- Crea/aggiorna il cliente se necessario
- Crea la prenotazione con tutte le date selezionate
- Ti riporta alla Dashboard

## 📆 Calendario

Il calendario offre 4 viste diverse per gestire le prenotazioni.

### Vista Giornaliera

Mostra tutte le prenotazioni di un singolo giorno.

**Navigazione:**
- Usa le frecce ← → per cambiare giorno
- Oppure seleziona una data dal date picker

**Informazioni mostrate:**
- Nome cliente e natante
- Tipo servizio e orario
- Numero persone e prezzo
- Stato prenotazione

**Interazioni:**
- Clic su una prenotazione → dettaglio completo

### Vista Settimanale

Griglia con natanti/barche per riga e giorni della settimana per colonna.

**Informazioni:**
- Badge colorati per ogni prenotazione
- Numero persone nel badge
- Tooltip al passaggio mouse

**Navigazione:**
- Pulsanti "Precedente" / "Successivo" per cambiare settimana

**Colori:**
- Vedi legenda colori (pulsante "Legenda Colori")

### Vista Mensile

Calendario classico con indicatori per ogni giorno.

**Indicatori:**
- Badge colorato con numero prenotazioni
- Colore basato sul tipo/potenza della prima prenotazione

**Interazioni:**
- Clic su un giorno → passa alla vista giornaliera di quel giorno

**Navigazione:**
- Frecce ← → per cambiare mese

### Vista Annuale

Panoramica dell'intero anno con indicatori di saturazione per mese.

**Indicatori:**
- **Basso**: ≤10 prenotazioni
- **Medio**: 11-20 prenotazioni
- **Alto**: >20 prenotazioni

**Navigazione:**
- Frecce ← → per cambiare anno

### Legenda Colori

Clicca "Legenda Colori" per vedere la codifica cromatica:

**Gommoni:**
- 🔵 40 CV - Blu Dodger
- 🔴 100 CV - Rosso Cremisi
- 🟢 150 CV - Verde Lime
- 🟠 Altre potenze - Arancione

**Escursioni:**
- 🟣 Escursione - Viola Medio

### Tooltip

Passa il mouse su una prenotazione per vedere:
- Nome cliente
- Tipo servizio
- Potenza CV (se gommone)

## 👥 Scheda Cliente

### Cerca Cliente

1. Seleziona un cliente dal menu a tendina
2. I dati vengono caricati automaticamente
3. Oppure clicca "Nuovo Cliente" per crearne uno

### Dati Anagrafici

Compila tutti i campi disponibili:
- Nome completo
- Telefono *
- Email
- Data e luogo di nascita
- Indirizzo completo
- Codice fiscale
- Documento d'identità

### Documenti

#### Documento d'Identità
1. Spunta "Presente" se il documento è disponibile
2. Inserisci numero documento
3. Seleziona tipo documento
4. Clicca "Carica" per upload file (PDF, JPG, PNG)
5. Clicca "Visualizza" per vedere documento caricato

#### Patente Nautica
1. Spunta "Presente" se la patente è disponibile
2. Clicca "Carica" per upload file
3. Clicca "Visualizza" per vedere patente caricata

### Ultima Prenotazione

Se il cliente ha prenotazioni precedenti, viene mostrata l'ultima con:
- Data
- Tipo servizio
- Natante/Barca
- Prezzo
- Stato

### Salvataggio

Clicca "Salva Cliente" per salvare le modifiche.

## 🎨 Codifica Colori

### Prenotazioni nel Calendario

I colori aiutano a identificare rapidamente il tipo di servizio:

- **Blu Dodger**: Gommone 40 CV
- **Rosso Cremisi**: Gommone 100 CV
- **Verde Lime**: Gommone 150 CV
- **Arancione**: Gommone altre potenze
- **Viola Medio**: Escursione

### Stati Prenotazione

- **Blu**: Confermata
- **Verde**: Pagata
- **Giallo**: In attesa (bordo tratteggiato)
- **Rosso**: Annullata

## ⌨️ Scorciatoie Tastiera

### Ricerca Globale
- **Enter**: Seleziona risultato evidenziato
- **Escape**: Chiudi ricerca
- **Frecce ↑↓**: Naviga tra risultati

### Calendario
- **Frecce ← →**: Naviga tra date/settimane/mesi/anni
- **Click**: Apri dettaglio prenotazione

## 🔍 Suggerimenti

1. **Ricerca rapida**: Usa sempre la ricerca globale per trovare rapidamente clienti o prenotazioni
2. **Date multiple**: Quando crei una prenotazione per più giorni, aggiungi tutte le date prima di salvare
3. **Prezzo modificabile**: Ricorda che puoi sempre modificare il prezzo reale per accordi personalizzati
4. **Validazione patente**: Il sistema ti avvisa automaticamente se serve la patente nautica
5. **Calendario mensile**: Clicca su un giorno per vedere subito le prenotazioni di quel giorno
6. **Tooltip**: Passa il mouse sulle prenotazioni per informazioni rapide

## ❓ Domande Frequenti

**Q: Come modifico una prenotazione esistente?**
A: Cerca la prenotazione, apri il dettaglio dal calendario, e modifica i campi necessari.

**Q: Come segno un natante come rientrato?**
A: Nel calendario giornaliero, clicca sulla prenotazione e usa il pulsante "Segna come Rientrato".

**Q: Posso creare una prenotazione senza cliente esistente?**
A: Sì, compila i dati cliente direttamente nel form di prenotazione.

**Q: Come vedo tutte le prenotazioni di un cliente?**
A: Apri la scheda cliente e scorri la lista prenotazioni (se implementata) o usa la ricerca.

**Q: I colori nel calendario sono sempre gli stessi?**
A: Sì, i colori sono basati sul tipo e potenza del natante, sempre secondo la legenda.
