-- =========================================
-- DATABASE: CMPT_391_P01
-- STORED PROCEDURES FOR COURSE REGISTRATION SYSTEM
-- =========================================
USE CMPT_391_P01;
GO

-- =========================================
-- DEBUGGING: View Table Data (Optional)
-- =========================================
-- Uncomment any of the lines below to view table data:
-- SELECT * FROM Classroom;
-- SELECT * FROM Course;
-- SELECT * FROM Sect_TimeSlot;
-- SELECT * FROM Section;
-- SELECT * FROM Student;
-- SELECT * FROM StudentCredentials;
-- SELECT * FROM Takes;
-- SELECT * FROM Cart;

-- =========================================
-- PROCEDURE: Validate Student Login
-- Returns StudentID if login is successful
-- =========================================
CREATE OR ALTER PROCEDURE sp_ValidateStudentLogin
    @Username VARCHAR(20),
    @Password VARCHAR(20)
AS
BEGIN
    SELECT StudentID
    FROM StudentCredentials
    WHERE Username = @Username AND StudentPassword = @Password;
END;
GO

-- =========================================
-- PROCEDURE: Get Courses by Label
-- Returns basic course info based on label prefix
-- =========================================
IF OBJECT_ID('sp_GetCoursesByLabel', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetCoursesByLabel;
GO

CREATE PROCEDURE sp_GetCoursesByLabel
    @Label VARCHAR(10)
AS
BEGIN
    SELECT CourseID, courseLabel, courseName, credits, prereq
    FROM Course
    WHERE courseLabel LIKE @Label + '%';
END;
GO

-- =========================================
-- PROCEDURE: Get Course Search Results
-- Returns all courses and their class count for a semester and keyword
-- =========================================
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
        (C.courseLabel + ' ' + CAST(C.CourseID AS VARCHAR(10))) LIKE @Keyword
        OR C.courseName LIKE @Keyword
    )
    ORDER BY C.CourseID;
END;
GO

-- =========================================
-- PROCEDURE: Get Student's Full Name
-- =========================================
CREATE OR ALTER PROCEDURE GetStudentFullName
    @StudentID INT
AS
BEGIN
    SELECT first_name + ' ' + last_name AS FullName
    FROM Student
    WHERE StudentID = @StudentID;
END;
GO

-- =========================================
-- PROCEDURE: Get Available Course Sections
-- Uses view: vw_SectionAvailability
-- =========================================
CREATE OR ALTER PROCEDURE dbo.GetCourseSections
    @CourseID INT,
    @Semester VARCHAR(10)
AS
BEGIN
    SELECT SectionID, CrseName, DayOfWeek, StartTime, EndTime,
           Building, RoomNumber, InstructorName, TotalSeats, EnrolledStudents
    FROM vw_SectionAvailability
    WHERE CourseID = @CourseID AND Semester = @Semester
    ORDER BY DayOfWeek, StartTime;
END;
GO

-- =========================================
-- PROCEDURE: Get Formatted Prerequisite Chain
-- Recursive CTE to return full prereq path for a course
-- =========================================
CREATE OR ALTER PROCEDURE GetFormattedCoursePrerequisites
    @CourseID INT
AS
BEGIN
    WITH PrereqChain AS (
        SELECT prereq
        FROM Course
        WHERE CourseID = @CourseID
        UNION ALL
        SELECT c.prereq
        FROM Course c
        INNER JOIN PrereqChain pc ON c.CourseID = pc.prereq
    )
    SELECT 
        c.courseLabel + ' ' + CAST(c.CourseID AS VARCHAR) + ' - ' + c.courseName AS FormattedCourse
    FROM Course c
    JOIN PrereqChain p ON c.CourseID = p.prereq
    WHERE p.prereq IS NOT NULL;
END;
GO

-- =========================================
-- PROCEDURE: Get Registered Classes for a Student
-- Optionally filters by semester and year
-- =========================================
CREATE OR ALTER PROCEDURE GetRegisteredClassesForStudent
    @StudentID INT,
    @Semester NVARCHAR(10) = NULL,
    @CrseYear INT = NULL
AS
BEGIN
    SELECT 
        s.SectionID,
        c.CourseName,
        c.courseLabel,
        c.credits,
        s.Semester,
        s.CrseYear,
        cr.Building,
        cr.RoomNumber,
        CONCAT(i.FirstName, ' ', i.LastName) AS InstructorName,
        st.DayOfWeek,
        st.StartTime,
        st.EndTime
    FROM Takes t
    INNER JOIN Section s ON t.SectionID = s.SectionID
    INNER JOIN Course c ON s.CourseID = c.CourseID
    INNER JOIN Instructor i ON s.InstructorID = i.InstructorID
    INNER JOIN Classroom cr ON s.ClassroomID = cr.ClassroomID
    INNER JOIN Sect_TimeSlot st ON s.SectionID = st.SectionID
    WHERE 
        t.StudentID = @StudentID
        AND (@Semester IS NULL OR s.Semester = @Semester)
        AND (@CrseYear IS NULL OR s.CrseYear = @CrseYear)
    ORDER BY s.CrseYear, s.Semester, st.DayOfWeek, st.StartTime;
END;
GO

-- =========================================
-- PROCEDURE: Get Completed Courses for a Student
-- Groups results by academic year
-- =========================================
CREATE OR ALTER PROCEDURE GetCompletedCoursesForStudent
    @StudentID INT
AS
BEGIN
    SELECT 
        c.CourseID,
        c.courseLabel,
        c.CourseName,
        s.Semester,
        s.CrseYear,
        CASE 
            WHEN s.Semester = 'Fall' THEN s.CrseYear
            WHEN s.Semester = 'Winter' THEN s.CrseYear - 1
            ELSE s.CrseYear
        END AS AcademicYear
    FROM Takes t
    INNER JOIN Section s ON t.SectionID = s.SectionID
    INNER JOIN Course c ON s.CourseID = c.CourseID
    WHERE t.StudentID = @StudentID
      AND t.Grade IS NOT NULL -- Only completed classes
    ORDER BY 
        AcademicYear DESC,
        s.Semester DESC;
END;
GO
