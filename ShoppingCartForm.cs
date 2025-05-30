using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace CMPT_391_Project_01
{
    /// <summary>
    /// Represents the shopping cart interface where students can view, filter, select,
    /// remove, or register courses added to their cart.
    /// </summary>
    public class ShoppingCartForm : Form
    {
        private readonly int studentId;
        private readonly string connectionString = "Server=DESKTOP-JKB2ILV\\MSSQLSERVER01;Database=CMPT_391_P01;Trusted_Connection=True;TrustServerCertificate=True;";
        private readonly DataGridView cartGridView;
        private readonly Button removeButton;
        private readonly Button checkoutButton;
        private readonly ComboBox semesterFilter;
        private readonly HashSet<(int SectionID, int CourseID)> selectedBeforeReload = new();
        private readonly CheckBox selectAllCheckBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartForm"/> class.
        /// </summary>
        /// <param name="studentId">The ID of the logged-in student.</param>
        public ShoppingCartForm(int studentId)
        {
            this.studentId = studentId;

            // Form setup
            this.Text = "My Shopping Cart";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Initialize UI controls
            semesterFilter = new ComboBox
            {
                Location = new Point(50, 20),
                Width = 200,
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            semesterFilter.Items.AddRange(new[] { "All", "Fall", "Winter", "Spring" });
            semesterFilter.SelectedIndex = 0;
            semesterFilter.SelectedIndexChanged += (s, e) => LoadCartItems();

            cartGridView = new DataGridView
            {
                Location = new Point(50, 60),
                Width = 880,
                Height = 440,
                ReadOnly = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false
            };
            StyleGridView();

            cartGridView.CellContentClick += (s, e) =>
            {
                if (e.ColumnIndex == cartGridView.Columns["Select"].Index && e.RowIndex >= 0)
                    cartGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            cartGridView.CellValueChanged += CartGridView_CellValueChanged;
            cartGridView.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (cartGridView.IsCurrentCellDirty)
                    cartGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            removeButton = new Button
            {
                Text = "Remove Selected",
                Height = 40,
                Width = 150,
                Location = new Point(50, 520),
                BackColor = Color.FromArgb(220, 90, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Enabled = false
            };
            removeButton.Click += RemoveButton_Click;

            checkoutButton = new Button
            {
                Text = "Checkout",
                Height = 40,
                Width = 150,
                Location = new Point(220, 520),
                BackColor = Color.FromArgb(11, 35, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            checkoutButton.Click += CheckoutButton_Click;

            // Add components to form
            this.Controls.Add(semesterFilter);
            this.Controls.Add(cartGridView);
            this.Controls.Add(removeButton);
            this.Controls.Add(checkoutButton);

            selectAllCheckBox = new CheckBox
            {
                Size = new Size(15, 15),
                BackColor = Color.Transparent
            };

            LoadCartItems();
        }

        /// <summary>
        /// Styles the DataGridView headers and rows.
        /// </summary>
        private void StyleGridView()
        {
            cartGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            cartGridView.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            cartGridView.DefaultCellStyle.ForeColor = Color.Black;
            cartGridView.DefaultCellStyle.BackColor = Color.White;
            cartGridView.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            cartGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            cartGridView.EnableHeadersVisualStyles = false;
            cartGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(11, 35, 94);
            cartGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }

        /// <summary>
        /// Loads cart items from the database via stored procedure and populates the grid.
        /// </summary>
        private void LoadCartItems()
        {
            // Save selection state before refresh
            selectedBeforeReload.Clear();
            foreach (DataGridViewRow row in cartGridView.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Select"].Value) == true)
                {
                    int sid = Convert.ToInt32(row.Cells["SectionID"].Value);
                    int cid = Convert.ToInt32(row.Cells["CourseID"].Value);
                    selectedBeforeReload.Add((sid, cid));
                }
            }

            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using var cmd = new SqlCommand("sp_GetCartItems", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@Semester", semesterFilter.SelectedItem?.ToString() ?? "All");

                var adapter = new SqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);

                if (!table.Columns.Contains("Select"))
                    table.Columns.Add("Select", typeof(bool));

                foreach (DataRow row in table.Rows)
                {
                    int sid = Convert.ToInt32(row["SectionID"]);
                    int cid = Convert.ToInt32(row["CourseID"]);
                    row["Select"] = selectedBeforeReload.Contains((sid, cid));
                }

                cartGridView.DataSource = table;
                AddSelectAllCheckboxToHeader();
                UpdateRemoveButtonState();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load cart.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Adds the "Select All" checkbox to the DataGridView header.
        /// </summary>
        private void AddSelectAllCheckboxToHeader()
        {
            Rectangle headerCell = cartGridView.GetCellDisplayRectangle(cartGridView.Columns["Select"].Index, -1, true);
            selectAllCheckBox.Location = new Point(
                headerCell.Left + (headerCell.Width - selectAllCheckBox.Width) / 2,
                headerCell.Top + (headerCell.Height - selectAllCheckBox.Height) / 2);

            selectAllCheckBox.CheckedChanged -= SelectAllCheckBox_CheckedChanged;
            selectAllCheckBox.CheckedChanged += SelectAllCheckBox_CheckedChanged;

            if (!cartGridView.Controls.Contains(selectAllCheckBox))
            {
                cartGridView.Controls.Add(selectAllCheckBox);
            }
        }

        /// <summary>
        /// Handles logic for selecting or deselecting all rows when the header checkbox is toggled.
        /// </summary>
        private void SelectAllCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            foreach (DataGridViewRow row in cartGridView.Rows)
            {
                row.Cells["Select"].Value = selectAllCheckBox.Checked;
            }
        }

        /// <summary>
        /// Enables or disables the Remove button depending on checkbox selections.
        /// </summary>
        private void UpdateRemoveButtonState()
        {
            bool anySelected = cartGridView.Rows.Cast<DataGridViewRow>()
                .Any(row => Convert.ToBoolean(row.Cells["Select"].Value) == true);
            removeButton.Enabled = anySelected;
        }

        /// <summary>
        /// Handles checkbox value changes to update button state.
        /// </summary>
        private void CartGridView_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == cartGridView.Columns["Select"].Index)
                UpdateRemoveButtonState();
        }

        /// <summary>
        /// Removes selected rows or all rows if nothing is selected, using stored procedures.
        /// </summary>
        private void RemoveButton_Click(object? sender, EventArgs e)
        {
            var selectedItems = cartGridView.Rows.Cast<DataGridViewRow>()
                .Where(row => Convert.ToBoolean(row.Cells["Select"].Value) == true)
                .Select(row => (SectionID: Convert.ToInt32(row.Cells["SectionID"].Value), CourseID: Convert.ToInt32(row.Cells["CourseID"].Value)))
                .ToList();

            if (!selectedItems.Any())
            {
                DialogResult result = MessageBox.Show("No items selected. Do you want to remove all courses from your cart?",
                    "Confirm Clear All", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using var conn = new SqlConnection(connectionString);
                    conn.Open();

                    using var cmd = new SqlCommand("sp_ClearCart", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("All courses removed from cart.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCartItems();
                }
                return;
            }

            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                foreach (var (sectionId, courseId) in selectedItems)
                {
                    using var cmd = new SqlCommand("sp_RemoveFromCart", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@SectionID", sectionId);
                    cmd.Parameters.AddWithValue("@CourseID", courseId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Selected courses removed from cart.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCartItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing selected courses.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Registers the selected courses using a stored procedure and removes them from the cart.
        /// </summary>
        private void CheckoutButton_Click(object? sender, EventArgs e)
        {
            var selectedItems = cartGridView.Rows.Cast<DataGridViewRow>()
                .Where(row => Convert.ToBoolean(row.Cells["Select"].Value) == true)
                .Select(row => (SectionID: Convert.ToInt32(row.Cells["SectionID"].Value), CourseID: Convert.ToInt32(row.Cells["CourseID"].Value)))
                .ToList();

            if (!selectedItems.Any())
            {
                MessageBox.Show("Please select courses to register.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to register the selected courses?",
                "Confirm Checkout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            List<int> successList = new();
            List<(int CourseID, string Reason)> failedList = new();

            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                foreach (var (sectionId, courseId) in selectedItems)
                {
                    using var cmd = new SqlCommand("RegisterStudentToSection", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@SectionID", sectionId);
                    cmd.Parameters.AddWithValue("@CourseID", courseId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        successList.Add(courseId);

                        using var removeCmd = new SqlCommand("sp_RemoveFromCart", conn) { CommandType = CommandType.StoredProcedure };
                        removeCmd.Parameters.AddWithValue("@StudentID", studentId);
                        removeCmd.Parameters.AddWithValue("@SectionID", sectionId);
                        removeCmd.Parameters.AddWithValue("@CourseID", courseId);
                        removeCmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        string reason = ex.Message.Contains("prerequisite", StringComparison.OrdinalIgnoreCase) ? "Missing prerequisite" :
                                        ex.Message.Contains("Schedule conflict", StringComparison.OrdinalIgnoreCase) ? "Schedule conflict" :
                                        ex.Message;
                        failedList.Add((courseId, reason));
                    }
                }

                string summary = "";
                if (successList.Any())
                    summary += $"✅ Registered: {string.Join(", ", successList)}\n";

                if (failedList.Any())
                    summary += $"❌ Failed:\n" + string.Join("\n", failedList.Select(f => $"- Course ID {f.CourseID}: {f.Reason}"));

                if (string.IsNullOrEmpty(summary)) summary = "No registrations processed.";

                MessageBox.Show(summary, "Checkout Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCartItems();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Checkout failed.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
