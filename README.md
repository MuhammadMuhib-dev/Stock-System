# ClothStock System

Desktop application for managing clothing inventory. Built with C# and Windows Forms on .NET 10, using a layered structure (UI, business logic, data access, models).

## Status

Under active development. Stock management (articles, colors, stock in, adjustments, stock view) is in place. Authentication, orders, expenses and reports are not implemented yet.

## Features

Implemented:

- Article and color management
- Stock in, with article, color and batch selection
- Stock adjustment
- Stock view
- Reusable controls: `ArticleColorPicker`, `BatchPicker`, `MeterSuitDisplay`
- Settings form and session handling

Planned:

- User authentication and roles
- Order management
- Expense tracking
- Reports

## Tech Stack

| Area          | Technology                  |
| ------------- | --------------------------- |
| Language      | C#                          |
| Framework     | .NET 10 (`net10.0-windows`) |
| UI            | Windows Forms               |
| Configuration | `appsettings.json`          |

## Solution Layout

```
Stock-System/
├── ClothStock.Business/     Business logic
├── ClothStock.Data/         Data access
├── ClothStock.Models/       Entities
├── ClothStockSystem/        Windows Forms application
│   ├── Base/                Shared base form
│   ├── Controls/            Reusable controls
│   ├── Froms/               Forms grouped by module
│   │   ├── Auth/
│   │   ├── Expenses/
│   │   ├── Main/
│   │   ├── Orders/
│   │   ├── Reports/
│   │   ├── Settings/
│   │   └── Stock/
│   ├── Helpers/             MessageHelper, SessionManager
│   ├── appsettings.json
│   └── Program.cs
└── ClothStockSystem.slnx
```

## Getting Started

### Requirements

- Windows 10 or later
- .NET 10 SDK
- Visual Studio 2022 or later with the ".NET desktop development" workload

### Setup

```bash
git clone https://github.com/MuhammadMuhib-dev/Stock-System.git
cd Stock-System
dotnet restore
dotnet build
dotnet run --project ClothStockSystem
```

You can also open `ClothStockSystem.slnx` in Visual Studio, set `ClothStockSystem` as the startup project and press F5.

### Configuration

Edit `ClothStockSystem/appsettings.json` before the first run (for example, the database connection string). Do not commit real credentials.

## Development Log

Daily progress is recorded here, newest entry first.

### 2026-11-16

- Added project README.
- Reviewed current structure; Auth, Orders, Expenses and Reports modules are still empty.

<!--
Copy this block for each new day and fill it in:

### YYYY-MM-DD

- What was completed
- What is in progress
- Issues or blockers
-->

## Roadmap

- [x] Layered solution structure
- [x] Article and color management
- [x] Stock in, adjustment and stock view
- [ ] Authentication and roles
- [ ] Order management
- [ ] Expense tracking
- [ ] Reports
- [ ] Export to Excel / PDF

## Author

Muhammad Muhib - [@MuhammadMuhib-dev](https://github.com/MuhammadMuhib-dev)
