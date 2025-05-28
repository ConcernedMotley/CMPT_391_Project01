-- Materialized views

--Set the options to support indexed views.
-- REF: https://learn.microsoft.com/en-us/sql/relational-databases/views/create-indexed-views?view=sql-server-ver17&redirectedfrom=MSDN
SET NUMERIC_ROUNDABORT OFF;
SET ANSI_PADDING,
    ANSI_WARNINGS,
    CONCAT_NULL_YIELDS_NULL,
    ARITHABORT,
    QUOTED_IDENTIFIER,
    ANSI_NULLS ON;

GO

--SEE DATA uncomment Ctrl + U | comment Cntrl + C
--select * from Classroom;
--select * from Sect_TimeSlot;
--select * from Section;
--Select * from Course;
--select * from Student;
--Select * from StudentCredentials;
--select * from Takes;
--select * from Cart;


--Views
CREATE VIEW Course_Info
	WITH SCHEMABINDING
AS
SELECT CONCAT(Faculty, '-', CourseID, ': ', CrseName) AS CourseName, Semester, CrseYear, Capicity, 
	DayOfWeek, StartTime, EndTime, 
	RoomNumber, Building
FROM dbo.Section S, dbo.Sect_TimeSlot TS, dbo.Classroom C
WHERE S.SectionID = TS.SectionID
	AND S.ClassroomID = C.ClassroomID
GO

-- See view data
--SELECT * FROM dbo.Course_Info;