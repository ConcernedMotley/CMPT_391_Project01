using System;
using System.Windows.Forms;

namespace CMPT_391_Project_01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = userTextBox.Text;
            string password = passTextBox.Text;

            // Simple hardcoded validation for demo purposes
            if (username == "admin" && password == "password")
            {
                SemesterSelectionForm semesterForm = new SemesterSelectionForm();
                semesterForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
