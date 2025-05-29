-- =========================================
-- DATABASE: CMPT_391_P01
-- TRANSACTIONS FOR COURSE REGISTRATION SYSTEM
-- =========================================
-- PROCEDURE: RegisterStudentToSection
-- PURPOSE: Attempts to register a student in a course section, enforcing:
--          - no duplicate registrations
--          - prerequisite fulfillment
--          - schedule conflict checks
--          - course completion restrictions
-- 
-- PARAMETERS:
--   @StudentID  BIGINT  -- ID of the student attempting to register
--   @SectionID  INT     -- Section to register into
--   @CourseID   INT     -- Course related to the section
--
-- NOTES:
--   - Relies on existing data in: Takes, Section, Course, Sect_TimeSlot
--   - Will rollback on any error or conflict
--   - Grade is initialized as NULL until course is completed
-- =========================================
USE CMPT_391_P01;
GO

CREATE OR ALTER PROCEDURE dbo.RegisterStudentToSection
    @StudentID BIGINT,
    @SectionID INT,
    @CourseID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- =====================================
        -- Step 1: Determine Semester for Section
        -- =====================================
        DECLARE @Semester VARCHAR(10);
        SELECT @Semester = Semester FROM Section WHERE SectionID = @SectionID;

        -- =====================================
        -- Step 2: Check for duplicate course registration in same semester
        -- =====================================
        IF EXISTS (
            SELECT 1
            FROM Takes T
            JOIN Section S ON T.SectionID = S.SectionID
            WHERE T.StudentID = @StudentID
              AND S.CourseID = @CourseID
              AND S.Semester = @Semester
        )
        BEGIN
            RAISERROR('You have already registered in another section of this course for the same semester.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- =====================================
        -- Step 3: Check if course already completed
        -- =====================================
        IF EXISTS (
            SELECT 1
            FROM Takes T
            JOIN Section S ON T.SectionID = S.SectionID
            WHERE T.StudentID = @StudentID
              AND S.CourseID = @CourseID
              AND T.Grade IS NOT NULL
        )
        BEGIN
            RAISERROR('You have already completed this course.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- =====================================
        -- Step 4: Check for duplicate section registration
        -- =====================================
        IF EXISTS (
            SELECT 1 
            FROM Takes 
            WHERE StudentID = @StudentID AND SectionID = @SectionID
        )
        BEGIN
            RAISERROR('You are already registered in this section.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- =====================================
        -- Step 5: Check prerequisite
        -- =====================================
        DECLARE @Prereq INT;
        SELECT @Prereq = prereq FROM Course WHERE CourseID = @CourseID;

        IF @Prereq IS NOT NULL AND NOT EXISTS (
            SELECT 1 
            FROM Takes 
            WHERE StudentID = @StudentID AND CourseID = @Prereq AND Grade IS NOT NULL
        )
        BEGIN
            RAISERROR('You have not completed the required prerequisite.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- =====================================
        -- Step 6: Check for schedule conflict
        -- =====================================
        IF EXISTS (
            SELECT 1
            FROM Sect_TimeSlot NewSlot
            JOIN Takes T ON T.StudentID = @StudentID
            JOIN Sect_TimeSlot ExistingSlot ON ExistingSlot.SectionID = T.SectionID
            WHERE NewSlot.SectionID = @SectionID
              AND NewSlot.DayOfWeek = ExistingSlot.DayOfWeek
              AND NewSlot.StartTime < ExistingSlot.EndTime
              AND NewSlot.EndTime > ExistingSlot.StartTime
        )
        BEGIN
            RAISERROR('Schedule conflict with an already registered section.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- =====================================
        -- Step 7: Perform Registration
        -- =====================================
        INSERT INTO Takes (CourseID, SectionID, Grade, StudentID)
        VALUES (@CourseID, @SectionID, NULL, @StudentID);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Rollback on error and re-raise
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @Error NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Error, 16, 1);
    END CATCH
END;
GO
