# Specifiche Prototipo – Versione 2  
**Destinatario**: Agente AI per progettazione UI/UX e logica applicativa  
**Data riferimento**: 11 gennaio 2026  

---

## 1. Sezione: Nuova Prenotazione

*(... contenuto precedente invariato ...)*

---

## 2. Scheda Cliente

*(... contenuto precedente invariato ...)*

---

## 3. Calendario delle Prenotazioni

### 3.1 Modalità di visualizzazione
L’utente deve poter alternare tra quattro viste tramite pulsanti/tabs:
- **Giornaliera**: elenco orario delle prenotazioni di un singolo giorno.
- **Settimanale**: griglia 7 giorni × fasce orarie (es. 8:00–20:00).
- **Mensile**: calendario classico con indicatori visivi (es. pallini colorati) per carico giornaliero.
- **Annuale**: vista per mese, con indicatori di saturazione (basso/medio/alto).

### 3.2 Interazioni comuni
- Clic su una data/ora apre il dettaglio delle prenotazioni di quel momento.
- È possibile creare una nuova prenotazione direttamente dal calendario (es. pulsante “+” su una cella o drag & drop).

### 3.3 Codifica cromatica per tipo di servizio
Le prenotazioni devono essere visualizzate con **colori distinti in base al tipo e alla potenza dell’imbarcazione**, secondo la seguente legenda:

| Tipo di servizio | Potenza motore | Colore associato | Nome simbolico |
|------------------|----------------|------------------|----------------|
| Gommone          | 40 CV          | `#1E90FF` (Blu Dodger) | Blu |
| Gommone          | 100 CV         | `#DC143C` (Rosso Cremisi) | Rosso |
| Gommone          | 150 CV         | `#32CD32` (Verde Lime) | Verde |
| Gommone          | Altre potenze  | `#FFA500` (Arancione) | Arancione |
| Escursione       | N/A            | `#9370DB` (Viola Medio) | Viola |

> **Note**:
> - Le escursioni non coinvolgono imbarcazioni noleggiate direttamente dal cliente, quindi usano una categoria separata (“Escursione”) con colore fisso.
> - Se in futuro vengono aggiunte nuove potenze standard (es. 60 CV, 200 CV), estendere la legenda con colori distinti e documentarli.
> - Il colore deve essere applicato a:
>   - Blocchi/eventi nel calendario (giorno/settimana/mese)
>   - Indicatori nella vista annuale
>   - Badge o etichette nel dettaglio prenotazione
> - In tutte le viste, al passaggio del mouse (o tap su mobile), mostrare un tooltip con:  
>   _“[Nome cliente] – [Tipo servizio] – [Potenza CV, se applicabile]”_

### 3.4 Dati visualizzati per prenotazione
Per ogni prenotazione mostrare almeno:
- Nome cliente
- Tipo servizio (es. “Gommone 100 CV” o “Escursione: Grotte Blu”)
- Numero partecipanti (solo per escursioni) o durata (per gommoni, se rilevante)
- Stato (con ulteriore codifica secondaria, es. bordo tratteggiato = in attesa)

---

## 4. Regole di Business Chiave

*(... contenuto precedente invariato, ma aggiornare la tabella se necessario ...)*

| Condizione | Requisito |
|-----------|----------|
| Gommone < 100 CV | Solo documento d’identità richiesto |
| Gommone ≥ 100 CV | Documento + patente nautica obbligatori |
| Escursione | Solo documento d’identità richiesto |
| Prenotazione multi-data | Prezzo = somma dei costi per ogni data |
| Modifica “Prezzo reale” | Non sovrascrivere dopo intervento manuale |
| Visualizzazione calendario | Colore basato su tipo e potenza imbarcazione (vedi legenda §3.3) |

---

## 5. Note per l’Agente AI

- Priorità: chiarezza funzionale > stile grafico.
- Assumere che l’utente sia staff interno (non cliente finale).
- Tutti i campi sensibili (CF, documento, ecc.) devono rispettare normative GDPR (visualizzazione controllata, non esportazione non autorizzata).
- Il prototipo deve supportare flussi in cui un cliente esistente viene cercato e i suoi dati popolati automaticamente nel form di prenotazione.
- La legenda cromatica deve essere accessibile (es. icona “?” o pannello espandibile) in tutte le viste del calendario.