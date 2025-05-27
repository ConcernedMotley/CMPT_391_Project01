using System;
using System.Drawing;
using System.Windows.Forms;

namespace CMPT_391_Project_01
{
    public class SemesterSelectionForm : Form
    {
        private Panel leftPanel;
        private Panel rightPanel;
        private PictureBox logo;
        private Label secureCourseLabel;
        private Label welcomeLabel;
        private Label instructionLabel;
        private Button fallButton;
        private Button winterButton;
        private PictureBox artImage;
        private Label fallArrow;
        private Label winterArrow;
        private string selectedSemester = "";




        public SemesterSelectionForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.leftPanel = new Panel();
            this.rightPanel = new Panel();
            this.logo = new PictureBox();
            this.secureCourseLabel = new Label();
            this.welcomeLabel = new Label();
            this.instructionLabel = new Label();
            this.fallButton = new Button();
            this.winterButton = new Button();
            this.artImage = new PictureBox();

            // Form settings
            this.ClientSize = new Size(1440, 1024);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Select Semester";
            this.BackColor = Color.White;

            // Left Panel
            this.leftPanel.Size = new Size(720, 1024);
            this.leftPanel.Location = new Point(0, 0);
            this.leftPanel.BackColor = Color.White;

            // Logo
            this.logo.Size = new Size(64, 64);
            this.logo.Location = new Point(328, 80);
            this.logo.SizeMode = PictureBoxSizeMode.StretchImage;
            this.logo.Image = Image.FromFile("logo.jpg");

            // SecureCourse Label
            this.secureCourseLabel.Text = "SecureCourse";
            this.secureCourseLabel.Font = new Font("Georgia", 20F, FontStyle.Bold);
            this.secureCourseLabel.ForeColor = Color.FromArgb(11, 35, 94);
            this.secureCourseLabel.AutoSize = true;
            this.secureCourseLabel.Location = new Point(290, 150);

            // Welcome Label
            this.welcomeLabel.Text = "Hello Kelvin Yeboah!";
            this.welcomeLabel.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            this.welcomeLabel.ForeColor = Color.FromArgb(11, 35, 94);
            this.welcomeLabel.AutoSize = true;
            this.welcomeLabel.Location = new Point(160, 240);

            // Instruction Label
            this.instructionLabel.Text = "Select the semester you\nwish to enrol in";
            this.instructionLabel.Font = new Font("Segoe UI", 14F);
            this.instructionLabel.ForeColor = Color.FromArgb(11, 35, 94);
            this.instructionLabel.AutoSize = true;
            this.instructionLabel.Location = new Point(160, 310);

            // Fall Button
            this.fallButton.Text = "FALL\n2025–2026 academic year";
            this.fallButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.fallButton.ForeColor = Color.FromArgb(11, 35, 94);
            this.fallButton.BackColor = Color.FromArgb(245, 248, 250);
            this.fallButton.FlatStyle = FlatStyle.Flat;
            this.fallButton.FlatAppearance.BorderSize = 0;
            this.fallButton.Size = new Size(360, 80);
            this.fallButton.Location = new Point(160, 400);
            this.fallButton.TextAlign = ContentAlignment.MiddleLeft;
            this.fallButton.Click += new EventHandler(FallButton_Click);

            // Arrow inside Fall Button
            this.fallArrow = new Label();
            fallArrow.Text = ">";
            fallArrow.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            fallArrow.ForeColor = Color.FromArgb(11, 35, 94);
            fallArrow.BackColor = Color.Transparent;
            fallArrow.AutoSize = true;
            fallArrow.Location = new Point(this.fallButton.Width - 40, 25);
            fallArrow.Click += new EventHandler(FallButton_Click);
            this.fallButton.Controls.Add(fallArrow);

            // On hover effect for Fall
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

            // Winter Button
            this.winterButton.Text = "WINTER\n2025–2026 academic year";
            this.winterButton.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.winterButton.ForeColor = Color.FromArgb(11, 35, 94);
            this.winterButton.BackColor = Color.FromArgb(245, 248, 250);
            this.winterButton.FlatStyle = FlatStyle.Flat;
            this.winterButton.FlatAppearance.BorderSize = 0;
            this.winterButton.Size = new Size(360, 80);
            this.winterButton.Location = new Point(160, 500);
            this.winterButton.TextAlign = ContentAlignment.MiddleLeft;
            this.winterButton.Click += new EventHandler(WinterButton_Click);

            // Arrow inside Winter Button
            this.winterArrow = new Label();
            winterArrow.Text = ">";
            winterArrow.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            winterArrow.ForeColor = Color.FromArgb(11, 35, 94);
            winterArrow.BackColor = Color.Transparent;
            winterArrow.AutoSize = true;
            winterArrow.Location = new Point(this.winterButton.Width - 40, 25);
            winterArrow.Click += new EventHandler(WinterButton_Click);
            this.winterButton.Controls.Add(winterArrow);

            // On hover effect for winter
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


            // Add left controls
            this.leftPanel.Controls.Add(this.logo);
            this.leftPanel.Controls.Add(this.secureCourseLabel);
            this.leftPanel.Controls.Add(this.welcomeLabel);
            this.leftPanel.Controls.Add(this.instructionLabel);
            this.leftPanel.Controls.Add(this.fallButton);
            this.leftPanel.Controls.Add(this.winterButton);

            // Right Panel (Image)
            this.rightPanel.Size = new Size(720, 1024);
            this.rightPanel.Location = new Point(720, 0);
            this.rightPanel.BackColor = Color.White;

            // Art Image
            this.artImage.Size = new Size(720, 1024);
            this.artImage.Location = new Point(0, 0);
            this.artImage.SizeMode = PictureBoxSizeMode.StretchImage;
            this.artImage.Image = Image.FromFile("abstract_art.jpg");

            this.rightPanel.Controls.Add(this.artImage);

            // Add panels to form
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.rightPanel);
        }

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
            CourseSearchForm courseSearch = new CourseSearchForm("fall");
            courseSearch.Show();
        }

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
            CourseSearchForm courseSearch = new CourseSearchForm("winter");
            courseSearch.Show();
        }

    }
}
