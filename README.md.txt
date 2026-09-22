#  Trading Journal

En skrivbordsapplikation för traders som vill logga, analysera och utvärdera 
sina trades, byggd som ett alternativ till manuell Excel hantering.

![Trading Journal Screenshot]("\Trading journal\Screenshots\Journal.png")
![Posistion size Screenshot]("\Trading journal\Screenshots\PositionCalculatorng")
![Performance Screenshot]("\Trading journal\Screenshots\Performance.png")

---

##  Bakgrund

Projektet bygger på ett verkligt behov: att ersätta en manuell Excel-modell 
med en strukturerad applikation som automatiserar beräkningar, minskar 
manuella fel och ger en tydlig överblick av trading-prestanda.

---

##  Funktioner

- Journal: Logga trades med alla relevanta fält (asset, entry, SL, TP, exit, amount)
- Position Size Calculator: Automatisk beräkning av position size baserat på 
  risk%, kapital och hävstång
- Performance Dashboard: Equity curve, expectancy, win rate, max drawdown 
  och andra nyckeltal
- SQL Server: All data sparas lokalt i en SQL Server-databas
- Färgkodning: Long/Short och PnL visas med gröna/röda färger för snabb överblick

---

##  Tech stack (Teknologi: Användning) 

- C# / .NET 8: Huvudspråk 
- WPF :Skrivbordsgränssnitt 
- MVVM : Arkitekturmönster 
- Entity Framework Core 8 : ORM och databashantering 
- SQL Server Express : Lokal databas 
- OxyPlot : Equity curve-graf 
- xUnit : Enhetstester 
- CommunityToolkit.Mvvm : MVVM-hjälpbibliotek 

---

##  Arkitektur

Projektet är uppdelat i tre lager enligt MVVM-principen:
![Arkitektur Screenshot]("\Trading journal\Screenshots\Arkitektur.png")

TradingJournal/
├── TradingJournal.Core/ # Domänlogik, modeller, services, repository
│ ├── Models/ # Trade, LongTrade, ShortTrade, Account
│ ├── Services/ # PositionSizeCalculator, PerformanceCalculator
│ ├── Interfaces/ # ITradeRepository
│ └── Data/ # AppDbContext, TradeRepository
├── TradingJournal.Wpf/ # Presentationslager
│ ├── ViewModels/ # JournalVM, PositionCalculatorVM, PerformanceVM
│ ├── Views/ # XAML-vyer
│ └── Helpers/ # Converters, RelayCommand
└── TradingJournal.Tests/ # Enhetstester
├── PositionSizeCalculatorTests.cs
└── PerformanceCalculatorTests.cs

---

### OOP-design
- 'Trade' är en abstrakt basklass
- 'LongTrade' och 'ShortTrade' ärver och override:ar 'CalculatePnL()'
- 'ITradeRepository' möjliggör testbarhet via dependency injection

---

##  Formler

- Position Size Calculator:
TotalRisk = Capital × RiskPercent / 100
Amount = TotalRisk / |Entry − StopLoss|
NominalPositionSize = Amount × Entry
FinalPositionSize = NominalPositionSize / Leverage
RR = |TakeProfit − Entry| / |Entry − StopLoss|

- Performance:
Expectancy = (WinRate × AvgWin) − (LossRate × AvgLoss)
MaxDrawdown = (Peak − Balance) / Peak
WinLossRatio = AvgWin / AvgLoss


---

##  Kom igång

### Krav
- .NET 8 SDK
- Visual Studio 2022
- SQL Server Express (lokal instans)

### Installation

1. Klona repot:
```bash
git clone https://github.com/Khalil-creatot/TradingJournal.git
```

2. Öppna 'TradingJournal.sln' i Visual Studio

3. Uppdatera connection string i 'App.xaml.cs' med ditt servernamn:
```csharp
@"Server=DITT-SERVERNAMN\SQLEXPRESS;Database=TradingJournalDb;
  Trusted_Connection=True;TrustServerCertificate=True;"
```

4. Kör projektet (F5) – databasen skapas automatiskt vid första uppstart

---

## 🧪 Tester

Projektet innehåller enhetstester för all beräkningslogik:

```bash
dotnet test
```

Testerna verifierar:
- Position size-beräkningar mot verkliga Excel-värden
- PnL-beräkning för Long och Short trades
- Felhantering (SL = Entry, ogiltigt kapital, ogiltig risk%)
- Performance-nyckeltal (equity curve, drawdown, expectancy)

---

##  Vad jag lärde mig

- MVVM i praktiken: Separation mellan logik och UI är svårare än teorin 
  antyder, speciellt med WPF data binding
- OOP-design: Att välja arv vs komposition kräver eftertanke; 
  'Trade' hierarkin fungerade bra men 'ITradeRepository' gav mest värde
- Testdriven tänkning: Att ha Excel-värden som facit gjorde det enkelt 
  att skriva meningsfulla tester tidigt
- Scope-hantering: Dependency injection med scoped DbContext i WPF 
  kräver extra uppmärksamhet

---

##  Framtida förbättringar

- Swing trades (MT) och långsiktiga investeringar (LT)
- DCA-tracking
- Asset allocation-vy
- CSV-export av journal
- Multi-account stöd
- Mörkt/ljust tema

---

## Utvecklare

**Khalil Alizadeh**  
 kandidatexamen i informatik - inriktning systemvetenskap – Högskolan i Borås  
[LinkedIn](https://linkedin.com/in/khalilalizadeh) · [GitHub](https://github.com/Khalil-creatot)