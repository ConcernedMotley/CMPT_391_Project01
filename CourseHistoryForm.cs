using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace CMPT_391_Project_01
{
    public class CourseHistoryForm : Form
    {
        private readonly int studentId;
        private readonly string connectionString = "Server=DESKTOP-JKB2ILV\\MSSQLSERVER01;Database=CMPT_391_P01;Trusted_Connection=True;TrustServerCertificate=True;";
        private readonly DataGridView historyGrid = new DataGridView();

        public CourseHistoryForm(int studentId)
        {
            this.studentId = studentId;

            this.Text = "Course History";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            historyGrid.Dock = DockStyle.Fill;
            historyGrid.ReadOnly = true;
            historyGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            historyGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            historyGrid.BackgroundColor = Color.White;
            historyGrid.BorderStyle = BorderStyle.None;

            StyleGrid(historyGrid);
            LoadCourseHistory();

            this.Controls.Add(historyGrid);
        }

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

                if (table.Columns.Contains("courseLabel"))
                    table.Columns["courseLabel"]!.ColumnName = "Course Code";

                if (table.Columns.Contains("CourseName"))
                    table.Columns["CourseName"]!.ColumnName = "Course Name";

                if (table.Columns.Contains("Semester"))
                    table.Columns["Semester"]!.ColumnName = "Semester";

           


                historyGrid.DataSource = table;
                var academicYearColumn = historyGrid.Columns["Academic Year"];
                if (academicYearColumn != null)
                {
                    historyGrid.Sort(academicYearColumn, ListSortDirection.Descending);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load course history.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


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
