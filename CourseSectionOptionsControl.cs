#nullable disable
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace CMPT_391_Project_01
{
    public class CourseSectionOptionsControl : UserControl
    {
        private readonly string courseId;
        private readonly string courseLabel;
        private readonly string courseName;
        private readonly string semester;
        private readonly string studentId = Session.StudentID;

        private readonly string connectionString = "Server=DESKTOP-JKB2ILV\\MSSQLSERVER01;Database=CMPT_391_P01;Trusted_Connection=True;TrustServerCertificate=True;";

        private Panel headerPanel;
        private Label titleLabel;
        private Label courseInfoLabel;
        private FlowLayoutPanel sectionListPanel;
        private Button registerButton;

        private RadioButton selectedRadio = null;

        public CourseSectionOptionsControl(string courseId, string courseLabel, string courseName, string semester)
        {
            this.courseId = courseId;
            this.courseLabel = courseLabel;
            this.courseName = courseName;
            this.semester = semester;

            this.Size = new Size(1176, 850);
            this.BackColor = Color.White;
            InitializeUI();
            LoadSections();
        }

        private void InitializeUI()
        {
            headerPanel = new Panel
            {
                Size = new Size(1144, 100),
                Location = new Point(16, 16),
                BackColor = Color.White
            };

            titleLabel = new Label
            {
                Text = "Course Registration",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(11, 35, 94),
                Location = new Point(0, 0),
                AutoSize = true
            };

            courseInfoLabel = new Label
            {
                Text = semester.ToUpper() + " SEMESTER\n" + courseLabel + " " + courseId + " - " + courseName,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(0, 40),
                AutoSize = true
            };

            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(courseInfoLabel);
            this.Controls.Add(headerPanel);

            // Column header row
            Panel headerRow = new Panel
            {
                Size = new Size(1100, 40),
                BackColor = Color.LightGray,
                Location = new Point(16, 120)
            };

            string[] headers = { "Option", "Section", "Date & Times", "Room", "Building", "Instructor", "Seats", "Select" };
            int[] positions = { 10, 60, 150, 370, 460, 590, 720, 900 };

            for (int i = 0; i < headers.Length; i++)
            {
                headerRow.Controls.Add(new Label
                {
                    Text = headers[i],
                    Location = new Point(positions[i], 10),
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    AutoSize = true
                });
            }

            this.Controls.Add(headerRow);

            sectionListPanel = new FlowLayoutPanel
            {
                Location = new Point(16, 160),
                Size = new Size(1144, 620),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            this.Controls.Add(sectionListPanel);

            registerButton = new Button
            {
                Text = "Register",
                Size = new Size(120, 40),
                Location = new Point(1040, 790),
                BackColor = Color.FromArgb(11, 35, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            registerButton.Click += RegisterButton_Click;
            this.Controls.Add(registerButton);

            Button backButton = new Button
            {
                Text = "<",
                Size = new Size(120, 40),
                Location = new Point(900, 790),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            backButton.Click += (s, e) =>
            {
                Form parentForm = this.FindForm();
                parentForm.Hide();
                new CourseSearchForm(semester).Show();
            };


            this.Controls.Add(backButton);

        }

        private void LoadSections()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"
            SELECT SectionID, CrseName, DayOfWeek, StartTime, EndTime,
                           Building, RoomNumber, InstructorName, TotalSeats, EnrolledStudents
                    FROM vw_SectionAvailability
                    WHERE CourseID = @CourseID AND Semester = @Semester
                    ORDER BY DayOfWeek, StartTime;
        ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseId);
                    cmd.Parameters.AddWithValue("@Semester", semester);

                    SqlDataReader reader = cmd.ExecuteReader();
                    int optionNum = 1;

                    while (reader.Read())
                    {
                        string sectionId = reader["SectionID"].ToString();
                        string section = reader["SectionID"].ToString();
                        string day = reader["DayOfWeek"].ToString();
                        string start = DateTime.Parse(reader["StartTime"].ToString()).ToString("h:mm tt");
                        string end = DateTime.Parse(reader["EndTime"].ToString()).ToString("h:mm tt");
                        string roomNumber = reader["RoomNumber"].ToString().Trim();
                        string building = reader["Building"].ToString();
                        string instructor = reader["InstructorName"].ToString();

                        int totalSeats = Convert.ToInt32(reader["TotalSeats"]);
                        int enrolled = Convert.ToInt32(reader["EnrolledStudents"]);
                        int availableSeats = totalSeats - enrolled;

                        RadioButton radio = new RadioButton
                        {
                            Tag = sectionId,
                            Location = new Point(950, 20),
                            AutoSize = true,
                            Enabled = availableSeats > 0
                        };
                        radio.CheckedChanged += (s, e) =>
                        {
                            var currentRadio = (RadioButton)s;

                            if (currentRadio.Checked)
                            {
                                if (selectedRadio != null && selectedRadio != currentRadio)
                                    selectedRadio.Checked = false;

                                selectedRadio = currentRadio;
                            }
                        };


                        Panel row = new Panel
                        {
                            Size = new Size(1100, 60),
                            BackColor = availableSeats == 0 ? Color.LightGray : Color.White,
                            BorderStyle = BorderStyle.FixedSingle
                        };

                        row.Controls.Add(new Label { Text = optionNum.ToString(), Location = new Point(10, 20), Size = new Size(40, 20) });
                        row.Controls.Add(new Label { Text = $"Section {sectionId}", Location = new Point(60, 20), Size = new Size(80, 20) });
                        row.Controls.Add(new Label { Text = $"{day} {start} to {end}", Location = new Point(150, 20), Size = new Size(210, 20) });
                        row.Controls.Add(new Label { Text = roomNumber, Location = new Point(370, 20), Size = new Size(80, 20) });
                        row.Controls.Add(new Label { Text = building, Location = new Point(460, 20), Size = new Size(120, 20) });
                        row.Controls.Add(new Label { Text = instructor, Location = new Point(590, 20), Size = new Size(120, 20) });
                        row.Controls.Add(new Label { Text = $"Total: {totalSeats} / Available: {availableSeats}", Location = new Point(720, 20), Size = new Size(160, 20) });
                        radio.Location = new Point(900, 20);
                        row.Controls.Add(radio);


                        sectionListPanel.Controls.Add(row);
                        optionNum++;
                    }
                }
            }
        }



        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if (selectedRadio == null)
            {
                MessageBox.Show("Please select a section before registering.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedSectionId = selectedRadio.Tag.ToString();

            DialogResult confirm = MessageBox.Show(
                $"Are you sure you want to register for section {selectedSectionId}?",
                "Confirm Registration",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("dbo.RegisterStudentToSection", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@StudentID", Convert.ToInt64(studentId));
                            cmd.Parameters.AddWithValue("@SectionID", Convert.ToInt32(selectedSectionId));
                            cmd.Parameters.AddWithValue("@CourseID", Convert.ToInt32(courseId));
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Redirect to CourseSearchForm
                    Form parentForm = this.FindForm();
                    parentForm.Hide();
                    new CourseSearchForm(semester).Show();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
