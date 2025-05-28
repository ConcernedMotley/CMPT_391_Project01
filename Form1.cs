using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

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
            string username = userTextBox!.Text;
            string password = passTextBox!.Text;

            if (ValidateCredentials(username, password, out string studentId))
            {
                Session.StudentID = studentId;
                SemesterSelectionForm semesterForm = new SemesterSelectionForm();
                semesterForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ValidateCredentials(string username, string password, out string studentId)
        {
            studentId = "";
            string connectionString = "Server=DESKTOP-JKB2ILV\\MSSQLSERVER01;Database=CMPT_391_P01;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
            SELECT StudentID 
            FROM StudentCredentials 
            WHERE Username = @Username AND StudentPassword = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    object? result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        studentId = result.ToString()!;
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
