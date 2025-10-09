USE [StacktimDb]
GO

create table Players(
	ID INT PRIMARY KEY IDENTITY NOT NULL,
	PSEUDO VARCHAR(40) NOT NULL UNIQUE,
	EMAIL VARCHAR(100) NOT NULL UNIQUE,
	Rank VARCHAR(20) CHECK (Rank IN ('Bronze', 'Silver', 'Gold', 'Platinum', 'Diamond', 'Master')),
	TotalScore int DEFAULT 0 CHECK(TotalScore >= 0),
	RegistrationDate DATETIME DEFAULT GETDATE()
)

create table Teams(
	ID INT PRIMARY KEY IDENTITY NOT NULL,
	Name VARCHAR(100) UNIQUE NOT NULL,
	Tag VARCHAR(3) UNIQUE NOT NULL CHECK(LEN(Tag) = 3 AND Tag = UPPER(Tag)),
	CaptainId INT,
	CONSTRAINT FK_Teams_Players FOREIGN KEY (CaptainId) REFERENCES Players(ID),
	CreationDate DATETIME DEFAULT GETDATE()
)

create table TeamPlayers(
	TeamId INT,
	CONSTRAINT FK_TeamPlayers_Teams FOREIGN KEY (TeamId) REFERENCES Teams(ID),
	PlayerId INT,
	CONSTRAINT FK_TeamPlayers_Players FOREIGN KEY (PlayerId) REFERENCES Players(ID),
	Role INT CHECK (Role IN (0, 1, 2)),
	JoinDate DATETIME DEFAULT GETDATE()
)