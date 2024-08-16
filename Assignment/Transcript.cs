using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ASM_STD_ATT
{
    public partial class Transcript : Form
    {
        private string connectionString = "Data Source=WINDOWS-7R0K5VR\\SQLEXPRESS01;Initial Catalog=Assignment_2;Integrated Security=True;Trusted_Connection=Yes";
        public Transcript()
        {
            InitializeComponent();
        }

        private SqlConnection GetConnection()
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Cannot connect database",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                MessageBox.Show(ex.Message);
            }
            return connection;

        }

        private void dtgUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void llbLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Log out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();

                Form1 formlogin = new Form1();
                formlogin.ShowDialog();

                this.Close();
            }
        }

        private void btnShowTranscript_Click(object sender, EventArgs e)
        {
            btnShowTranscript.BackColor = Color.SteelBlue;
            button4.BackColor = Color.White;           
            txtSearch.Visible = true;
            label1.Visible = true;
            btnSearch.Visible = true;
            panel3.Visible = true;
            panel4.Visible = false;
            dataGridView3.Visible = false;
            dataGridView1.Visible = true ;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.SteelBlue;
            btnShowTranscript.BackColor = Color.White;
            label1.Visible = false;
            txtSearch.Visible = false;
            dataGridView3.Visible= true;
            dataGridView1.Visible= false;
            btnSearch.Visible = false;  
            panel3.Visible = false;
            panel4.Visible = true;
        }

        private void Transcript_Load(object sender, EventArgs e)
        {
            button4_Click(sender, e);
            button4.BackColor = Color.SteelBlue;
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            string searchValue = txtSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchValue))
            {
                // Kiểm tra xem giá trị tìm kiếm có phải là một số nguyên hợp lệ không
                if (int.TryParse(searchValue, out int studentID))
                {
                    SqlConnection connection = GetConnection();
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        // Truy vấn để tìm kiếm người dùng bằng UserID
                        string query = @"
                        SELECT 
                        t.TranscriptID, 
                        s.NameofStudent, 
                        t.StudentID, 
                        t.Score,
                        t.CourseID                        
                    FROM Transcript t
                    INNER JOIN Students s ON t.StudentID = s.StudentID
                    WHERE t.StudentID = @StudentID";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@StudentID", studentID);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable data = new DataTable();
                        adapter.Fill(data);

                        if (data.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = data;
                        }
                        else
                        {
                            MessageBox.Show("No students found.");
                            dataGridView1.DataSource = null;
                        }
                    }
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a valid numeric Student ID.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a Student ID to search.");
            }
        }
    }
}
