CREATE DATABASE CompanyManager
USE CompanyManager

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
