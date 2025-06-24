--Umožniť manažovať (vytvárať/upravovať/mazať) max. 4-úrovňovú hierarchickú organizačnú štruktúru firmy: firma → divízie → projekty → oddelenia.
--Každý z uzlov organizačnej štruktúry bude pomenovaný názvom a kódom a bude mať svojho vedúceho 
--(firma – riaditeľ, divízia – vedúci divízie, projekt – vedúci projektu, 	oddelenie – vedúci oddelenia). Vedúci uzla je niektorý zo zamestnancov firmy.

--Umožniť pridávať, meniť a vymazávať zamestnancov.
--Pre zamestnanca sa bude evidovať minimálne titul, meno a priezvisko, telefón a e-mail.


DROP TABLE [dbo].[Departments]
DROP TABLE [dbo].[Projects]
DROP TABLE [dbo].[Divisions]
DROP TABLE [dbo].[Companies]
DROP TABLE [dbo].[Employees]

CREATE TABLE Employees(
	Id_Employee int Identity(1,1) PRIMARY KEY,
	First_Name varchar(30) NOT NULL,
	Last_Name varchar(30) NOT NULL,
	Title varchar(6),
	Phone varchar(12) NOT NULL,
	Email varchar(100) NOT NULL
)

CREATE TABLE Companies(
	Id_Company int Identity(1,1) PRIMARY KEY,
	Com_Name varchar (100) NOT NULL,
	Code varchar(10) NOT NULL,
	Id_Boss int NOT NULL FOREIGN KEY REFERENCES Employees(Id_Employee)
)

CREATE TABLE Divisions(
	Id_Division int Identity(1,1) PRIMARY KEY,
	Div_Name varchar (100) NOT NULL,
	Code varchar(10) NOT NULL,
	Id_Company int NOT NULL FOREIGN KEY REFERENCES Companies(Id_Company),
	Id_Boss int NOT NULL FOREIGN KEY REFERENCES Employees(Id_Employee)
)

CREATE TABLE Projects(
	Id_Project int Identity(1,1) PRIMARY KEY,
	Pro_Name varchar (100) NOT NULL,
	Code varchar(10) NOT NULL,
	Id_Division int NOT NULL FOREIGN KEY REFERENCES Divisions(Id_Division),
	Id_Boss int NOT NULL FOREIGN KEY REFERENCES Employees(Id_Employee)
)

CREATE TABLE Departments(
	Id_Department int Identity(1,1) PRIMARY KEY,
	Dep_Name varchar (100) NOT NULL,
	Code varchar(10) NOT NULL,
	Id_Project int NOT NULL FOREIGN KEY REFERENCES Projects(Id_Project),
	Id_Boss int NOT NULL FOREIGN KEY REFERENCES Employees(Id_Employee)
);