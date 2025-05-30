using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace CMPT_391_Project_01
{
    /// <summary>
    /// Form for selecting the semester (Fall or Winter) after login.
    /// Displays a personalized greeting, semester options, and allows logout.
    /// </summary>
    public class SemesterSelectionForm : Form
    {
        // UI components
        private Panel leftPanel = null!;
        private Panel rightPanel = null!;
        private PictureBox logo = null!;
        private Label secureCourseLabel = null!;
        private Label welcomeLabel = null!;
        private Label instructionLabel = null!;
        private Button fallButton = null!;
        private Button winterButton = null!;
        private PictureBox artImage = null!;
        private Label fallArrow = null!;
        private Label winterArrow = null!;

        // State tracking
        private string selectedSemester = "";
        private string studentId = null!;

        /// <summary>
        /// Constructor that initializes the form and reads the current student ID from session.
        /// </summary>
        public SemesterSelectionForm()
        {
            studentId = Session.StudentID ?? "";
            InitializeComponent();
        }

        /// <summary>
        /// Initializes all UI elements, layout, and styles for the form.
        /// </summary>
        private void InitializeComponent()
        {
            this.ClientSize = new Size(1440, 1024);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Select Semester";
            this.BackColor = Color.White;

            // ========== LEFT PANEL ==========
            leftPanel = new Panel
            {
                Size = new Size(720, 1024),
                Location = new Point(0, 0),
                BackColor = Color.White
            };

            logo = new PictureBox
            {
                Size = new Size(64, 64),
                Location = new Point(328, 80),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Image.FromFile("logo.jpg")
            };

            secureCourseLabel = new Label
            {
                Text = "SecureCourse",
                Font = new Font("Georgia", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(11, 35, 94),
                AutoSize = true,
                Location = new Point(290, 150)
            };

            welcomeLabel = new Label
            {
                Text = $"Hello {GetStudentName(studentId)}!",
                Font = new Font("Segoe UI", 26F, FontStyle.Bold),
                ForeColor = Color.FromArgb(11, 35, 94),
                AutoSize = true,
                Location = new Point(160, 240)
            };

            instructionLabel = new Label
            {
                Text = "Select the semester you\nwish to enrol in",
                Font = new Font("Segoe UI", 14F),
                ForeColor = Color.FromArgb(11, 35, 94),
                AutoSize = true,
                Location = new Point(160, 310)
            };

            // ========== FALL BUTTON ==========
            fallButton = new Button
            {
                Text = "FALL\n2024–2025 academic year",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(11, 35, 94),
                BackColor = Color.FromArgb(245, 248, 250),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(360, 80),
                Location = new Point(160, 400),
                TextAlign = ContentAlignment.MiddleLeft
            };
            fallButton.FlatAppearance.BorderSize = 0;
            fallButton.Click += FallButton_Click;

            fallArrow = new Label
            {
                Text = ">",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(11, 35, 94),
                Location = new Point(fallButton.Width - 40, 25),
                AutoSize = true
            };
            fallArrow.Click += FallButton_Click;
            fallButton.Controls.Add(fallArrow);

            fallButton.MouseEnter += (s, e) =>
            {
                if (fallButton.ForeColor != Color.White)
                {
                    fallButton.BackColor = Color.FromArgb(11, 35, 94);
                    fallButton.ForeColor = Color.White;
                    fallArrow.ForeColor = Color.White;
                }
            };
            fallButton.MouseLeave += (s, e) =>
            {
                if (selectedSemester != "fall")
                {
                    fallButton.BackColor = Color.FromArgb(245, 248, 250);
                    fallButton.ForeColor = Color.FromArgb(11, 35, 94);
                    fallArrow.ForeColor = Color.FromArgb(11, 35, 94);
                }
            };

            // ========== WINTER BUTTON ==========
            winterButton = new Button
            {
                Text = "WINTER\n2025 academic year",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(11, 35, 94),
                BackColor = Color.FromArgb(245, 248, 250),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(360, 80),
                Location = new Point(160, 500),
                TextAlign = ContentAlignment.MiddleLeft
            };
            winterButton.FlatAppearance.BorderSize = 0;
            winterButton.Click += WinterButton_Click;

            winterArrow = new Label
            {
                Text = ">",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(11, 35, 94),
                Location = new Point(winterButton.Width - 40, 25),
                AutoSize = true
            };
            winterArrow.Click += WinterButton_Click;
            winterButton.Controls.Add(winterArrow);

            winterButton.MouseEnter += (s, e) =>
            {
                if (winterButton.ForeColor != Color.White)
                {
                    winterButton.BackColor = Color.FromArgb(11, 35, 94);
                    winterButton.ForeColor = Color.White;
                    winterArrow.ForeColor = Color.White;
                }
            };
            winterButton.MouseLeave += (s, e) =>
            {
                if (selectedSemester != "winter")
                {
                    winterButton.BackColor = Color.FromArgb(245, 248, 250);
                    winterButton.ForeColor = Color.FromArgb(11, 35, 94);
                    winterArrow.ForeColor = Color.FromArgb(11, 35, 94);
                }
            };

            // ========== LOGOUT BUTTON ==========
            Button logoutButton = new Button
            {
                Text = "LOGOUT",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(11, 35, 94),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 50),
                Location = new Point(160, 600)
            };
            logoutButton.FlatAppearance.BorderSize = 0;
            logoutButton.Click += LogoutButton_Click;

            // Add all elements to left panel
            leftPanel.Controls.Add(logo);
            leftPanel.Controls.Add(secureCourseLabel);
            leftPanel.Controls.Add(welcomeLabel);
            leftPanel.Controls.Add(instructionLabel);
            leftPanel.Controls.Add(fallButton);
            leftPanel.Controls.Add(winterButton);
            leftPanel.Controls.Add(logoutButton);

            // ========== RIGHT PANEL ==========
            rightPanel = new Panel
            {
                Size = new Size(720, 1024),
                Location = new Point(720, 0),
                BackColor = Color.White
            };

            artImage = new PictureBox
            {
                Size = new Size(720, 1024),
                Location = new Point(0, 0),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Image.FromFile("abstract_art.jpg")
            };
            rightPanel.Controls.Add(artImage);

            // Add both panels to the form
            this.Controls.Add(leftPanel);
            this.Controls.Add(rightPanel);
        }

        /// <summary>
        /// Retrieves the full name of the student for personalized greeting.
        /// </summary>
        private string GetStudentName(string studentId)
        {
            string name = "Student";
            try
            {
                using var conn = new SqlConnection("Server=DESKTOP-JKB2ILV\\MSSQLSERVER01;Database=CMPT_391_P01;Trusted_Connection=True;TrustServerCertificate=True;");
                conn.Open();
                using var cmd = new SqlCommand("GetStudentFullName", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                var result = cmd.ExecuteScalar();
                if (result is not null && result != DBNull.Value)
                {
                    name = result.ToString()!;
                }
            }
            catch
            {
                name = "Student"; // fallback
            }

            return name;
        }

        /// <summary>
        /// Event handler for Fall button. Loads Fall semester courses.
        /// </summary>
        private void FallButton_Click(object? sender, EventArgs e)
        {
            selectedSemester = "fall";
            fallButton.BackColor = Color.FromArgb(11, 35, 94);
            fallButton.ForeColor = Color.White;
            fallArrow.ForeColor = Color.White;

            winterButton.BackColor = Color.FromArgb(245, 248, 250);
            winterButton.ForeColor = Color.FromArgb(11, 35, 94);
            winterArrow.ForeColor = Color.FromArgb(11, 35, 94);

            this.Hide();
            var courseSearch = new CourseSearchForm("fall", int.Parse(studentId));
            courseSearch.Show();
        }

        /// <summary>
        /// Event handler for Winter button. Loads Winter semester courses.
        /// </summary>
        private void WinterButton_Click(object? sender, EventArgs e)
        {
            selectedSemester = "winter";
            winterButton.BackColor = Color.FromArgb(11, 35, 94);
            winterButton.ForeColor = Color.White;
            winterArrow.ForeColor = Color.White;

            fallButton.BackColor = Color.FromArgb(245, 248, 250);
            fallButton.ForeColor = Color.FromArgb(11, 35, 94);
            fallArrow.ForeColor = Color.FromArgb(11, 35, 94);

            this.Hide();
            var courseSearch = new CourseSearchForm("winter", int.Parse(studentId));
            courseSearch.Show();
        }

        /// <summary>
        /// Logs out the user and returns to the login screen.
        /// </summary>
        private void LogoutButton_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Session.StudentID = null;
                var loginForm = new Form1();
                loginForm.Show();
                this.Close();
            }
        }
    }
}
