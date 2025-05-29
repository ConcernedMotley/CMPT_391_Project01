-- ================================================
-- DATABASE RESET & SCHEMA SETUP FOR CMPT_391_P01
-- ================================================

-- Step 1: Make sure you're using a different DB context
USE master;
GO
-- Step 2: Drop the CMPT_391_P01 database if it exists
IF EXISTS (
    SELECT name 
    FROM sys.databases 
    WHERE name = N'CMPT_391_P01'
)
BEGIN
    -- Force disconnect all users and rollback transactions
    ALTER DATABASE CMPT_391_P01 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    
    -- Now safely drop it
    DROP DATABASE CMPT_391_P01;
END
GO

-- Step 3: Recreate the database
CREATE DATABASE CMPT_391_P01;
GO

-- Step 4: Switch to the new database
USE CMPT_391_P01;
GO

-- =============================================
-- TABLE CLEANUP: Drop Tables If They Exist
-- (in correct dependency order)
-- =============================================
DROP TABLE IF EXISTS StudentCredentials;
DROP TABLE IF EXISTS Cart;
DROP TABLE IF EXISTS Takes;
DROP TABLE IF EXISTS Sect_TimeSlot;
DROP TABLE IF EXISTS Classroom;
DROP TABLE IF EXISTS Student;
DROP TABLE IF EXISTS Section;
DROP TABLE IF EXISTS Course;
DROP TABLE IF EXISTS Instructor;

-- =============================================
-- TABLE DEFINITIONS
-- =============================================

-- TABLE: Instructor
-- Stores instructor information
CREATE TABLE Instructor (
    InstructorID INT PRIMARY KEY,
    FirstName NVARCHAR(20),
    LastName NVARCHAR(20),
    Department NVARCHAR(50)
);
-- TABLE: Student
-- Stores student personal and academic details
--Student (student_id, first_name, last_name, department) - Ethan
CREATE TABLE Student (
    StudentID bigint PRIMARY KEY,
    first_name VARCHAR(20),
    last_name VARCHAR(20),
    department VARCHAR(50)
);

-- TABLE: Course
-- Stores course catalog with prerequisites
--Course (course_id,courseLabel, courseName, credits, prereq) - Ethan
CREATE TABLE Course (
    CourseID INT PRIMARY KEY,
    courseLabel VARCHAR(4),      -- e.g., 'CMPT', 'ENGL'
    courseName VARCHAR(50),
    credits INT,
    prereq INT
);

-- TABLE: Classroom
-- Stores information about physical classrooms
--Classroom (classroom_id, building, roomNumber, capacity) - Nat
CREATE TABLE Classroom (
    ClassroomID INT NOT NULL PRIMARY KEY,
    Building VARCHAR(50) NOT NULL,
    RoomNumber VARCHAR(10) NOT NULL,
    Capacity INT NOT NULL,
);

-- TABLE: Section
-- Represents course offerings per semester
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

-- TABLE: StudentCredentials
-- Stores student login details
-- StudentCredentials (student_id, username, studentPassword) - Sankalp
CREATE TABLE StudentCredentials (
	StudentID bigint NOT NULL,
	Username varchar(20) NOT NULL UNIQUE,
	StudentPassword varchar(20) NOT NULL UNIQUE,
	FOREIGN KEY (StudentID) REFERENCES Student(StudentID)
);

-- TABLE: Takes
-- Tracks student registrations and grades
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

-- TABLE: Sect_TimeSlot
-- Stores timing information for each section
--sect_timeSlot (timeslot_id,  section_id, st art_time, end_time, day(s)) - Nat
CREATE TABLE Sect_TimeSlot (
    TimeSlotID INT PRIMARY KEY,
    SectionID INT NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    DayOfWeek VARCHAR(10) NOT NULL,  -- e.g., 'Monday', 'Tue', etc.
    FOREIGN KEY (SectionID) REFERENCES Section(SectionID)
);

-- TABLE: Cart
-- Temporary table to store courses being registered (e.g., for preview before finalizing)
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
