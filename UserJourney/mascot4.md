# Specifiche Prototipo – Versione 2.1 (Aggiornamento Database e Skipper)  
**Destinatario**: Agente AI per progettazione UI/UX, logica applicativa e modello dati  
**Data riferimento**: 11 gennaio 2026  

---

## 1. Nuova Entità: Tabella `skipper`

### 1.1 Struttura della tabella
La base dati deve includere una tabella dedicata agli skipper, con i seguenti campi:

| Campo | Tipo | Obbligatorio | Descrizione |
|------|------|--------------|-------------|
| `id_skipper` | UUID / intero autoincrementale | Sì | Identificativo univoco |
| `nome` | Testo | Sì | Nome dello skipper |
| `cognome` | Testo | Sì | Cognome dello skipper |
| `telefono` | Testo / telefono | Sì | Recapito principale |
| `email` | Email | No | Indirizzo email (opzionale) |
| `numero_documento` | Testo | Sì | Es. “AB1234567” |
| `tipo_documento` | Enum / testo | Sì | Valori: `Carta d’identità`, `Passaporto`, `Patente di guida` |
| `numero_patente_nautica` | Testo | Sì | Numero ufficiale della patente nautica |
| `data_scadenza_patente` | Data | Sì | Data di scadenza della patente nautica |
| `note` | Testo lungo | No | Informazioni aggiuntive (es. lingue parlate, esperienza) |
| `attivo` | Booleano | Sì | `true` = disponibile per assegnazione |

> Tutti i documenti devono essere tracciati per conformità legale e assicurativa.

### 1.2 Gestione
- Interfaccia dedicata per la gestione degli skipper (CRUD: crea, leggi, aggiorna, elimina).
- Visualizzazione dello stato “attivo/disattivo” per evitare assegnazioni a personale non disponibile.
- Alert automatico se la patente nautica è in scadenza (es. < 30 giorni).

---

## 2. Modifica alla Sezione: Nuova Prenotazione

### 2.1 Campo “Includi skipper”
- Checkbox: **“Noleggio con skipper”**
- Se selezionato:
  - Viene mostrato un **dropdown** con la lista degli **skipper attivi** (nome + cognome).
  - L’utente deve selezionare uno skipper disponibile per la data richiesta.
  - Il sistema registra `id_skipper` nella prenotazione.

### 2.2 Logica condizionale per patente nautica
- **Regola aggiornata**:
  - Se il gommone ha **potenza ≥ 100 CV**:
    - **Senza skipper**: il cliente **deve possedere patente nautica**.
    - **Con skipper**: il cliente **non necessita di patente nautica** (solo documento d’identità richiesto).
  - Questa regola sovrascrive la logica precedente (§1.5 del documento v2).
- Il sistema deve:
  - Mostrare dinamicamente i campi obbligatori in base alla combinazione:  
    `(potenza motore, skipper_incluso)`
  - Validare l’inserimento prima del salvataggio.

### 2.3 Prezzo
- Se viene selezionato uno skipper, il sistema **aggiunge automaticamente** il costo giornaliero dello skipper al “Prezzo standard applicato”.
- Il campo “Prezzo reale” rimane modificabile manualmente (per sconti o accordi).

---

## 3. Aggiornamento Calendario

### 3.1 Visualizzazione skipper
- Nella vista **giornaliera** e nel dettaglio prenotazione, mostrare:
  - Nome e cognome dello skipper assegnato (se presente)
  - Icona ⚓ sempre visibile se skipper incluso
- Nelle viste **mensile/annuale**, nessuna modifica all’aggregazione: lo skipper non altera la tipologia del servizio (resta “Gommone 100 CV”, ecc.).

---

## 4. Aggiornamento Scheda Cliente

- Nessuna modifica diretta ai campi cliente.
- Tuttavia, nella sezione **“Prenotazioni in corso”** e **“Ultima prenotazione”**, se presente uno skipper, deve essere indicato:
  > _“Skipper: Mario Rossi”_

---

## 5. Regole di Business Aggiornate

| Condizione | Requisito |
|-----------|----------|
| Gommone < 100 CV | Solo documento d’identità richiesto (indipendentemente dallo skipper) |
| Gommone ≥ 100 CV **senza skipper** | Documento + patente nautica obbligatori |
| Gommone ≥ 100 CV **con skipper** | Solo documento d’identità richiesto (patente non necessaria per il cliente) |
| Skipper selezionato | Deve essere attivo e con patente nautica valida |
| Assegnazione skipper | Obbligatoria se “Noleggio con skipper” è attivo |

---

## 6. Note per l’Agente AI

- Il database ora comprende due tabelle principali collegate:  
  - `prenotazioni` → contiene `id_skipper` (nullable)  
  - `skipper` → master list di professionisti abilitati
- L’interfaccia di prenotazione deve validare in tempo reale:
  - Disponibilità dello skipper per la data selezionata (se gestito in futuro)
  - Validità della patente nautica dello skipper (almeno non scaduta)
- La presenza di uno skipper **abilita legalmente** il noleggio di imbarcazioni potenti a clienti non patentati.
- Tutti i dati degli skipper sono soggetti a GDPR e devono essere protetti come dati personali sensibili.
- In futuro, si potrà estendere con calendario disponibilità skipper, ma per la v2.1 si assume assegnazione manuale senza controllo di sovrapposizione.