# Relazione HelpDesk API

## 1. Avvio

1. Avviare SQL Server e impostare una connection string valida in `HelpDesk.API/appsettings.json`.
2. Dalla radice della soluzione eseguire `dotnet restore` e poi `dotnet run --project HelpDesk.API`.
3. Generare il token User con `cd TokenGenerator && dotnet HelpDesk.TokenGenerator.dll --role User --user user-1 --name "Mario Rossi"`.
4. Generare il token Operator con `dotnet HelpDesk.TokenGenerator.dll --role Operator --user op-1 --name "Operatore"`.
5. Inviare le chiamate a `http://localhost:5016/api/v1/tickets` con `Authorization: Bearer <token>`; la configurazione JWT deve coincidere con quella del generatore.

## 2. Stato dei lavori

### Parte A

- **Fatta** — soluzione .NET con quattro progetti e pacchetto `HelpDesk.Auth` referenziato dall’API.
- **Fatta** — modello `Ticket`, persistenza EF Core SQL Server e migrazione applicata automaticamente all’avvio.
- **Fatta** — creazione del ticket con DTO, validazione FluentValidation, stato iniziale `Open` e identificativo dal claim `sub`.
- **Fatta** — elenco filtrato per utente; gli operatori leggono tutti i ticket.
- **Fatta** — dettaglio con filtro per utente; un ticket altrui non viene trovato e quindi restituisce 404.
- **Fatta** — cambio di stato sequenziale `Open` → `InProgress` → `Resolved` → `Closed`; il primo operatore mantiene l’assegnazione anche se un altro operatore completa il ticket.
- **Parziale** — SLA: le durate sono configurabili per priorità e la scadenza viene calcolata, ma non è stata implementata la Strategy richiesta.
- **Fatta** — export CSV/JSON con Simple Factory, content type coerente e formato sconosciuto convertito in 400.
- **Parziale** — gestione errori e log: il middleware `ProblemDetails` e il logging HTTP strutturato sono attivi; restano da aggiungere log dedicati agli accessi negati gestiti direttamente dall’autenticazione/autorizzazione.
- **Fatta** — API versionata con prefisso `/api/v1/tickets`, inclusi elenco, dettaglio, cambio stato ed export.

### Parte B

- **Non fatta** — test unitari xUnit.
- **Non fatta** — paginazione.
- **Non fatta** — versione `/api/v2/tickets`.
- **Non fatta** — commenti, storico, riapertura, filtri, ticket scaduti ed eventi Observer.

## 3. Struttura

- **HelpDesk.Domain**: entità, enum, interfacce e regole del dominio; non deve contenere EF Core, ASP.NET o dettagli JWT.
- **HelpDesk.Application**: DTO, mapper, validator, servizi applicativi e contratti di persistenza/export; non deve dipendere dal web o gestire direttamente HTTP.
- **HelpDesk.Infrastructure**: DbContext, repository, Unit of Work e migrazioni SQL Server; non deve contenere controller o regole HTTP.
- **HelpDesk.API**: controller, autenticazione, middleware, configurazione e composizione della dependency injection; non deve esporre direttamente le entità né contenere query al database.

## 4. Scelte motivate

1. **Strategy per lo SLA.** È stato introdotto un provider (`IFixHoursProvider`) che legge la durata dalla configurazione invece di inserire uno `switch` nel servizio. È preferibile perché la durata dipende dalla priorità e può cambiare, per esempio introducendo ore lavorative, senza modificare il caso d’uso. Il costo è che la Strategy richiesta dalla traccia non è ancora completa: manca una famiglia di policy sostituibili e oggi il provider rimane un’unica implementazione configurabile.

2. **Simple Factory per l’export.** `TicketExporterFactory` restituisce un `ITicketExporter` CSV o JSON invece di mettere la scelta nel controller. Così un nuovo formato può essere collegato al servizio di export senza cambiare la firma degli endpoint e senza duplicare la serializzazione. Il costo è una classe e un’implementazione aggiuntive per ogni formato; il controller deve comunque associare il formato al relativo content type.

3. **Regole sulle transizioni.** Le transizioni sono state collocate in `TicketService.CheckStatusAvailability`, non nei validator e non nel controller. Il validator controlla solo che il valore sia un enum valido, mentre la validità della sequenza dipende dallo stato già persistito e quindi appartiene al caso d’uso. Questa scelta evita che chiamate diverse applichino regole diverse; il costo è mantenere una tabella di transizioni nel servizio, anche se oggi il flusso è lineare.

4. **Isolamento dei ticket.** L’identità viene letta dal claim `sub`, mai dal body, e il repository applica `CreatedByUserId == userId` per elenco e dettaglio; solo il ruolo Operator usa le query senza filtro. È preferibile rispetto al filtraggio dopo il caricamento perché il database non restituisce dati non autorizzati e il caso di un ticket altrui diventa 404, senza rivelarne l’esistenza. Il costo è dover mantenere due percorsi di query e propagare ruolo e identificativo fino al servizio.

5. **Eccezioni, HTTP e log.** Le eccezioni applicative sono associate a `ProblemDetails` e a codici distinti (400, 404, 409), invece di restituire stack trace o gestire gli errori in ogni controller. Nei log compaiono evento, id ticket, utente, esito e correlation id, ma mai token o dati sensibili; gli errori imprevisti vengono registrati senza esporre il messaggio interno al client. È preferibile per uniformare il contratto dell’API e facilitare la diagnosi; il costo è una mappa centralizzata da mantenere e la necessità di integrare ulteriormente gli eventi 401/403.

6. **DTO separati dall’entità.** I DTO di input non espongono stato, proprietario o assegnatario; questi valori sono determinati dal caso d’uso e dal token. Rispetto al binding diretto dell’entità evita over-posting e impedisce al client di assumere l’identità di un altro utente. Il costo è mantenere mapper e più tipi, ma il contratto HTTP resta indipendente dal modello persistente.

## 5. Limiti e miglioramenti

La soluzione compila, ma presenta avvisi di dipendenze vulnerabili; la connection string di esempio deve inoltre essere sostituita con una configurazione SQL Server reale. Con altro tempo completerei la Strategy SLA e aggiungerei log espliciti per 401/403. Infine aggiungerei test unitari e di integrazione per autorizzazione, transizioni, SLA, factory e gestione degli errori, poi paginazione e le estensioni della parte B.
