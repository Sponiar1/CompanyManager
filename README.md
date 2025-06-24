# CompanyManager

## Nastavenie databázy 
https://www.microsoft.com/en-us/sql-server/sql-server-downloads

1. Inštalácia SQL Server Express 
a) Pri inštalácií pre jednoduchosť nastavíme autentifikáciu na Windows Authentification.
b) Je možné nainštalovať aj SQL Server Management Studio pre prácu so skriptami

2. Po nainštalovaní vytvoríme novú databázu a spustíme v nej skript2.sql

## Pripojenie aplikácie na databázu
1. Zmena ConnectionString
V appsettings.json zmeníme nasledujúci String:
'' "ConnectionStrings": {
    "DefaultConnection": "Data Source=[1];Initial Catalog=[2]; Trusted_Connection=True; Encrypt=False;TrustServerCertificate=True;"
}, ''

Kde:
[1] zmeníme na názov servera 
[2] zmeníme na názov databázy
(Názov servera je možné získať skriptom skript1.sql)

## Spustenie aplikácie
Aplikácia funguje na porte 7214. Api sa dá testovať pomocou ScalaR: https://localhost:7214/scalar/v1
