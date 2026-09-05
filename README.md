# BudgetApp

A budget tracker I built for myself. I moved out recently, and I wanted to actually know where my money goes each month instead of guessing — so this app tracks my spending against a monthly allowance, and lets me photograph a receipt and have AI pull the line items off it.

<!-- ![Uploading a receipt and watching the items appear](docs/demo.gif) -->

## Why

Every budgeting app I tried had the same problem: entering transactions is tedious enough that I stop doing it after a week. If logging a grocery run means typing fourteen line items, I'm not going to log the grocery run.

So the core idea is: **take a photo of the receipt, and the app does the typing.** Claude reads the image, extracts the merchant, date, total and every line item, and the app checks the arithmetic before anything touches my ledger. If it isn't confident, it asks me instead of guessing.

Everything else — categories, monthly allowance, weekly and monthly reports — is deliberately boring and deliberately deterministic.

## Features

- **Receipt capture** — photograph a receipt from your phone browser; the image is compressed client-side and uploaded
- **AI line-item extraction** — Claude vision reads the receipt into a structured schema: merchant, date, currency, subtotal, tax, tip, total, and every line item
- **Validation before storage** — `sum(items) + tax + tip` must reconcile with the total, or the receipt goes to a review queue instead of into the ledger
- **Review queue** — anything the model flagged as uncertain or illegible gets a correction screen; my corrections are never overwritten by a later re-parse
- **Duplicate protection** — the same receipt photographed twice becomes one transaction (image hash + fuzzy merchant/date/total match)
- **Monthly allowance tracking** — set a budget per category, see budget-vs-actual as the month goes
- **Weekly & monthly reports** — spending by category, month-over-month deltas, computed in SQL, in *my* timezone
- **Manual entry too** — not everything has a receipt

## How the receipt parsing works

```
photo → compress in browser → upload → job row in SQL Server
      → background worker → Claude vision (structured output schema)
      → raw extraction stored, versioned with the prompt + model ID
      → arithmetic + confidence validation
          ├── passes → transaction created
          └── fails  → review queue → I correct it → transaction created
```

Three rules the code enforces, because a budget app that quietly invents numbers is worse than no budget app:

1. **The model's output is never written straight to the ledger.** It lands in a `ReceiptExtractions` table first — raw JSON, with the prompt version and model ID — and a transaction is only created after validation passes. Re-parsing later with a better prompt is cheap and destroys nothing.
2. **The model never does arithmetic for reports.** Every total, rollup and budget comparison is a SQL query. The LLM's only reporting job is optional commentary written *about* numbers SQL already computed.
3. **Money is stored as integer cents**, never as a floating-point number.

## Stack

| Layer | Choice |
| --- | --- |
| Framework | ASP.NET Core MVC (.NET 10), Razor views |
| Language | C# |
| Client | TypeScript, bundled with npm |
| UI | Bootstrap 5 |
| Database | SQL Server (LocalDB in dev, Azure SQL in prod) + EF Core |
| Background jobs | `BackgroundService` worker over a job table — no Redis |
| Image storage | Local filesystem behind an `IReceiptImageStore` interface |
| AI | Official `Anthropic` NuGet SDK — Claude vision + structured outputs |
| Tests | xUnit, `WebApplicationFactory` against real SQL Server |

## Getting started

```bash
git clone https://github.com/YOUR-USERNAME/BudgetApp.git
cd BudgetApp

dotnet restore
npm install --prefix src/BudgetApp.Web

dotnet user-secrets --project src/BudgetApp.Web \
  set "ConnectionStrings:Default" "Server=(localdb)\MSSQLLocalDB;Database=BudgetApp;Trusted_Connection=True;TrustServerCertificate=True"

dotnet ef database update --project src/BudgetApp.Infrastructure --startup-project src/BudgetApp.Web
dotnet run --project src/BudgetApp.Web
```

Requires the **.NET 10 SDK**, **Node.js LTS**, and **SQL Server** (LocalDB or a Docker container).

The app runs fine without an Anthropic API key — you just won't get receipt parsing. To enable it:

```bash
dotnet user-secrets --project src/BudgetApp.Web set "Anthropic:ApiKey" "sk-ant-..."
```

📖 **Full setup, secrets, migrations and troubleshooting: [Development Setup][wiki-setup]**

## Documentation

Longer-form docs live in the [wiki][wiki-home]:

| Page | What's in it |
| --- | --- |
| [Development Setup][wiki-setup] | Prerequisites, first run, secrets, migrations, troubleshooting |
| [Architecture][wiki-architecture] | Request flow, the job pipeline, why extractions are separate from transactions |
| [Data Model][wiki-data-model] | Tables, why money is `BIGINT` cents, why timestamps are UTC |
| [Receipt Extraction][wiki-extraction] | The prompt, the output schema, validation rules, confidence handling |
| [Evaluation Harness][wiki-eval] | Labeled receipt set, accuracy metrics, cost and latency per receipt |
| [Roadmap][wiki-roadmap] | What's built, what's next |

## Status & roadmap

Personal project, built in stages. Each stage ships before the next one starts.

- [ ] **v0** — Manual transactions, categories, accounts, auth
- [ ] **v1** — Weekly/monthly reports, monthly allowance, budget-vs-actual
- [ ] **v2** — Receipt upload and the background job pipeline
- [ ] **v3** — Claude extraction + review queue ← *the reason this project exists*
- [ ] **v4** — Evaluation harness: hand-labeled receipts, accuracy/cost/latency metrics
- [ ] **v5** — Model and prompt comparison (Opus vs. Sonnet vs. Haiku, image size vs. accuracy)
- [ ] **v6** — Narrative summaries over SQL-computed numbers
- [ ] **Later** — offline capture, bank CSV import, multi-currency

## Privacy

Receipts are personal data — they show where I was, when, and what I bought. So:

- Images are stored on the server's filesystem, never in the database, never in a public bucket
- The API key lives server-side only; the browser never talks to Anthropic
- Receipt images are sent to the Anthropic API for extraction — that's the tradeoff the feature requires, and it's worth stating plainly
- There's a setting to delete the original image once extraction succeeds

## Notes

Built for a single user (me). It would need real multi-tenancy work before anyone else should point it at their own receipts.

<!-- Wiki links — replace YOUR-USERNAME once -->
[wiki-home]: https://github.com/YOUR-USERNAME/BudgetApp/wiki
[wiki-setup]: https://github.com/YOUR-USERNAME/BudgetApp/wiki/Development-Setup
[wiki-architecture]: https://github.com/YOUR-USERNAME/BudgetApp/wiki/Architecture
[wiki-data-model]: https://github.com/YOUR-USERNAME/BudgetApp/wiki/Data-Model
[wiki-extraction]: https://github.com/YOUR-USERNAME/BudgetApp/wiki/Receipt-Extraction
[wiki-eval]: https://github.com/YOUR-USERNAME/BudgetApp/wiki/Evaluation-Harness
[wiki-roadmap]: https://github.com/YOUR-USERNAME/BudgetApp/wiki/Roadmap


## WIKI
To learn more about this project, see the [wiki](https://github.com/MichaelC21/Budget/wiki)
