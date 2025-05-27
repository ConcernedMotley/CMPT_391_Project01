﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CMPT_391_Project_01
{
    public class CourseSearchForm : Form
    {
        private Panel sidebar;
        private PictureBox logo;
        private Button navRegistration;

        private Panel header;
        private Label titleLabel;
        private Label userNameLabel;
        private Label userRoleLabel;
        private PictureBox profilePic;
        private Label backArrow;

        private Panel mainContent;
        private Panel semesterPanel;
        private Label semesterLabel;
        private LinkLabel changeLink;
        private TextBox searchTextBox;
        private Button searchButton;
        private FlowLayoutPanel resultsPanel;

        private string selectedSemester;
        private readonly string connectionString = "Server=DESKTOP-JKB2ILV\\MSSQLSERVER01;Database=CMPT_391_P01;Trusted_Connection=True;TrustServerCertificate=True;";

        public CourseSearchForm(string semester)
        {
            this.selectedSemester = semester;
            InitializeComponent();
            RenderSearchResults("");
        }

        private void InitializeComponent()
        {
            this.ClientSize = new Size(1440, 1024);
            this.Text = "Course Search";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;

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

            navRegistration = new Button
            {
                Text = "  Registration",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(11, 35, 94),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 40),
                Location = new Point(42, 150),
                TextAlign = ContentAlignment.MiddleLeft
            };
            navRegistration.FlatAppearance.BorderSize = 0;

            sidebar.Controls.Add(logo);
            sidebar.Controls.Add(navRegistration);

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
                SemesterSelectionForm semesterForm = new SemesterSelectionForm();
                semesterForm.Show();
            };

            titleLabel = new Label
            {
                Text = "Course Registration Search Results:",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(11, 35, 94),
                Location = new Point(60, 25),
                AutoSize = true
            };

            profilePic = new PictureBox
            {
                Size = new Size(40, 40),
                Location = new Point(1076, 20),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Image.FromFile("profile.jpg")
            };

            userNameLabel = new Label
            {
                Text = "Kelvin Yeboah",
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

            header.Controls.Add(backArrow);
            header.Controls.Add(titleLabel);
            header.Controls.Add(userNameLabel);
            header.Controls.Add(userRoleLabel);
            header.Controls.Add(profilePic);

            mainContent = new Panel
            {
                Size = new Size(1176, 850),
                Location = new Point(264, 100),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

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
            changeLink.MouseEnter += (s, e) => changeLink.LinkColor = Color.LightBlue;
            changeLink.MouseLeave += (s, e) => changeLink.LinkColor = Color.White;

            semesterPanel.Controls.Add(semesterLabel);
            semesterPanel.Controls.Add(changeLink);

            searchTextBox = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Size = new Size(1041, 46),
                Location = new Point(30, 100),
                ForeColor = Color.Gray,
                Text = "Search...",
                BorderStyle = BorderStyle.FixedSingle
            };
            searchTextBox.GotFocus += (s, e) =>
            {
                if (searchTextBox.Text == "Search...")
                {
                    searchTextBox.Text = "";
                    searchTextBox.ForeColor = Color.Black;
                }
            };
            searchTextBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchTextBox.Text))
                {
                    searchTextBox.Text = "Search...";
                    searchTextBox.ForeColor = Color.Gray;
                }
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

        private void RenderSearchResults(string keyword)
        {
            resultsPanel.Controls.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("GetCourseSearchResults", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Semester", selectedSemester.Equals("fall", StringComparison.OrdinalIgnoreCase) ? "Fall" : "Winter");
                    cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string rawCourseId = reader["CourseID"].ToString();         // Save course ID for later
                        string courseLabel = reader["courseLabel"].ToString();      // Save course label for later
                        string courseName = reader["courseName"].ToString();        // Save course name for later
                        int classCount = Convert.ToInt32(reader["ClassCount"]);

                        string displayCourseId = courseLabel + " " + rawCourseId;


                        Panel card = new Panel
                        {
                            Size = new Size(1040, 80),
                            BackColor = Color.White,
                            BorderStyle = BorderStyle.FixedSingle,
                            Margin = new Padding(0, 10, 0, 0)
                        };

                        Label codeLabel = new Label
                        {
                            Text = displayCourseId,
                            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                            ForeColor = Color.FromArgb(11, 35, 94),
                            Location = new Point(16, 10),
                            AutoSize = true
                        };

                        Label titleLabel = new Label
                        {
                            Text = courseName,
                            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                            ForeColor = Color.FromArgb(11, 35, 94),
                            Location = new Point(16, 30),
                            AutoSize = true
                        };

                        Label classCountLabel = new Label
                        {
                            Text = classCount + " Class options",
                            Font = new Font("Segoe UI", 9F),
                            ForeColor = Color.Black,
                            Location = new Point(16, 55),
                            AutoSize = true
                        };

                        card.Controls.Add(codeLabel);
                        card.Controls.Add(titleLabel);
                        card.Controls.Add(classCountLabel);
                        resultsPanel.Controls.Add(card);

                        card.Click += (s, e) =>
                        {
                            mainContent.Controls.Clear();
                            mainContent.Controls.Add(new CourseSectionOptionsControl(
                                rawCourseId,
                                courseLabel,
                                courseName,
                                selectedSemester
                            ));
                        };
                    }
                }
            }
        }
    }
}
