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
        C.courseLabel + ' ' + CAST(C.CourseID AS VARCHAR) AS [Course], -- Combined
        C.courseName AS [Course Name],
        S.Semester,
        T.Grade AS [Letter Grade]
    FROM Takes T
    INNER JOIN Section S ON T.SectionID = S.SectionID
    INNER JOIN Course C ON T.CourseID = C.CourseID
    WHERE T.StudentID = @StudentID
END;
GO



-- =========================================
-- PROCEDURE: Add Course to Cart
-- Inserts new course section to student's cart
-- =========================================
CREATE OR ALTER PROCEDURE sp_AddToCart
    @StudentID BIGINT,
    @SectionID INT,
    @CourseID INT,
    @Semester VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CourseName VARCHAR(50);

    -- Get course name from Course table
    SELECT @CourseName = courseName FROM Course WHERE CourseID = @CourseID;

    IF @CourseName IS NULL OR @CourseID = 0
    BEGIN
        RAISERROR('Invalid CourseID. Course not found.', 16, 1);
        RETURN;
    END

    BEGIN TRY
        INSERT INTO Cart (StudentID, SectionID, CourseID, CourseName, Semester)
        VALUES (@StudentID, @SectionID, @CourseID, @CourseName, @Semester);
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() = 2627
        BEGIN
            RAISERROR('This section is already in your cart.', 16, 1);
        END
        ELSE
        BEGIN
            DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
            DECLARE @ErrSeverity INT = ERROR_SEVERITY();
            THROW 50000, @ErrMsg, 1;
        END
    END CATCH
END
GO

-- =========================================
-- PROCEDURE: Remove Specific Course from Cart
-- Deletes a single course from the cart
-- =========================================
CREATE OR ALTER PROCEDURE sp_RemoveFromCart
    @StudentID BIGINT,
    @SectionID INT,
    @CourseID INT
AS
BEGIN
    DELETE FROM Cart
    WHERE StudentID = @StudentID
      AND SectionID = @SectionID
      AND CourseID = @CourseID;
END
GO

-- =========================================
-- PROCEDURE: Clear Entire Cart
-- Removes all courses from the cart for a student
-- =========================================
CREATE OR ALTER PROCEDURE sp_ClearCart
    @StudentID BIGINT
AS
BEGIN
    DELETE FROM Cart WHERE StudentID = @StudentID;
END
GO

-- =========================================
-- PROCEDURE: Get Cart Items
-- Returns cart contents for a student, optionally filtered by semester
-- =========================================
CREATE OR ALTER PROCEDURE sp_GetCartItems
    @StudentID BIGINT,
    @Semester VARCHAR(20) = 'All'
AS
BEGIN
    SET NOCOUNT ON;

    IF @Semester = 'All'
    BEGIN
        SELECT SectionID, StudentID, CourseID, CourseName, Semester
        FROM Cart
        WHERE StudentID = @StudentID;
    END
    ELSE
    BEGIN
        SELECT SectionID, StudentID, CourseID, CourseName, Semester
        FROM Cart
        WHERE StudentID = @StudentID AND Semester = @Semester;
    END
END
GO

-- SP to clear all registed courses 
CREATE OR ALTER PROCEDURE sp_RemoveFromCart
    @StudentID BIGINT,
    @SectionID INT,
    @CourseID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Cart
    WHERE StudentID = @StudentID
      AND SectionID = @SectionID
      AND CourseID = @CourseID;
END
GO
