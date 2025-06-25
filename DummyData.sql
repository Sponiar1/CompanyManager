USE CompanyManager;

INSERT INTO Employees (First_Name, Last_Name, Title, Phone, Email) VALUES
('Ján', 'Novák', 'Ing.', '0912345678', 'jan.novak@example.com'),
('Petra', 'Vyšná', 'Mgr.', '0908765432', 'petrav@example.com'),
('Marek', 'Malý', NULL, '0901111111', 'marekmalik@example.com'),
('Marek', 'Kováè', 'Bc.', '0902222222', 'kovacmarek@example.com'),
('Róbert', 'Krivý', 'Ing.', '0903333333', 'robert.krivy@example.com'),
('Peter', 'Podhorny', 'Ing.', '0902345678', 'peter.podhorny@example.com'),
('Vincent', 'Dolny', NULL, '0907654321', 'vincentdolny@example.com'),
('Pavlína', 'Mraèná', NULL, '0904444444', 'mracnap@example.com'),
('Denisa', 'Noèná', NULL, '0905555555', 'deniskanocna@example.com'),
('Norbert', 'Antonsky', NULL, '0907777777', 'antonskyR@example.com'),
('Andrej', 'Most', 'Ing.', '0906666666', 'mostandrej@example.com'),
('Andrea', 'Nízka', 'Ing.', '0908888888', 'andrea.nizka@example.com');

INSERT INTO Companies (Com_Name, Code, Id_Boss) VALUES
('Dummy s.r.o.', 'DUM', 1);

INSERT INTO Divisions (Div_Name, Code, Id_Company, Id_Boss) VALUES
('Vývoj', 'D001', 1, 2),    
('Marketing', 'D002', 1, 3),
('QA', 'D003', 1, 8),
('HR', 'D004', 1, 7);

INSERT INTO Projects (Pro_Name, Code, Id_Division, Id_Boss) VALUES
('Mobilná aplikácia', 'PV01', 1, 4),
('Webová aplikácia', 'PV02', 1, 5),
('Berlin', 'PM01', 2, 6),
('Bratislava', 'PM02', 2, 11);    

INSERT INTO Departments (Dep_Name, Code, Id_Project, Id_Boss) VALUES
('iOS', 'O001', 1, 1),
('Frontend', 'O002', 2, 9),
('Backend', 'O003', 2, 9),
('Reklamy', 'O004', 3, 10);