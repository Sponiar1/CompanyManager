CREATE TABLE Zamestnanci(
	ID int Identity(1,1) PRIMARY KEY,
	Meno varchar(30) NOT NULL,
	Priezvisko varchar(30) NOT NULL,
	Titul varchar(6),
	Telefon varchar(12) NOT NULL,
	Email varchar(100) NOT NULL
)

CREATE TABLE Firma(
	ID int Identity(1,1) PRIMARY KEY,
	Nazov varchar (100) NOT NULL,
	Kod varchar(10) NOT NULL,
	Riaditel int FOREIGN KEY REFERENCES Zamestnanci(ID)
)

CREATE TABLE Divizia(
	ID int Identity(1,1) PRIMARY KEY,
	Nazov varchar (100) NOT NULL,
	Kod varchar(10) NOT NULL,
	Firma int FOREIGN KEY REFERENCES Firma(ID),
	Veduci int FOREIGN KEY REFERENCES Zamestnanci(ID)
)

CREATE TABLE Projekty(
	ID int Identity(1,1) PRIMARY KEY,
	Nazov varchar (100) NOT NULL,
	Kod varchar(10) NOT NULL,
	Divizia int FOREIGN KEY REFERENCES Divizia(ID),
	Veduci int FOREIGN KEY REFERENCES Zamestnanci(ID)
)

CREATE TABLE Oddelenie(
	ID int Identity(1,1) PRIMARY KEY,
	Nazov varchar (100) NOT NULL,
	Kod varchar(10) NOT NULL,
	Projekt int FOREIGN KEY REFERENCES Projekty(ID),
	Veduci int FOREIGN KEY REFERENCES Zamestnanci(ID)
);
