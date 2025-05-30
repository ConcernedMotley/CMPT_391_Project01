using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CMPT_391_Project_01
{
    /// <summary>
    /// Represents the course search UI form where students can search and view available courses for a given semester.
    /// </summary>
    public class CourseSearchForm : Form
    {
        // UI components
        private Panel? sidebar;
        private PictureBox? logo;

        private Panel? header;
        private Label? titleLabel;
        private Label? userNameLabel;
        private Label? userRoleLabel;
        private PictureBox? profilePic;
        private Label? backArrow;

        private Panel mainContent = null!;
        private Panel? semesterPanel;
        private Label? semesterLabel;
        private LinkLabel? changeLink;
        private TextBox searchTextBox = null!;
        private Button? searchButton;
        private FlowLayoutPanel resultsPanel = null!;

        // Session & DB
        private string selectedSemester;
        private readonly string connectionString = "Server=DESKTOP-JKB2ILV\\MSSQLSERVER01;Database=CMPT_391_P01;Trusted_Connection=True;TrustServerCertificate=True;";
        private readonly string studentId = null!;
        private readonly Form caller = null!;

        /// <summary>
        /// Initializes the CourseSearchForm with semester and student ID context.
        /// </summary>
        public CourseSearchForm(string semester, int studentId)
        {
            this.selectedSemester = semester;
            this.studentId = studentId.ToString();

            InitializeComponent();
            RenderSearchResults(""); // Load default search results
        }

        /// <summary>
        /// Gets the full name of the student using a stored procedure.
        /// </summary>
        private string GetStudentName(string studentId)
        {
            string name = "Student";
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();
                using var cmd = new SqlCommand("GetStudentFullName", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                var result = cmd.ExecuteScalar();
                if (result is string fullName)
                    name = fullName;
            }
            catch
            {
                name = "Student";
            }
            return name;
        }

        /// <summary>
        /// Displays the "View Registered Classes" form when button is clicked.
        /// </summary>
        private void viewClassesButton_Click(object? sender, EventArgs e)
        {
            ViewRegisteredClassesForm form = new ViewRegisteredClassesForm(Convert.ToInt32(Session.StudentID));
            form.ShowDialog();
        }

        /// <summary>
        /// Initializes all UI components on the form.
        /// </summary>
        private void InitializeComponent()
        {
            // General form setup
            this.ClientSize = new Size(1440, 1024);
            this.Text = "Course Search";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Sidebar
            sidebar = new Panel
            {
                Size = new Size(264, 1024),
                Location = new Point(0, 0),
                BackColor = ColorTranslator.FromHtml("#F2F4FA"),
                BorderStyle = BorderStyle.FixedSingle
            };

            logo = new PictureBox
            {
                Size = new Size(64, 64),
                Location = new Point(100, 50),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Image.FromFile("logo.jpg")
            };

            // Sidebar Buttons
            Button viewClassesButton = new Button
            {
                Text = "  View My Classes",
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(11, 35, 94),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 40),
                Location = new Point(42, 150),
                TextAlign = ContentAlignment.MiddleLeft
            };
            viewClassesButton.FlatAppearance.BorderSize = 0;
            viewClassesButton.Click += (s, e) =>
            {
                var viewForm = new ViewRegisteredClassesForm(int.Parse(studentId));
                viewForm.ShowDialog();
            };

            Button courseHistoryButton = new Button
            {
                Text = "  Course History",
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(11, 35, 94),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 40),
                Location = new Point(42, 200),
                TextAlign = ContentAlignment.MiddleLeft
            };
            courseHistoryButton.FlatAppearance.BorderSize = 0;
            courseHistoryButton.Click += (s, e) =>
            {
                var historyForm = new CourseHistoryForm(int.Parse(studentId));
                historyForm.ShowDialog();
            };

            sidebar.Controls.Add(viewClassesButton);
            sidebar.Controls.Add(courseHistoryButton);
            sidebar.Controls.Add(logo);

            Button shoppingCartButton = new Button
            {
                Text = "  My Shopping Cart",
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(11, 35, 94),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 40),
                Location = new Point(42, 250),
                TextAlign = ContentAlignment.MiddleLeft
            };
            shoppingCartButton.FlatAppearance.BorderSize = 0;
            shoppingCartButton.Click += (s, e) =>
            {
                var cartForm = new ShoppingCartForm(int.Parse(studentId));
                cartForm.ShowDialog();

            };

            sidebar.Controls.Add(shoppingCartButton);

            // Header Panel
            header = new Panel
            {
                Size = new Size(1176, 80),
                Location = new Point(264, 0),
                BackColor = Color.White
            };

            backArrow = new Label
            {
                Text = "<",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                Location = new Point(10, 25),
                AutoSize = true,
                ForeColor = Color.FromArgb(11, 35, 94),
                Cursor = Cursors.Hand
            };
            backArrow.Click += (s, e) =>
            {
                this.Hide();
                if (caller != null) caller.Show();
                else new SemesterSelectionForm().Show();
            };

            titleLabel = new Label
            {
                Text = "Course Registration Search Results:",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(11, 35, 94),
                Location = new Point(60, 25),
                AutoSize = true
            };

            userNameLabel = new Label
            {
                Text = $"{GetStudentName(studentId)}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(920, 20),
                AutoSize = true
            };

            userRoleLabel = new Label
            {
                Text = "Student",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(920, 40),
                AutoSize = true
            };

            profilePic = new PictureBox
            {
                Size = new Size(40, 40),
                Location = new Point(1076, 20),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Image.FromFile("profile.jpg")
            };

            header.Controls.Add(backArrow);
            header.Controls.Add(titleLabel);
            header.Controls.Add(userNameLabel);
            header.Controls.Add(userRoleLabel);
            header.Controls.Add(profilePic);

            // Main content panel
            mainContent = new Panel
            {
                Size = new Size(1176, 850),
                Location = new Point(264, 100),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Semester Header Bar
            semesterPanel = new Panel
            {
                Size = new Size(1112, 56),
                Location = new Point(32, 30),
                BackColor = Color.FromArgb(11, 35, 94),
                BorderStyle = BorderStyle.FixedSingle
            };

            semesterLabel = new Label
            {
                Text = selectedSemester.ToUpper() + " SEMESTER",
                Font = new Font("Poppins", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(16, 14),
                AutoSize = true
            };

            changeLink = new LinkLabel
            {
                Text = "change",
                LinkColor = Color.White,
                Font = new Font("Libre Bodoni", 18F, FontStyle.Italic),
                Location = new Point(990, 17),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            changeLink.Click += (s, e) =>
            {
                selectedSemester = selectedSemester.ToLower() == "fall" ? "winter" : "fall";
                semesterLabel.Text = selectedSemester.ToUpper() + " SEMESTER";
                RenderSearchResults(searchTextBox.Text);
            };

            semesterPanel.Controls.Add(semesterLabel);
            semesterPanel.Controls.Add(changeLink);

            // Search Input and Button
            searchTextBox = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Size = new Size(1041, 46),
                Location = new Point(30, 100),
                ForeColor = Color.Gray,
                Text = "Search..."
            };
            searchTextBox.GotFocus += (s, e) => {
                if (searchTextBox.Text == "Search...") searchTextBox.Text = "";
            };
            searchTextBox.LostFocus += (s, e) => {
                if (string.IsNullOrWhiteSpace(searchTextBox.Text)) searchTextBox.Text = "Search...";
            };

            searchButton = new Button
            {
                Size = new Size(26, 26),
                Location = new Point(1074, 100),
                BackColor = Color.FromArgb(217, 217, 217),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                Text = "🔍"
            };
            searchButton.Click += (s, e) => RenderSearchResults(searchTextBox.Text);

            // Results panel for displaying search matches
            resultsPanel = new FlowLayoutPanel
            {
                Location = new Point(30, 160),
                Size = new Size(1100, 650),
                AutoScroll = true,
                BackColor = Color.White
            };

            mainContent.Controls.Add(semesterPanel);
            mainContent.Controls.Add(searchTextBox);
            mainContent.Controls.Add(searchButton);
            mainContent.Controls.Add(resultsPanel);

            this.Controls.Add(sidebar);
            this.Controls.Add(header);
            this.Controls.Add(mainContent);
        }

        /// <summary>
        /// Executes the course search and displays matching results in card format.
        /// </summary>
        private void RenderSearchResults(string keyword)
        {
            resultsPanel.Controls.Clear();

            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using SqlCommand cmd = new SqlCommand("GetCourseSearchResults", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Semester", selectedSemester);
            cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string rawCourseId = reader["CourseID"].ToString()!;
                string courseLabel = reader["courseLabel"].ToString()!;
                string courseName = reader["courseName"].ToString()!;
                int classCount = Convert.ToInt32(reader["ClassCount"]);

                string displayCourseId = courseLabel + " " + rawCourseId;

                // Build UI Card for each course
                Panel card = new Panel
                {
                    Size = new Size(1040, 80),
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(0, 10, 0, 0)
                };

                card.Controls.Add(new Label
                {
                    Text = displayCourseId,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(11, 35, 94),
                    Location = new Point(16, 10),
                    AutoSize = true
                });

                card.Controls.Add(new Label
                {
                    Text = courseName,
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(11, 35, 94),
                    Location = new Point(16, 30),
                    AutoSize = true
                });

                card.Controls.Add(new Label
                {
                    Text = classCount + " Class options",
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.Black,
                    Location = new Point(16, 55),
                    AutoSize = true
                });

                card.Click += (s, e) =>
                {
                    var sectionControl = new CourseSectionOptionsControl(rawCourseId, courseLabel, courseName, selectedSemester);

                    sectionControl.BackRequested += (sender, args) =>
                    {
                        mainContent.Controls.Clear();
                        mainContent.Controls.Add(semesterPanel);
                        mainContent.Controls.Add(searchTextBox);
                        mainContent.Controls.Add(searchButton);
                        mainContent.Controls.Add(resultsPanel);
                    };

                    mainContent.Controls.Clear();
                    mainContent.Controls.Add(sectionControl);
                };

                resultsPanel.Controls.Add(card);
            }
        }
    }
}
