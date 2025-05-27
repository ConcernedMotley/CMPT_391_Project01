-- Stored procedures
USE CMPT_391_P01;

--SEE DATA uncomment Ctrl + U | comment Cntrl + C
--select * from Classroom;
--Select * from Course;
--select * from Sect_TimeSlot;
--select * from Section;
--select * from Student;
--Select * from StudentCredentials;
--select * from Takes;
--select * from Cart;


-- SP for Search
GO

-- Drop if it already exists
IF OBJECT_ID('sp_GetCoursesByLabel', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetCoursesByLabel;
GO

CREATE PROCEDURE sp_GetCoursesByLabel
    @Label VARCHAR(10)
AS
BEGIN
    SELECT CourseID, courseLabel, courseName, credits, prereq
    FROM Course
    WHERE courseLabel LIKE @Label + '%'
END;
GO
