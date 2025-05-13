namespace CMPT_391_Project_01
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            loginLabel = new Label();
            userNameLogin = new Label();
            userNameTextBox = new TextBox();
            passwordLabel = new Label();
            passwordTextBox = new TextBox();
            loginButton = new Button();
            SuspendLayout();
            // 
            // loginLabel
            // 
            loginLabel.AutoSize = true;
            loginLabel.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            loginLabel.Location = new Point(351, 106);
            loginLabel.Name = "loginLabel";
            loginLabel.Size = new Size(127, 54);
            loginLabel.TabIndex = 0;
            loginLabel.Text = "Login";
            // 
            // userNameLogin
            // 
            userNameLogin.AutoSize = true;
            userNameLogin.Font = new Font("Segoe UI", 12F);
            userNameLogin.Location = new Point(204, 184);
            userNameLogin.Name = "userNameLogin";
            userNameLogin.Size = new Size(99, 28);
            userNameLogin.TabIndex = 0;
            userNameLogin.Text = "Username";
            // 
            // userNameTextBox
            // 
            userNameTextBox.Location = new Point(325, 184);
            userNameTextBox.Name = "userNameTextBox";
            userNameTextBox.Size = new Size(244, 27);
            userNameTextBox.TabIndex = 1;
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Font = new Font("Segoe UI", 12F);
            passwordLabel.Location = new Point(204, 238);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(93, 28);
            passwordLabel.TabIndex = 0;
            passwordLabel.Text = "Password";
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(325, 238);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(244, 27);
            passwordTextBox.TabIndex = 1;
            // 
            // loginButton
            // 
            loginButton.Font = new Font("Segoe UI", 12F);
            loginButton.Location = new Point(351, 297);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(144, 49);
            loginButton.TabIndex = 2;
            loginButton.Text = "LOGIN";
            loginButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 450);
            Controls.Add(loginButton);
            Controls.Add(passwordTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(userNameTextBox);
            Controls.Add(userNameLogin);
            Controls.Add(loginLabel);
            Name = "Form1";
            Text = "Dashboard";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label loginLabel;
        private Label userNameLogin;
        private TextBox userNameTextBox;
        private Label passwordLabel;
        private TextBox passwordTextBox;
        private Button loginButton;
    }
}
