USE Assignment_2;
GO

CREATE TABLE Course (
	CourseID INT PRIMARY KEY,
	CourseName VARCHAR(50),
	Credit VARCHAR(50),
);
GO

CREATE TABLE Class (
	ClassID INT PRIMARY KEY,
	ClassName INT,
	CourseID INT,
	FOREIGN KEY (CourseID) REFERENCES Course(CourseID),
);
GO

CREATE TABLE Students (
	StudentID INT PRIMARY KEY,
	StudentName VARCHAR(50),
	DateOfBirth DATE,
	Gender VARCHAR(50),
	AddressofStudent VARCHAR(50),
	Email VARCHAR(100) UNIQUE,
	ClassID INT,
	FOREIGN KEY (ClassID) REFERENCES Class(ClassID),
);
GO

CREATE TABLE Teachers (
	TeacherID INT PRIMARY KEY,
	TeacherName INT,
	AddressofTeacher VARCHAR(50),
	Email VARCHAR(100) UNIQUE,
	ClassID INT,
	FOREIGN KEY (ClassID) REFERENCES Class(ClassID),
);
GO 

CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(256) NOT NULL,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100),
    DateOfBirth DATE,
	Role NVARCHAR(50) NOT NULL,
	StudentID INT,
	TeacherID INT,
	FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
	FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID),

);

CREATE TABLE Transcript (
	TranscriptID INT PRIMARY KEY,
	Score INT,
	StudentID INT,
	CourseID INT,
	FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
	FOREIGN KEY (CourseID) REFERENCES Course(CourseID),
);
GO
 


