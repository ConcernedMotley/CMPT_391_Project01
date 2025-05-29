-- =============================================
-- DATABASE: CMPT_391_P01
--  MATERIALIZED VIEWS FOR COURSE REGISTRATION SYSTEM
-- =============================================

USE CMPT_391_P01;
GO

-- =============================================
-- VIEW: vw_SectionAvailability
-- PURPOSE: 
--   Provides detailed information about each section,
--   including timing, location, instructor, and seat availability.
--
-- NOTES:
--   - Uses COUNT_BIG to support SCHEMABINDING.
--   - Used to power section listing and registration forms.
-- =============================================
DROP VIEW IF EXISTS dbo.vw_SectionAvailability;
GO

CREATE VIEW dbo.vw_SectionAvailability
WITH SCHEMABINDING
AS
SELECT 
    S.SectionID,
    S.CourseID,
    S.Semester,
    S.CrseName,
    ST.DayOfWeek,
    ST.StartTime,
    ST.EndTime,
    CR.Building,
    CR.RoomNumber,
    CONCAT(I.FirstName, ' ', I.LastName) AS InstructorName,
    S.Capacity AS TotalSeats,
    COUNT_BIG(T.StudentID) AS EnrolledStudents
FROM dbo.Section S
JOIN dbo.Sect_TimeSlot ST ON S.SectionID = ST.SectionID
JOIN dbo.Classroom CR ON S.ClassroomID = CR.ClassroomID
JOIN dbo.Instructor I ON S.InstructorID = I.InstructorID
LEFT JOIN dbo.Takes T ON S.SectionID = T.SectionID
GROUP BY 
    S.SectionID, S.CourseID, S.Semester, S.CrseName,
    ST.DayOfWeek, ST.StartTime, ST.EndTime,
    CR.Building, CR.RoomNumber,
    I.FirstName, I.LastName,
    S.Capacity;
GO

-- =============================================
-- VIEW: vw_StudentSchedule
-- PURPOSE:
--   Displays a student's full class schedule including time and location.
--
-- USE CASE:
--   Used to validate time conflicts during registration.
--
-- INDEX:
--   Unique index ensures one entry per student/section combination.
-- =============================================
CREATE OR ALTER VIEW dbo.vw_StudentSchedule
WITH SCHEMABINDING
AS
SELECT 
    T.StudentID,
    T.SectionID,
    S.CourseID,
    S.Semester,
    ST.DayOfWeek,
    ST.StartTime,
    ST.EndTime,
    CR.Building,
    CR.RoomNumber
FROM dbo.Takes T
JOIN dbo.Section S ON T.SectionID = S.SectionID
JOIN dbo.Sect_TimeSlot ST ON S.SectionID = ST.SectionID
JOIN dbo.Classroom CR ON S.ClassroomID = CR.ClassroomID;
GO

CREATE UNIQUE CLUSTERED INDEX IX_vw_StudentSchedule
ON dbo.vw_StudentSchedule(StudentID, SectionID);
GO

-- =============================================
-- VIEW: vw_CourseCompletion
-- PURPOSE:
--   Lists all courses that have been completed by students (Grade IS NOT NULL).
--
-- USE CASE:
--   Used to validate prerequisite completion and display academic history.
--
-- INDEX:
--   Ensures unique entries per student per course.
-- =============================================
CREATE OR ALTER VIEW dbo.vw_CourseCompletion
WITH SCHEMABINDING
AS
SELECT 
    T.StudentID,
    S.CourseID,
    T.Grade
FROM dbo.Takes T
JOIN dbo.Section S ON T.SectionID = S.SectionID
WHERE T.Grade IS NOT NULL;
GO

CREATE UNIQUE CLUSTERED INDEX IX_vw_CourseCompletion
ON dbo.vw_CourseCompletion(StudentID, CourseID);
GO
