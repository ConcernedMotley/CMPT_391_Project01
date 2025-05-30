using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace CMPT_391_Project_01
{
    /// <summary>
    /// A Windows Form that displays the course history (completed courses) for a specific student.
    /// </summary>
    public class CourseHistoryForm : Form
    {
        private readonly int studentId; // The ID of the currently logged-in student
        private readonly string connectionString = "Server=DESKTOP-JKB2ILV\\MSSQLSERVER01;Database=CMPT_391_P01;Trusted_Connection=True;TrustServerCertificate=True;";
        private readonly DataGridView historyGrid = new DataGridView(); // Grid to display completed courses

        /// <summary>
        /// Initializes the form and loads the student's course history.
        /// </summary>
        /// <param name="studentId">The unique ID of the student</param>
        public CourseHistoryForm(int studentId)
        {
            this.studentId = studentId;

            // Basic form setup
            this.Text = "Course History";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Configure DataGridView properties
            historyGrid.Dock = DockStyle.Fill;
            historyGrid.ReadOnly = true;
            historyGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            historyGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            historyGrid.BackgroundColor = Color.White;
            historyGrid.BorderStyle = BorderStyle.None;

            StyleGrid(historyGrid);   // Apply custom styles
            LoadCourseHistory();      // Load and display course data

            this.Controls.Add(historyGrid);
        }

        /// <summary>
        /// Connects to the database, runs the stored procedure to get completed courses, 
        /// and binds the result to the DataGridView.
        /// </summary>
        private void LoadCourseHistory()
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("GetCompletedCoursesForStudent", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@StudentID", studentId);

                var adapter = new SqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);

                // Rename columns for display
                if (table.Columns.Contains("Course"))
                    table.Columns["Course"]!.ColumnName = "Course";

                if (table.Columns.Contains("CourseName"))
                    table.Columns["CourseName"]!.ColumnName = "Course Name";

                if (table.Columns.Contains("Semester"))
                    table.Columns["Semester"]!.ColumnName = "Semester";

                if (table.Columns.Contains("Grade"))
                    table.Columns["Grade"]!.ColumnName = "Letter Grade";


                // Bind the data to the grid
                historyGrid.DataSource = null;
                historyGrid.DataSource = table;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load course history.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Styles the appearance of the DataGridView for a clean, consistent UI.
        /// </summary>
        /// <param name="grid">The DataGridView to style</param>
        private void StyleGrid(DataGridView grid)
        {
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            grid.DefaultCellStyle.ForeColor = Color.Black;
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(11, 35, 94);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }
    }
}
