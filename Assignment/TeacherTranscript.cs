using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM_STD_ATT
{
    public partial class TeacherTranscript : Form
    {
        private string connectionString = "Data Source=WINDOWS-7R0K5VR\\SQLEXPRESS01;Initial Catalog=Assignment_2;Integrated Security=True;Trusted_Connection=Yes";

        public TeacherTranscript()
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

        private void TeacherTranscript_Load(object sender, EventArgs e)
        {
            // Load the transcript data including CourseID
            GetTranscript();
        }

        private void dtgTranscript_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dtgTranscript.SelectedCells[0].RowIndex;
            txtTranscriptID.Text = Convert.ToString(dtgTranscript.Rows[index].Cells[0].Value);
            txtStudentID.Text = Convert.ToString(dtgTranscript.Rows[index].Cells[2].Value);
            txtScore.Text = Convert.ToString(dtgTranscript.Rows[index].Cells[3].Value);
            cbbCourseID.Text = Convert.ToString(dtgTranscript.Rows[index].Cells[4].Value); // Display the CourseID
        }

        private bool IsTranscriptOrStudentExists(int transcriptID, int studentID)
        {
            SqlConnection connection = GetConnection();
            connection.Open();

            if (connection.State == ConnectionState.Open)
            {
                string query = "SELECT COUNT(*) FROM Transcript WHERE TranscriptID = @TranscriptID OR StudentID = @StudentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TranscriptID", transcriptID);
                command.Parameters.AddWithValue("@StudentID", studentID);

                int count = (int)command.ExecuteScalar();
                connection.Close();
                return count > 0; // Trả về true nếu TranscriptID hoặc StudentID đã tồn tại
            }

            connection.Close();
            return false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Ensure that all necessary fields are filled
            string studentID = txtStudentID.Text;
            string score = txtScore.Text;
            string courseID = cbbCourseID.Text;
            string transcriptID = txtTranscriptID.Text;

            // Validate inputs
            if (string.IsNullOrEmpty(transcriptID))
            {
                MessageBox.Show("Transcript ID is required");
                return;
            }
            if (string.IsNullOrEmpty(studentID))
            {
                MessageBox.Show("Student ID is required");
                return;
            }
            if (string.IsNullOrEmpty(courseID))
            {
                MessageBox.Show("You must choose Course ID");
                return;
            }

            if (string.IsNullOrEmpty(score))
            {
                MessageBox.Show("Score is required");
                return;
            }

            // Ensure the inputs are convertible to the appropriate types
            if (!int.TryParse(studentID, out int studentIDInt))
            {
                MessageBox.Show("Student ID must be a valid integer");
                return;
            }

            if (!int.TryParse(courseID, out int courseIDInt))
            {
                MessageBox.Show("Course ID must be a valid integer");
                return;
            }

            if (!int.TryParse(score, out int scoreInt))
            {
                MessageBox.Show("Score must be a valid integer");
                return;
            }

            if (!int.TryParse(transcriptID, out int transcriptIDInt))
            {
                MessageBox.Show("Transcript ID must be a valid integer");
                return;
            }

            // Check if TranscriptID or StudentID already exists
            if (IsTranscriptOrStudentExists(transcriptIDInt, studentIDInt))
            {
                MessageBox.Show("TranscriptID or StudentID already exists in the database.");
                return;
            }

            // Open SQL connection and execute the insert query
            SqlConnection connection = GetConnection();
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                string query = "INSERT INTO Transcript (TranscriptID, StudentID, CourseID, Score) " +
                    "VALUES (@TranscriptID, @StudentID, @CourseID, @Score)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TranscriptID", transcriptIDInt);
                command.Parameters.AddWithValue("@StudentID", studentIDInt);
                command.Parameters.AddWithValue("@CourseID", courseIDInt);
                command.Parameters.AddWithValue("@Score", scoreInt);

                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Transcript table entry added successfully");
                    LoadTranscriptData();
                }
                else
                {
                    MessageBox.Show("Failed to add entry to transcript table");
                }
            }
            connection.Close();
        }

        private void GetTranscript()
        {
            SqlConnection connection = GetConnection();
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = @"
                        SELECT t.TranscriptID, s.NameofStudent, s.StudentID, t.Score, c.CourseID
                        FROM Transcript t
                        INNER JOIN Students s ON t.StudentID = s.StudentID
                        INNER JOIN Course c ON t.CourseID = c.CourseID";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    dtgTranscript.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void LoadTranscriptData()
        {
            SqlConnection connection = GetConnection();
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = @"
                        SELECT t.TranscriptID, s.NameofStudent, s.StudentID, t.Score, c.CourseID
                        FROM Transcript t
                        INNER JOIN Students s ON t.StudentID = s.StudentID
                        INNER JOIN Course c ON t.CourseID = c.CourseID";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    dtgTranscript.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void GetUser()
        {
            SqlConnection connection = GetConnection();
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                string query = "SELECT * FROM Transcript";
                SqlCommand command = new SqlCommand(@query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dtgTranscript.DataSource = data;
            }
            connection.Close();
        }

        private void ClearData()
        {
            txtTranscriptID.Text = "";
            txtScore.Text = "";
            txtStudentID.Text = "";
            cbbCourseID.SelectedIndex = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTranscriptID.Text))
            {
                // Lấy thông tin từ các trường
                int transcriptID = Convert.ToInt32(txtTranscriptID.Text);
                int studentID = Convert.ToInt32(txtStudentID.Text);
                int score = Convert.ToInt32(txtScore.Text);
                int courseID = Convert.ToInt32(cbbCourseID.Text);

                // Kết nối với cơ sở dữ liệu và thực hiện cập nhật
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        string query = @"
                    UPDATE Transcript 
                    SET StudentID = @studentID, Score = @score, CourseID = @courseID
                    WHERE TranscriptID = @transcriptID";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@studentID", studentID);
                            command.Parameters.AddWithValue("@score", score);
                            command.Parameters.AddWithValue("@courseID", courseID);
                            command.Parameters.AddWithValue("@transcriptID", transcriptID);

                            int result = command.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show("Update success");
                                LoadTranscriptData();
                            }
                            else
                            {
                                MessageBox.Show("Cannot update");
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No rows were selected");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTranscriptID.Text))
            {
                int transcriptID = Convert.ToInt32(txtTranscriptID.Text);

                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        string query = "DELETE FROM Transcript WHERE TranscriptID = @transcriptID";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@transcriptID", transcriptID);

                            int result = command.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show("Delete success");
                                LoadTranscriptData();
                            }
                            else
                            {
                                MessageBox.Show("Cannot delete");
                            }
                        }
                    }
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("No rows were selected");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                Teacher formlogin = new Teacher();
                formlogin.ShowDialog();
                this.Close();
            }
        }
    }
}
