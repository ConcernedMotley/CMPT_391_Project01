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


-- Delete procedure first so you dont have to alter it
IF OBJECT_ID('Get_Enrollment_History', 'p') IS NOT NULL
	DROP PROCEDURE Get_Enrollment_History;
GO

CREATE PROCEDURE Get_Enrollment_History
@CrseYear INT,
@Semester varchar(10)
AS
BEGIN
	SELECT * FROM Section S WHERE S.CrseYear = @CrseYear AND S.Semester = @Semester;
END;
GO

EXEC Get_Enrollment_History @Semester = 'Fall', @CrseYear = 2024;