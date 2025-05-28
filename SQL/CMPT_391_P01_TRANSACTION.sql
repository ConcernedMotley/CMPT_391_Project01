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

        -- Check if already registered in a section of the same course in the same semester
        IF EXISTS (
            SELECT 1
            FROM Takes T
            JOIN Section S ON T.SectionID = S.SectionID
            WHERE T.StudentID = @StudentID
              AND S.CourseID = @CourseID
              AND S.Semester = (SELECT Semester FROM Section WHERE SectionID = @SectionID)
        )
        BEGIN
            RAISERROR('You have already registered in another section of this course for the same semester.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Check if already completed the course
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
        -- Check if already registered in same section
        IF EXISTS (
            SELECT 1 FROM Takes
            WHERE StudentID = @StudentID AND SectionID = @SectionID
        )
        BEGIN
            RAISERROR('You are already registered in this section.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Insert into Takes
        INSERT INTO Takes (CourseID, SectionID, Grade, StudentID)
        VALUES (@CourseID, @SectionID, NULL, @StudentID);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
