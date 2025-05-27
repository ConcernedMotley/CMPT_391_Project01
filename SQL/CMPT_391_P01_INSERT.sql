USE CMPT_391_P01;


--Delete old records
DELETE FROM Cart;
DELETE FROM Sect_TimeSlot;
DELETE FROM Takes;
DELETE FROM StudentCredentials;
DELETE FROM Section;
DELETE FROM Course;
DELETE FROM Student;
DELETE FROM Classroom;

-- INSERT INTO Student
INSERT INTO Student (StudentID, first_name, last_name, department)
VALUES  
(1000000, 'Alice', 'Nguyen', 'Computer Science'),
(1000001, 'Brandon', 'Lee', 'Mechanical Engineering'),
(1000002, 'Carla', 'Martinez', 'Psychology'),
(1000003, 'Daniel', 'Kim', 'Mathematics'),
(1000004, 'Emily', 'Smith', 'Biology'),
(1000005, 'Faisal', 'Khan', 'Chemistry'),
(1000006, 'Grace', 'Johnson', 'Business'),
(1000007, 'Henry', 'Zhao', 'Physics'),
(1000008, 'Irene', 'Davis', 'Sociology'),
(1000009, 'Jack', 'Wilson', 'Computer Science'),
(1000010, 'Kaitlyn', 'Brown', 'Nursing'),
(1000011, 'Liam', 'Patel', 'History'),
(1000012, 'Maya', 'Lopez', 'English Literature'),
(1000013, 'Noah', 'Green', 'Political Science'),
(1000014, 'Olivia', 'White', 'Environmental Science'),
(1000015, 'Priya', 'Shah', 'Biomedical Engineering'),
(1000016, 'Quentin', 'Adams', 'Economics'),
(1000017, 'Rachel', 'Chen', 'Art History'),
(1000018, 'Samuel', 'Robinson', 'Philosophy'),
(1000019, 'Tina', 'Brooks', 'Education'),
(1000020, 'Aaron', 'Reed', 'Computer Science'),
(1000021, 'Bella', 'Tran', 'Computer Science'),
(1000022, 'Chris', 'Ng', 'Computer Science'),
(1000023, 'Diana', 'Park', 'Computer Science'),
(1000024, 'Ethan', 'Wright', 'Computer Science'),
(1000025, 'Farah', 'Ali', 'Computer Science'),
(1000026, 'Gavin', 'Olsen', 'Computer Science'),
(1000027, 'Hannah', 'Cruz', 'Computer Science'),
(1000028, 'Isaac', 'Morgan', 'Computer Science'),
(1000029, 'Jasmine', 'Bell', 'Computer Science'),
(1000030, 'Kyle', 'Foster', 'Computer Science'),
(1000031, 'Lena', 'Chow', 'Computer Science'),
(1000032, 'Miles', 'Thompson', 'Computer Science'),
(1000033, 'Nina', 'Sanders', 'Computer Science'),
(1000034, 'Oscar', 'Murphy', 'Computer Science');

--Credentials
INSERT INTO StudentCredentials (StudentID, Username, StudentPassword)
VALUES
(1000000, 'nguyen.a', 'pass1000000'),
(1000001, 'lee.b', 'pass1000001'),
(1000002, 'martinez.c', 'pass1000002'),
(1000003, 'kim.d', 'pass1000003'),
(1000004, 'smith.e', 'pass1000004'),
(1000005, 'khan.f', 'pass1000005'),
(1000006, 'johnson.g', 'pass1000006'),
(1000007, 'zhao.h', 'pass1000007'),
(1000008, 'davis.i', 'pass1000008'),
(1000009, 'wilson.j', 'pass1000009'),
(1000010, 'brown.k', 'pass1000010'),
(1000011, 'patel.l', 'pass1000011'),
(1000012, 'lopez.m', 'pass1000012'),
(1000013, 'green.n', 'pass1000013'),
(1000014, 'white.o', 'pass1000014'),
(1000015, 'shah.p', 'pass1000015'),
(1000016, 'adams.q', 'pass1000016'),
(1000017, 'chen.r', 'pass1000017'),
(1000018, 'robinson.s', 'pass1000018'),
(1000019, 'brooks.t', 'pass1000019'),
(1000020, 'reed.a', 'pass1000020'),
(1000021, 'tran.b', 'pass1000021'),
(1000022, 'ng.c', 'pass1000022'),
(1000023, 'park.d', 'pass1000023'),
(1000024, 'wright.e', 'pass1000024'),
(1000025, 'ali.f', 'pass1000025'),
(1000026, 'olsen.g', 'pass1000026'),
(1000027, 'cruz.h', 'pass1000027'),
(1000028, 'morgan.i', 'pass1000028'),
(1000029, 'bell.j', 'pass1000029'),
(1000030, 'foster.k', 'pass1000030'),
(1000031, 'chow.l', 'pass1000031'),
(1000032, 'thompson.m', 'pass1000032'),
(1000033, 'sanders.n', 'pass1000033'),
(1000034, 'murphy.o', 'pass1000034');

-- Classroom
INSERT INTO Classroom (ClassroomID, Building, RoomNumber, Capacity)
VALUES
(1, 'Anderson Hall', '101A', 50),
(2, 'Bennett Center', '202B', 40),
(3, 'Chen Building', '303C', 35),
(4, 'Diaz Pavilion', '104D', 50),
(5, 'Evans Annex', '105E', 45),
(6, 'Anderson Hall', '106A', 60),
(7, 'Bennett Center', '107B', 50),
(8, 'Chen Building', '108C', 30),
(9, 'Diaz Pavilion', '109D', 25),
(10, 'Evans Annex', '110E', 40),
(11, 'Anderson Hall', '111A', 35),
(12, 'Bennett Center', '112B', 45),
(13, 'Chen Building', '113C', 40),
(14, 'Diaz Pavilion', '114D', 35),
(15, 'Evans Annex', '115E', 50),
(16, 'Anderson Hall', '116A', 30),
(17, 'Bennett Center', '117B', 25),
(18, 'Chen Building', '118C', 50),
(19, 'Diaz Pavilion', '119D', 35),
(20, 'Evans Annex', '120E', 40);

--Insert Courses
INSERT INTO Course (CourseID, courseName, credits, prereq)
VALUES
(101, 'Intro to Programming', 3, NULL),
(200, 'Data Structures', 3, 101),
(204, 'Algorithms', 3, 200),
(229, 'Computer Architecture', 3, 200),
(360, 'Operating Systems', 3, 200),
(291, 'Databases', 3, 101),
(395, 'Software Engineering', 3, 200),
(315, 'Web Development', 3, 200),
(305, 'Mobile App Development', 3, 395),
(255, 'Machine Learning', 3, 204),
(355, 'Artificial Intelligence', 3, 255),
(361, 'Networks', 3, 360),
(280, 'Cybersecurity Fundamentals', 3, 229),
(250, 'Human-Computer Interaction', 3, 101),
(272, 'Discrete Math', 3, NULL),
(104, 'Theory of Computation', 3, NULL),
(391, 'Advanced Databases', 3, 291),
(299, 'Cloud Computing', 3, 200),
(497, 'DevOps Practices', 3, 395),
(496, 'Capstone Project', 3, 395);

-- Insert Section
INSERT INTO Section (SectionID, CourseID, CrseYear, Semester, CrseName, ClassroomID, Capicity)
VALUES
(1, 101, 2024, 'Fall', 'Intro to Programming', 1, 50),
(2, 200, 2024, 'Fall', 'Data Structures', 2, 40),
(3, 204, 2024, 'Winter', 'Algorithms', 3, 35),
(4, 229, 2024, 'Winter', 'Computer Architecture', 4, 50),
(5, 360, 2025, 'Fall', 'Operating Systems', 5, 45),
(6, 291, 2024, 'Fall', 'Databases', 6, 60),
(7, 395, 2025, 'Winter', 'Software Engineering', 7, 50),
(8, 315, 2024, 'Spring', 'Web Development', 8, 30),
(9, 305, 2025, 'Spring', 'Mobile App Development', 9, 25),
(10, 255, 2025, 'Fall', 'Machine Learning', 10, 40),
(11, 355, 2025, 'Winter', 'Artificial Intelligence', 11, 35),
(12, 361, 2025, 'Fall', 'Networks', 12, 45),
(13, 280, 2024, 'Winter', 'Cybersecurity Fundamentals', 13, 40),
(14, 250, 2024, 'Fall', 'Human-Computer Interaction', 14, 35),
(15, 272, 2024, 'Fall', 'Discrete Math', 15, 50),
(16, 104, 2024, 'Spring', 'Theory of Computation', 16, 30),
(17, 391, 2025, 'Spring', 'Advanced Databases', 17, 25),
(18, 299, 2025, 'Winter', 'Cloud Computing', 18, 50),
(19, 497, 2025, 'Spring', 'DevOps Practices', 19, 35),
(20, 496, 2025, 'Spring', 'Capstone Project', 20, 40);


-- Sect timeslots
INSERT INTO Sect_TimeSlot (TimeSlotID, SectionID, StartTime, EndTime, DayOfWeek)
VALUES
(1, 1, '09:00', '10:20', 'Monday'),
(2, 2, '10:30', '11:50', 'Tuesday'),
(3, 3, '13:00', '14:20', 'Wednesday'),
(4, 4, '14:30', '15:50', 'Thursday'),
(5, 5, '08:00', '09:20', 'Friday'),
(6, 6, '09:00', '10:20', 'Monday'),
(7, 7, '11:00', '12:20', 'Tuesday'),
(8, 8, '13:00', '14:20', 'Wednesday'),
(9, 9, '14:30', '15:50', 'Thursday'),
(10, 10, '10:00', '11:20', 'Friday'),
(11, 11, '11:30', '12:50', 'Monday'),
(12, 12, '13:00', '14:20', 'Tuesday'),
(13, 13, '14:30', '15:50', 'Wednesday'),
(14, 14, '15:00', '16:20', 'Thursday'),
(15, 15, '09:00', '10:20', 'Friday'),
(16, 16, '10:30', '11:50', 'Monday'),
(17, 17, '13:00', '14:20', 'Tuesday'),
(18, 18, '14:30', '15:50', 'Wednesday'),
(19, 19, '11:00', '12:20', 'Thursday'),
(20, 20, '08:30', '09:50', 'Friday');


-- Insert dummy data into Cart with prereqs included where needed
INSERT INTO Cart (SectionID, StudentID, CourseID, CourseName)
VALUES
-- Student 1000000 takes Intro + Data Structures (prereq satisfied)
(1, 1000000, 101, 'Intro to Programming'),
(2, 1000000, 200, 'Data Structures'),

-- Student 1000001 takes Intro, Data Structures, Algorithms
(1, 1000001, 101, 'Intro to Programming'),
(2, 1000001, 200, 'Data Structures'),
(3, 1000001, 204, 'Algorithms'),

-- Student 1000002 takes Intro, Data Structures, Operating Systems (prereqs)
(1, 1000002, 101, 'Intro to Programming'),
(2, 1000002, 200, 'Data Structures'),
(5, 1000002, 360, 'Operating Systems'),

-- Student 1000003 takes Web Dev, Software Eng (prereq satisfied)
(1, 1000003, 101, 'Intro to Programming'),
(2, 1000003, 200, 'Data Structures'),
(7, 1000003, 395, 'Software Engineering'),
(8, 1000003, 315, 'Web Development'),

-- Student 1000004 takes Databases, Advanced Databases
(1, 1000004, 101, 'Intro to Programming'),
(6, 1000004, 291, 'Databases'),
(17, 1000004, 391, 'Advanced Databases'),

-- Student 1000005 takes Data Structures + Capstone (prereqs handled)
(1, 1000005, 101, 'Intro to Programming'),
(2, 1000005, 200, 'Data Structures'),
(7, 1000005, 395, 'Software Engineering'),
(20, 1000005, 496, 'Capstone Project'),

-- Student 1000006 just takes Discrete Math (no prereq)
(15, 1000006, 272, 'Discrete Math'),

-- Student 1000007 takes Intro, Data Structures, AI track
(1, 1000007, 101, 'Intro to Programming'),
(2, 1000007, 200, 'Data Structures'),
(3, 1000007, 204, 'Algorithms'),
(10, 1000007, 255, 'Machine Learning'),
(11, 1000007, 355, 'Artificial Intelligence'),

-- Student 1000008 takes Cybersecurity Fundamentals (needs Architecture)
(1, 1000008, 101, 'Intro to Programming'),
(2, 1000008, 200, 'Data Structures'),
(4, 1000008, 229, 'Computer Architecture'),
(13, 1000008, 280, 'Cybersecurity Fundamentals'),

-- Student 1000009 repeats Intro (realistic) and takes Web Dev
(1, 1000009, 101, 'Intro to Programming'),
(8, 1000009, 315, 'Web Development'),

-- Student 1000010 takes only Theory of Computation (no prereq)
(16, 1000010, 104, 'Theory of Computation'),

-- Student 1000011 takes Human-Computer Interaction (has prereq)
(1, 1000011, 101, 'Intro to Programming'),
(14, 1000011, 250, 'Human-Computer Interaction'),

-- Student 1000012 takes DevOps (prereq = Software Engineering)
(2, 1000012, 200, 'Data Structures'),
(7, 1000012, 395, 'Software Engineering'),
(19, 1000012, 497, 'DevOps Practices');
