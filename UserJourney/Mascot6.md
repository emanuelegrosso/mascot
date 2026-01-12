
Conversazione aperta. 1 messaggio da leggere.

Vai ai contenuti
Utilizzo di Gmail con gli screen reader
1 di 2.256
mascot6
Posta in arrivo

Marta Agostinetto
Allegati
19:14 (0 minuti fa)
a me

Sembra che questo messaggio sia in inglese

 Un allegato
  •  Scansionato da Gmail
Manual
# 📋 Specifiche Tecniche – Aggiornamenti Sistema Prenotazioni

**Data**: 13 gennaio 2026  
**Versione**: 1.0  
**Autore**: [Inserire nome]  

---

## 🔧 1. Modifiche alla Sezione “Prenotazione”

### 1.1 Nuove colonne da aggiungere alla tabella delle prenotazioni

| Nome Colonna          | Tipo Dato        | Descrizione                                                                 | Obbligatorio | Valori Possibili / Formato |
|-----------------------|------------------|-----------------------------------------------------------------------------|--------------|----------------------------|
| `Acconto Lasciato`    | Booleano         | Indica se è stato versato un acconto per la prenotazione                    | No           | ✅ Sì / ❌ No               |
| `Saldo da Pagare`     | Numero (decimale)| Importo residuo da pagare per completare il pagamento della prenotazione    | No           | Es. `150.00`               |
| `Saldo Completato`    | Booleano         | Indica se il saldo è stato interamente pagato                               | No           | ✅ Sì / ❌ No               |

> **Note implementative**:
> - Il campo `Saldo da Pagare` deve essere calcolabile automaticamente come:  
>   `Totale Prenotazione - Acconto Versato`  
>   (se presente un campo "Totale Prenotazione" e "Acconto Versato").
> - In alternativa, se non si gestisce il totale, il campo può essere inserito manualmente.
> - I campi booleani devono essere visualizzati con icone chiare (es. ✅/❌) o switch nell’interfaccia utente.

---

## 📊 2. Modifiche al Dashboard Giornaliero

### 2.1 Nuovo campo: “Danni e Costi Associati”

| Nome Campo             | Tipo Dato        | Descrizione                                                                 | Obbligatorio | Esempio                     |
|------------------------|------------------|-----------------------------------------------------------------------------|--------------|-----------------------------|
| `Descrizione Danni`    | Testo (lungo)    | Breve descrizione dei danni riscontrati nella giornata                      | No           | “Graffio sul tavolo n.5”    |
| `Costo Danni`          | Numero (decimale)| Importo corrisposto per il risarcimento o la riparazione dei danni          | No           | `45.50`                     |

> **Note implementative**:
> - Il campo deve essere editabile direttamente dal dashboard giornaliero.
> - Deve essere visibile in una sezione dedicata (es. “Incidenti / Danni”).
> - È consigliabile aggiungere un pulsante “+ Aggiungi danno” per gestire più eventi nella stessa giornata (se previsto il supporto a più danni/giorno).
> - Se non vengono inseriti danni, i campi rimangono vuoti o con valore `0`.

---

## 💡 Considerazioni Generali

- Tutti i nuovi campi devono essere **salvati nel database** e **visualizzabili nei report esportabili** (CSV/PDF).
- L’interfaccia utente deve garantire **chiarezza visiva** e **facilità di compilazione**.
- I campi relativi ai pagamenti (`Acconto Lasciato`, `Saldo da Pagare`, `Saldo Completato`) devono poter essere **filtrati e ordinati** nella vista elenco prenotazioni.
- Eventuali modifiche ai dati devono essere **tracciabili** (opzionale: log modifiche).

---

## ✅ Checklist Implementazione

- [ ] Aggiungere le 3 nuove colonne alla tabella “Prenotazioni”
- [ ] Aggiornare form di modifica/inserimento prenotazione
- [ ] Aggiornare logica di calcolo del saldo (se automatica)
- [ ] Aggiungere sezione “Danni” al dashboard giornaliero
- [ ] Aggiornare schema del database (aggiunta colonne/tabella)
- [ ] Testare salvataggio, visualizzazione ed esportazione dati
- [ ] Documentare le novità per l’utente finale

---

> **Firma responsabile sviluppo**: _________________________  
> **Data approvazione**: _________________________
aggiornamentosezprenotazioni6.md
Visualizzazione di aggiornamentosezprenotazioni6.md.