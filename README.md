# CompanyManager

## Nastavenie databázy

1. Stiahnite a nainštalujte **SQL Server Express**  
   [SQL Server na stiahnutie](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
   - Pri inštalácii odporúčame zvoliť autentifikáciu typu **Windows Authentication** (pre jednoduchosť).
   - Voliteľne môžete nainštalovať aj **SQL Server Management Studio** pre pohodlnú prácu so skriptami.

2. Po nainštalovaní:
   - Spustite na servery skript `Tables.sql` (vytvorí databázu aj tabuľky)

---

## Pripojenie aplikácie k databáze

1. Otvorte súbor `appsettings.json`.
2. Upravte sekciu ConnectionStrings nasledovne:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=[SERVER_NAME];Initial Catalog=[DB_NAME];Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;"
   }

Kde:
- [SERVER_NAME] – názov vášho SQL servera (môžete zistiť spustením skriptu skript1.sql)
- [DB_NAME] – názov vytvorenej databázy

## Spustenie aplikácie
Aplikácia funguje na porte 7214. Api sa dá testovať pomocou ScalaR: https://localhost:7214/scalar/v1
