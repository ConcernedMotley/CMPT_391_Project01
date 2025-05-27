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


-- SP for number of class options
CREATE OR ALTER PROCEDURE GetCourseSearchResults
    @Semester VARCHAR(10),
    @Keyword VARCHAR(100)
AS
BEGIN
    SELECT 
        C.CourseID,
        C.courseLabel,
        C.courseName,
        (
            SELECT COUNT(DISTINCT ST.TimeSlotID)
            FROM Section S2
            JOIN Sect_TimeSlot ST ON S2.SectionID = ST.SectionID
            WHERE S2.CourseID = C.CourseID AND S2.Semester = @Semester
        ) AS ClassCount
    FROM Course C
    WHERE EXISTS (
        SELECT 1 
        FROM Section S 
        WHERE S.CourseID = C.CourseID AND S.Semester = @Semester
    )
    AND (
        C.courseLabel + ' ' + CAST(C.CourseID AS VARCHAR) LIKE @Keyword
        OR C.courseName LIKE @Keyword
    )
    ORDER BY C.CourseID;
END;
