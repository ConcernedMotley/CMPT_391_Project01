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

-- Drop child tables first
DROP TABLE IF EXISTS StudentCredentials;

-- Drop parent tables
DROP TABLE IF EXISTS Student;

--TBD if parent or child
DROP TABLE IF EXISTS Course;
DROP TABLE IF EXISTS Section;
DROP TABLE IF EXISTS Takes;
DROP TABLE IF EXISTS SectionTimeSlot;
DROP TABLE IF EXISTS Classroom;


--Student (student_id, first_name, last_name, department) - Ethan

CREATE TABLE Student (
    student_id INT PRIMARY KEY,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    department VARCHAR(100)
);

--Course (course_id, courseName, credits, prereq) - Ethan

CREATE TABLE Course (
    CourseID INT PRIMARY KEY,
    courseName VARCHAR(100),
    credits INT,
    prereq INT,
    --FOREIGN KEY (prereq) REFERENCES Course(course_id)
);


--Section (section_id, year, semester, course_id, courseName) - Sankalp
CREATE TABLE Section(
	SectionID int NOT NULL PRIMARY KEY,
	CourseID int NOT NULL,
	CrseYear int NOT NULL,
	Semester varchar(10) NOT NULL,
	CrseName varchar(20) NOT NULL,
	FOREIGN KEY (CourseID) REFERENCES Course(CourseID)
);

-- StudentCredentials (student_id, username, studentPassword) - Sankalp
CREATE TABLE StudentCredentials (
	StudentID bigint NOT NULL PRIMARY KEY,
	Username varchar(20) NOT NULL UNIQUE,
	StudentPassword varchar(20) NOT NULL UNIQUE,
	--FOREIGN KEY (StudentID) REFERENCES Student(StudentID)
);

--Takes (course_id, section_id, grade, student_id) - Sankalp
CREATE TABLE Takes (
	CourseID int NOT NULL,
	SectionID int NOT NULL,
	Grade float,
	StudentID bigint NOT NULL,
	CONSTRAINT PK_Takes PRIMARY KEY (CourseID, SectionID, Grade, StudentID),
	FOREIGN KEY (CourseID) REFERENCES Course(CourseID),
	FOREIGN KEY (StudentID) REFERENCES StudentCredentials(StudentID),
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
--Classroom (classroom_id, section_id, building, roomNumber, capacity) - Nat
CREATE TABLE Classroom (
    ClassroomID INT PRIMARY KEY,
    SectionID INT NOT NULL UNIQUE,  -- Unique if one classroom per section
    Building VARCHAR(50) NOT NULL,
    RoomNumber VARCHAR(10) NOT NULL,
    Capacity INT NOT NULL,
    FOREIGN KEY (SectionID) REFERENCES Section(SectionID)
);
