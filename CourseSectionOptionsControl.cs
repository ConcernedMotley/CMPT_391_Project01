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
    /// <summary>
    /// A UserControl for displaying section options of a course during registration.
    /// Allows a student to view, select, and register for available course sections.
    /// Also shows prerequisite information and handles UI interactions.
    /// </summary>
    public class CourseSectionOptionsControl : UserControl
    {
        // Core course and session information
        private readonly string courseId;
        private readonly string courseLabel;
        private readonly string courseName;
        private readonly string semester;
        private readonly string studentId = Session.StudentID;

        // SQL connection string
        private readonly string connectionString = "Server=DESKTOP-JKB2ILV\\MSSQLSERVER01;Database=CMPT_391_P01;Trusted_Connection=True;TrustServerCertificate=True;";

        // UI components
        private Panel headerPanel;
        private Label titleLabel;
        private Label courseInfoLabel;
        private FlowLayoutPanel sectionListPanel;
        private Button registerButton;
        private Label prereqLabel;
        private LinkLabel togglePrereqLink;
        private bool prereqExpanded = false;
        public event EventHandler BackRequested;

        private RadioButton selectedRadio = null;

        /// <summary>
        /// Constructor for the section options control.
        /// </summary>
        public CourseSectionOptionsControl(string courseId, string label, string name, string semester)
        {
            this.courseId = courseId;
            this.courseLabel = label;
            this.courseName = name;
            this.semester = semester;

            this.Size = new Size(1176, 850);
            this.BackColor = Color.White;
            InitializeUI();
            LoadPrerequisite(prereqLabel);
            LoadSections();
        }

        /// <summary>
        /// Initializes all UI components and layout.
        /// </summary>
        private void InitializeUI()
        {
            // Header panel
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

            togglePrereqLink = new LinkLabel
            {
                Text = "Show Prerequisites",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                LinkColor = Color.Blue,
                Location = new Point(900, 45),
                AutoSize = true
            };
            togglePrereqLink.Click += TogglePrerequisite;

            prereqLabel = new Label
            {
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Black,
                Location = new Point(0, 80),
                Size = new Size(1100, 20),
                Visible = false
            };

            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(courseInfoLabel);
            headerPanel.Controls.Add(togglePrereqLink);
            headerPanel.Controls.Add(prereqLabel);
            this.Controls.Add(headerPanel);

            // Column headers
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

            // Scrollable section list
            sectionListPanel = new FlowLayoutPanel
            {
                Location = new Point(16, 160),
                Size = new Size(1144, 620),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            this.Controls.Add(sectionListPanel);

            // Add to Cart button
            registerButton = new Button
            {
                Text = "Add to Cart",
                Size = new Size(120, 40),
                Location = new Point(1040, 790),
                BackColor = Color.FromArgb(11, 35, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            registerButton.Click += AddToCartButton_Click;
            this.Controls.Add(registerButton);


            // Back button
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
            backButton.Click += (s, e) => BackRequested?.Invoke(this, EventArgs.Empty);
            this.Controls.Add(backButton);
        }

        /// <summary>
        /// Expands or collapses the prerequisite information section.
        /// </summary>
        private void TogglePrerequisite(object sender, EventArgs e)
        {
            prereqExpanded = !prereqExpanded;
            prereqLabel.Visible = prereqExpanded;
            togglePrereqLink.Text = prereqExpanded ? "Hide Prerequisites" : "Show Prerequisites";
        }

        /// <summary>
        /// Loads and displays all available sections for the course.
        /// </summary>
        private void LoadSections()
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using SqlCommand cmd = new SqlCommand("dbo.GetCourseSections", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@CourseID", courseId);
            cmd.Parameters.AddWithValue("@Semester", semester);

            using SqlDataReader reader = cmd.ExecuteReader();
            int optionNum = 1;

            while (reader.Read())
            {
                string sectionId = reader["SectionID"].ToString();
                string day = reader["DayOfWeek"].ToString();
                string start = DateTime.Parse(reader["StartTime"].ToString()).ToString("h:mm tt");
                string end = DateTime.Parse(reader["EndTime"].ToString()).ToString("h:mm tt");
                string roomNumber = reader["RoomNumber"].ToString().Trim();
                string building = reader["Building"].ToString();
                string instructor = reader["InstructorName"].ToString();

                int totalSeats = Convert.ToInt32(reader["TotalSeats"]);
                int enrolled = Convert.ToInt32(reader["EnrolledStudents"]);
                int availableSeats = totalSeats - enrolled;

                // Radio button to select this section
                RadioButton radio = new RadioButton
                {
                    Tag = sectionId,
                    Location = new Point(950, 20),
                    AutoSize = true,
                    Enabled = availableSeats > 0
                };
                radio.CheckedChanged += (s, e) =>
                {
                    if (radio.Checked)
                    {
                        if (selectedRadio != null)
                        {
                            selectedRadio.Checked = false;
                        }
                        selectedRadio = radio;
                    }
                };


                // UI row with section info
                Panel row = new Panel
                {
                    Size = new Size(1100, 60),
                    BackColor = availableSeats == 0 ? Color.LightGray : Color.White,
                    BorderStyle = BorderStyle.FixedSingle
                };

                // Ensure labels are wide enough and aligned correctly
                row.Controls.Add(new Label { Text = optionNum.ToString(), Location = new Point(10, 20), AutoSize = true, Width = 40 });
                row.Controls.Add(new Label { Text = $"Section {sectionId}", Location = new Point(60, 20), AutoSize = true, Width = 80 });
                row.Controls.Add(new Label { Text = $"{day} {start}–{end}", Location = new Point(150, 20), AutoSize = true, Width = 200 });
                row.Controls.Add(new Label { Text = roomNumber, Location = new Point(370, 20), AutoSize = true, Width = 80 });
                row.Controls.Add(new Label { Text = building, Location = new Point(460, 20), AutoSize = true, Width = 120 });
                row.Controls.Add(new Label { Text = instructor, Location = new Point(590, 20), AutoSize = true, Width = 120 });
                row.Controls.Add(new Label
                {
                    Text = $"Total: {totalSeats} / Available: {availableSeats}",
                    Location = new Point(720, 20),
                    AutoSize = true,
                    Width = 170
                });
                radio.Location = new Point(980, 20);
                row.Controls.Add(radio);
                ;

                sectionListPanel.Controls.Add(row);
                optionNum++;
            }
        }

        /// <summary>
        /// Loads prerequisite information using a stored procedure.
        /// </summary>
        private void LoadPrerequisite(Label label)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using SqlCommand cmd = new SqlCommand("GetFormattedCoursePrerequisites", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@CourseID", courseId);

            using SqlDataReader reader = cmd.ExecuteReader();
            var prereqs = new System.Collections.Generic.List<string>();

            while (reader.Read())
            {
                prereqs.Add(reader["FormattedCourse"].ToString());
            }

            if (prereqs.Count > 0)
            {
                label.Text = "Prerequisites: " + string.Join(", ", prereqs);
                togglePrereqLink.Visible = true;
            }
            else
            {
                label.Text = "Prerequisites: None";
                togglePrereqLink.Visible = false;
            }
        }

        /// <summary>
        /// Adds the selected section to the student's shopping cart.
        /// </summary>
        private void AddToCartButton_Click(object sender, EventArgs e)
        {
            if (selectedRadio == null)
            {
                MessageBox.Show("Please select a section before adding to cart.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedSectionId = selectedRadio.Tag.ToString();

            DialogResult confirm = MessageBox.Show(
                $"Do you want to add section {selectedSectionId} to your cart?",
                "Add to Cart",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();

                    using SqlCommand cmd = new SqlCommand("sp_AddToCart", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@StudentID", Convert.ToInt64(studentId));
                    cmd.Parameters.AddWithValue("@SectionID", Convert.ToInt32(selectedSectionId));
                    cmd.Parameters.AddWithValue("@CourseID", Convert.ToInt32(courseId));
                    cmd.Parameters.AddWithValue("@Semester", semester); // e.g., "Fall"
                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Course added to cart successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("duplicate"))
                    {
                        MessageBox.Show("This section is already in your cart.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add to cart.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}

