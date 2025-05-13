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

--Course (course_id, courseName, credits, prereq) - Ethan

--Section (section_id, year, semester, course_id, courseName) - Sankalp

--Takes (course_id, section_id, grade, student_id) - Sankalp

-- StudentCredentials (student_id, username, studentPassword) - Sankalp
CREATE TABLE StudentCredentials (
	StudentID bigint NOT NULL PRIMARY KEY,
	Username varchar(20) NOT NULL UNIQUE,
	StudentPassword varchar(20) NOT NULL UNIQUE,
	--FOREIGN KEY (StudentID) REFERENCES Student(StudentID)
);

--sect_timeSlot (timeslot_id,  section_id, st art_time, end_time, day(s)) - Nat

--Classroom (classroom_id, section_id, building, roomNumber, capacity) - Nat

