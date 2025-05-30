using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace CMPT_391_Project_01
{
    /// <summary>
    /// Displays a student's registered classes with filters for Fall and Winter semesters.
    /// Pulls data from the SQL Server using a stored procedure.
    /// </summary>
    public class ViewRegisteredClassesForm : Form
    {
        private readonly int studentId;
        private readonly string connectionString =
            "Server=DESKTOP-JKB2ILV\\MSSQLSERVER01;Database=CMPT_391_P01;Trusted_Connection=True;TrustServerCertificate=True;";

        private readonly DataGridView registeredClassesGridView;
        private readonly Button fallFilterButton;
        private readonly Button winterFilterButton;

        /// <summary>
        /// Initializes the form with the given student ID.
        /// Sets up layout, styling, event handlers, and loads the default class list.
        /// </summary>
        /// <param name="studentId">ID of the logged-in student</param>
        public ViewRegisteredClassesForm(int studentId)
        {
            this.studentId = studentId;

            // ===== DataGridView Setup =====
            registeredClassesGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            StyleGridView();

            // ===== Filter Buttons =====
            fallFilterButton = new Button
            {
                Text = "FALL 2024",
                Height = 40,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(200, 220, 255),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            winterFilterButton = new Button
            {
                Text = "WINTER 2025",
                Height = 40,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(220, 255, 200),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // ===== Filter Events =====
            fallFilterButton.Click += (s, e) => LoadRegisteredClasses("Fall", 2024);
            winterFilterButton.Click += (s, e) => LoadRegisteredClasses("Winter", 2025);

            // ===== Form Setup =====
            this.Text = "My Registered Classes";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // ===== Add Controls =====
            Controls.Add(registeredClassesGridView);
            Controls.Add(winterFilterButton);
            Controls.Add(fallFilterButton);

            // Load default data on form load
            this.Load += (s, e) => LoadRegisteredClasses();
        }

        /// <summary>
        /// Loads the registered classes for the student.
        /// Optionally filters by semester and academic year.
        /// </summary>
        /// <param name="semester">Semester name (Fall/Winter) or null</param>
        /// <param name="crseYear">Course year or null</param>
        private void LoadRegisteredClasses(string? semester = null, int? crseYear = null)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("GetRegisteredClassesForStudent", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parameters: student ID and optional filters
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@Semester", (object?)semester ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CrseYear", (object?)crseYear ?? DBNull.Value);

                var adapter = new SqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);

                // Inform if no results
                if (table.Rows.Count == 0)
                {
                    MessageBox.Show("No registered classes found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                registeredClassesGridView.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load registered classes.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Applies consistent style to the DataGridView (fonts, colors, selection, etc.)
        /// </summary>
        private void StyleGridView()
        {
            registeredClassesGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            registeredClassesGridView.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            registeredClassesGridView.DefaultCellStyle.ForeColor = Color.Black;
            registeredClassesGridView.DefaultCellStyle.BackColor = Color.White;
            registeredClassesGridView.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            registeredClassesGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            registeredClassesGridView.EnableHeadersVisualStyles = false;
            registeredClassesGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(11, 35, 94);
            registeredClassesGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }
    }
}
