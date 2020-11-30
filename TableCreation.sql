CREATE DATABASE CatFeeding;

CREATE TABLE Users (
	     Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	  Login VARCHAR(100),
   Password VARCHAR(100),
   Nickname NVARCHAR(100),
       Role NVARCHAR(50)
);

CREATE TABLE Pets (
	     Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	   Name VARCHAR(50),
       Owner_Id INT NOT NULL FOREIGN KEY REFERENCES Users
);

CREATE TABLE FeedTime (
	     Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	    User_Id INT NOT NULL FOREIGN KEY REFERENCES Users,
		 Pet_Id INT NOT NULL FOREIGN KEY REFERENCES Pets,
	  Feed_Time DATETIME2(7)
);

CREATE TABLE UsersPets (
	     Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	    User_Id INT NOT NULL FOREIGN KEY REFERENCES Users,
		 Pet_Id INT NOT NULL FOREIGN KEY REFERENCES Pets,
);

CREATE TABLE Statistics (
	       Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	     Name nvarchar(50) NOT NULL,
  Description nvarchar(250) NOT NULL,
SQLExpression nvarchar(max) NOT NULL
);

INSERT INTO Statistics (Name, Description, SqlExpression) VALUES ('DifferenceInFeeding', 'Return the difference between weekends and non-weekends feeding' , 
'Select Pet_Id, 
CONVERT(decimal(4, 1), ROUND(COUNT(CASE WHEN DATEPART(W, Feed_Time) = 6 OR DATEPART(W, Feed_Time) = 7 THEN 0 END) / 2. , 2)) - CONVERT(decimal(4, 1), 
ROUND(COUNT(CASE WHEN DATEPART(W, Feed_Time) != 6 AND DATEPART(W, Feed_Time) != 7 THEN 0 END) / 5. , 2)) as Diff
FROM FeedTime 
GROUP BY Pet_Id;');
