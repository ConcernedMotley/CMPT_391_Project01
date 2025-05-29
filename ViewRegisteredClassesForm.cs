using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace CMPT_391_Project_01
{
    public class ViewRegisteredClassesForm : Form
    {
        private readonly int studentId;
        private readonly string connectionString = "Server=DESKTOP-JKB2ILV\\MSSQLSERVER01;Database=CMPT_391_P01;Trusted_Connection=True;TrustServerCertificate=True;";
        private readonly DataGridView registeredClassesGridView;
        private readonly Button fallFilterButton;
        private readonly Button winterFilterButton;

        public ViewRegisteredClassesForm(int studentId)
        {
            this.studentId = studentId;

            // DataGridView setup
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

            // Filter buttons
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

            fallFilterButton.Click += (s, e) => LoadRegisteredClasses("Fall", 2024);
            winterFilterButton.Click += (s, e) => LoadRegisteredClasses("Winter", 2025);


            // Form setup
            this.Text = "My Registered Classes";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Controls.Add(registeredClassesGridView);
            Controls.Add(winterFilterButton);
            Controls.Add(fallFilterButton);

            this.Load += (s, e) => LoadRegisteredClasses(); // Load all for current year
        }

        private void LoadRegisteredClasses(string? semester = null, int? crseYear = null)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("GetRegisteredClassesForStudent", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@Semester", (object?)semester ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CrseYear", (object?)crseYear ?? DBNull.Value);

                var adapter = new SqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);

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
