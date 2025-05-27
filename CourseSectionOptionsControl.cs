#nullable disable
using System;
using System.Data;
using System.Drawing;
using System.Linq;
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
        private readonly string studentId = "123456"; // TODO: Replace with actual logged-in student ID

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

            string[] headers = { "Option", "Section", "Date & Times", "Room", "Instructor", "Seats", "Select" };
            int[] positions = { 10, 80, 300, 520, 640, 780, 950 };

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
        }

        private void LoadSections()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"
                    SELECT S.SectionID, S.CrseName, ST.DayOfWeek, ST.StartTime, ST.EndTime, 
                           CR.Building, CR.RoomNumber, 
                           'John Doe' AS Instructor, -- Placeholder
                           S.Capicity AS TotalSeats,
                           S.Capicity - COUNT(T.StudentID) AS AvailableSeats
                    FROM Section S
                    JOIN Sect_TimeSlot ST ON S.SectionID = ST.SectionID
                    JOIN Classroom CR ON S.ClassroomID = CR.ClassroomID
                    LEFT JOIN Takes T ON S.SectionID = T.SectionID
                    WHERE S.CourseID = @CourseID AND S.Semester = @Semester
                    GROUP BY S.SectionID, S.CrseName, ST.DayOfWeek, ST.StartTime, ST.EndTime, CR.Building, CR.RoomNumber, S.Capicity
                    ORDER BY ST.DayOfWeek, ST.StartTime;
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
                        string section = reader["CrseName"].ToString();
                        string day = reader["DayOfWeek"].ToString();
                        string start = reader["StartTime"].ToString();
                        string end = reader["EndTime"].ToString();
                        string room = reader["Building"] + "-" + reader["RoomNumber"];
                        string instructor = reader["Instructor"].ToString();
                        int totalSeats = Convert.ToInt32(reader["TotalSeats"]);
                        int availableSeats = Convert.ToInt32(reader["AvailableSeats"]);

                        RadioButton radio = new RadioButton
                        {
                            Tag = sectionId,
                            Location = new Point(950, 20),
                            AutoSize = true
                        };
                        radio.CheckedChanged += (s, e) =>
                        {
                            if (radio.Checked) selectedRadio = radio;
                        };

                        Panel row = new Panel
                        {
                            Size = new Size(1100, 60),
                            BackColor = Color.White,
                            BorderStyle = BorderStyle.FixedSingle
                        };

                        row.Controls.Add(new Label { Text = optionNum.ToString(), Location = new Point(10, 20), AutoSize = true });
                        row.Controls.Add(new Label { Text = section, Location = new Point(80, 20), Size = new Size(200, 20) });
                        row.Controls.Add(new Label { Text = day + " " + start + " to " + end, Location = new Point(300, 20), Size = new Size(200, 20) });
                        row.Controls.Add(new Label { Text = room, Location = new Point(520, 20), Size = new Size(100, 20) });
                        row.Controls.Add(new Label { Text = instructor, Location = new Point(640, 20), Size = new Size(120, 20) });
                        row.Controls.Add(new Label { Text = $"Total: {totalSeats} / Available: {availableSeats}", Location = new Point(780, 20), AutoSize = true });
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
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Takes (StudentID, SectionID) VALUES (@StudentID, @SectionID)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", studentId);
                        cmd.Parameters.AddWithValue("@SectionID", selectedSectionId);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Registration failed:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
