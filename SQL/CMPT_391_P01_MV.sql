USE CMPT_391_P01;
GO

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
-- View: vw_SectionInstructor
-- Description: (Optional) Lists instructors assigned to each section
-- =============================================
-- CREATE OR ALTER VIEW dbo.vw_SectionInstructor
-- WITH SCHEMABINDING
-- AS
-- SELECT 
--     S.SectionID,
--     I.InstructorID,
--     I.FirstName,
--     I.LastName
-- FROM dbo.Section S
-- JOIN dbo.Instructor I ON S.InstructorID = I.InstructorID;
-- GO

-- CREATE UNIQUE CLUSTERED INDEX IX_vw_SectionInstructor_SectionID
-- ON dbo.vw_SectionInstructor (SectionID);
-- GO
-- =============================================
-- View: vw_StudentSchedule
-- Description: Shows detailed schedule for each student
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
-- View: vw_CourseCompletion
-- Description: Lists all completed courses by students
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
