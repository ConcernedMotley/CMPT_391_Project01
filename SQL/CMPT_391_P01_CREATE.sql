-- Make sure you're using a different database (e.g., master)
USE master;
GO

-- If the database exists, force drop it
IF EXISTS (
    SELECT name 
    FROM sys.databases 
    WHERE name = N'CMPT_391_P01'
)
BEGIN
    -- Force disconnect all users and rollback open transactions
    ALTER DATABASE CMPT_391_P01 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    
    -- Now safely drop it
    DROP DATABASE CMPT_391_P01;
END
GO

-- Recreate the database
CREATE DATABASE CMPT_391_P01;
GO

-- Switch to the new database
USE CMPT_391_P01;
GO

DROP TABLE IF EXISTS StudentCredentials;
DROP TABLE IF EXISTS Cart;
DROP TABLE IF EXISTS Takes;
DROP TABLE IF EXISTS Sect_TimeSlot;
DROP TABLE IF EXISTS Classroom;
DROP TABLE IF EXISTS Student;
DROP TABLE IF EXISTS Section;
DROP TABLE IF EXISTS Course;
DROP TABLE IF EXISTS Instructor;


CREATE TABLE Instructor (
    InstructorID INT PRIMARY KEY,
    FirstName NVARCHAR(20),
    LastName NVARCHAR(20),
    Department NVARCHAR(50)
);

--Student (student_id, first_name, last_name, department) - Ethan
CREATE TABLE Student (
    StudentID bigint PRIMARY KEY,
    first_name VARCHAR(20),
    last_name VARCHAR(20),
    department VARCHAR(50)
);

--Course (course_id,courseLabel, courseName, credits, prereq) - Ethan

CREATE TABLE Course (
    CourseID INT PRIMARY KEY,
    courseLabel VARCHAR(4),      -- e.g., 'CMPT', 'ENGL'
    courseName VARCHAR(50),
    credits INT,
    prereq INT
);

--Classroom (classroom_id, building, roomNumber, capacity) - Nat
CREATE TABLE Classroom (
    ClassroomID INT NOT NULL PRIMARY KEY,
    Building VARCHAR(50) NOT NULL,
    RoomNumber VARCHAR(10) NOT NULL,
    Capacity INT NOT NULL,
);


--Section (section_id, year, semester, course_id, courseName, ClassroomID, Capicity) - Sankalp
CREATE TABLE Section(
	SectionID int NOT NULL PRIMARY KEY,
	ClassroomID INT NOT NULL,
	CourseID int NOT NULL,
	CrseYear int NOT NULL,
	Semester varchar(10) NOT NULL,
	CrseName varchar(50) NOT NULL,
	Capacity INT NOT NULL,
    InstructorID INT,
	FOREIGN KEY (CourseID) REFERENCES Course(CourseID),
	FOREIGN KEY (ClassroomID) REFERENCES Classroom(ClassroomID),
    FOREIGN KEY (InstructorID) REFERENCES Instructor(InstructorID)
);

-- StudentCredentials (student_id, username, studentPassword) - Sankalp
CREATE TABLE StudentCredentials (
	StudentID bigint NOT NULL,
	Username varchar(20) NOT NULL UNIQUE,
	StudentPassword varchar(20) NOT NULL UNIQUE,
	FOREIGN KEY (StudentID) REFERENCES Student(StudentID)
);

--Takes (course_id, section_id, grade, student_id) - Sankalp
CREATE TABLE Takes (
	CourseID int NOT NULL,
	SectionID int NOT NULL,
	Grade VARCHAR (2) null,
	StudentID bigint NOT NULL,
	CONSTRAINT PK_Takes PRIMARY KEY (CourseID, SectionID, StudentID),
	FOREIGN KEY (CourseID) REFERENCES Course(CourseID),
	FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
	FOREIGN KEY (SectionID) REFERENCES Section(SectionID)
);


--sect_timeSlot (timeslot_id,  section_id, st art_time, end_time, day(s)) - Nat
CREATE TABLE Sect_TimeSlot (
    TimeSlotID INT PRIMARY KEY,
    SectionID INT NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    DayOfWeek VARCHAR(10) NOT NULL,  -- e.g., 'Monday', 'Tue', etc.
    FOREIGN KEY (SectionID) REFERENCES Section(SectionID)
);

--Cart (SectionID, StudentID, CourseID, CourseName) - Sankalp
CREATE TABLE Cart (
	
	SectionID INT,
	StudentID BIGINT,
	CourseID INT,
	CourseName VARCHAR(50),
	CONSTRAINT PK_Cart PRIMARY KEY (SectionID, StudentID, CourseID, CourseName),
	FOREIGN KEY (CourseID) REFERENCES Course(CourseID),
	FOREIGN KEY (StudentID) REFERENCES Student(StudentID),
	FOREIGN KEY (SectionID) REFERENCES Section(SectionID)
);
