using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace CMPT_391_Project_01
{
    /// <summary>
    /// The main login form of the application.
    /// Handles user input for authentication and navigates to the semester selection screen upon successful login.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Initializes the login form UI components.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Triggered when the login button is clicked.
        /// Validates the username and password using a stored procedure.
        /// If credentials are valid, it opens the SemesterSelectionForm and hides the login form.
        /// </summary>
        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = userTextBox!.Text;
            string password = passTextBox!.Text;

            if (ValidateCredentials(username, password, out string studentId))
            {
                // Store the student ID in session
                Session.StudentID = studentId;

                // Proceed to semester selection screen
                SemesterSelectionForm semesterForm = new SemesterSelectionForm();
                semesterForm.Show();
                this.Hide();
            }
            else
            {
                // Show error message if credentials are invalid
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Validates the entered username and password using the stored procedure sp_ValidateStudentLogin.
        /// </summary>
        /// <param name="username">The username entered by the user.</param>
        /// <param name="password">The password entered by the user.</param>
        /// <param name="studentId">Outputs the corresponding student ID if login is successful.</param>
        /// <returns>True if credentials are valid, otherwise false.</returns>
        private bool ValidateCredentials(string username, string password, out string studentId)
        {
            studentId = "";
            string connectionString = "Server=DESKTOP-JKB2ILV\\MSSQLSERVER01;Database=CMPT_391_P01;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_ValidateStudentLogin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    // ExecuteScalar returns the student ID if found
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
