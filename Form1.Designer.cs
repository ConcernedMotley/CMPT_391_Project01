namespace CMPT_391_Project_01
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.leftPanel = new Panel();
            this.rightPanel = new Panel();
            this.logo = new PictureBox();
            this.secureCourseLabel = new Label();
            this.loginLabel = new Label();
            this.userLabel = new Label();
            this.userTextBox = new TextBox();
            this.passLabel = new Label();
            this.passTextBox = new TextBox();
            this.loginButton = new Button();
            this.artImage = new PictureBox();

            this.SuspendLayout();

            // Form
            this.ClientSize = new Size(1440, 1024);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.BackColor = Color.White;

            // Left Panel
            this.leftPanel.Size = new Size(720, 1024);
            this.leftPanel.Location = new Point(0, 0);
            this.leftPanel.BackColor = Color.White;

            // Logo
            this.logo.Size = new Size(64, 64);
            this.logo.Location = new Point(328, 100);
            this.logo.SizeMode = PictureBoxSizeMode.StretchImage;
            //this.logo.Image = Image.FromFile("logo.jpg");

            // SecureCourse Label
            this.secureCourseLabel.Text = "SecureCourse";
            this.secureCourseLabel.Font = new Font("Georgia", 20F, FontStyle.Bold);
            this.secureCourseLabel.ForeColor = Color.FromArgb(11, 35, 94);
            this.secureCourseLabel.AutoSize = true;
            this.secureCourseLabel.Location = new Point(290, 170);

            // Login Label
            this.loginLabel.Text = "Login";
            this.loginLabel.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            this.loginLabel.ForeColor = Color.FromArgb(11, 35, 94);
            this.loginLabel.AutoSize = true;
            this.loginLabel.Location = new Point(300, 250);

            // User Label
            this.userLabel.Text = "Username";
            this.userLabel.Font = new Font("Segoe UI", 12F);
            this.userLabel.ForeColor = Color.FromArgb(14, 44, 117);
            this.userLabel.Location = new Point(180, 330);

            // User TextBox
            this.userTextBox.Size = new Size(360, 32);
            this.userTextBox.Location = new Point(180, 360);
            this.userTextBox.Font = new Font("Segoe UI", 11F);
            this.userTextBox.BackColor = Color.FromArgb(245, 248, 250);

            // Password Label
            this.passLabel.Text = "Password";
            this.passLabel.Font = new Font("Segoe UI", 12F);
            this.passLabel.ForeColor = Color.FromArgb(14, 44, 117);
            this.passLabel.Location = new Point(180, 410);

            // Password TextBox
            this.passTextBox.Size = new Size(360, 32);
            this.passTextBox.Location = new Point(180, 440);
            this.passTextBox.Font = new Font("Segoe UI", 11F);
            this.passTextBox.BackColor = Color.FromArgb(245, 248, 250);
            this.passTextBox.UseSystemPasswordChar = true;

            // Login Button
            this.loginButton.Text = "LOGIN";
            this.loginButton.Font = new Font("Segoe UI", 14F);
            this.loginButton.ForeColor = Color.White;
            this.loginButton.BackColor = Color.FromArgb(11, 35, 94);
            this.loginButton.FlatStyle = FlatStyle.Flat;
            this.loginButton.FlatAppearance.BorderSize = 0;
            this.loginButton.Size = new Size(360, 48);
            this.loginButton.Location = new Point(180, 520);
            this.loginButton.Click += new EventHandler(this.loginButton_Click);

            // Add left controls
            this.leftPanel.Controls.Add(this.logo);
            this.leftPanel.Controls.Add(this.secureCourseLabel);
            this.leftPanel.Controls.Add(this.loginLabel);
            this.leftPanel.Controls.Add(this.userLabel);
            this.leftPanel.Controls.Add(this.userTextBox);
            this.leftPanel.Controls.Add(this.passLabel);
            this.leftPanel.Controls.Add(this.passTextBox);
            this.leftPanel.Controls.Add(this.loginButton);

            // Right Panel
            this.rightPanel.Size = new Size(720, 1024);
            this.rightPanel.Location = new Point(720, 0);
            this.rightPanel.BackColor = Color.White;

            // Art Image
            this.artImage.Size = new Size(720, 1024);
            this.artImage.Location = new Point(0, 0);
            this.artImage.SizeMode = PictureBoxSizeMode.StretchImage;
            //this.artImage.Image = Image.FromFile("abstract_art.jpg");

            this.rightPanel.Controls.Add(this.artImage);

            // Add panels to form
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.rightPanel);

            this.ResumeLayout(false);
        }

        private Panel leftPanel;
        private Panel rightPanel;
        private PictureBox logo;
        private Label secureCourseLabel;
        private Label loginLabel;
        private Label userLabel;
        private TextBox userTextBox;
        private Label passLabel;
        private TextBox passTextBox;
        private Button loginButton;
        private PictureBox artImage;

        #endregion
    }
}
