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
    public partial class User : Form
    {
        private string connectionString = "Data Source=WINDOWS-7R0K5VR\\SQLEXPRESS01;Initial Catalog=Assignment_2;Integrated Security=True;Trusted_Connection=Yes";

        public User()
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



        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserID.Text)) 
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                string firstname = txtFirstname.Text;
                string lastname = txtLastname.Text;
                string email = txtEmail.Text;
                string dob = txtDob.Text;

                string role = string.Empty;
                if (rbTeacher.Checked)
                {
                    role = "Teacher";
                }
                else if (rbStudent.Checked)
                {
                    role = "Student";
                }
                if (string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("Username is required");
                    return;
                }           
                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Password is required");
                    return;
                }

                if (string.IsNullOrEmpty(firstname))
                {
                    MessageBox.Show("First name is required");
                    return;
                }

                if (string.IsNullOrEmpty(lastname))
                {
                   MessageBox.Show("Last name is required");
                    return;
                }

                if (string.IsNullOrEmpty(email))
                {
                   MessageBox.Show("Email is required");
                    return;
                }

               if (string.IsNullOrEmpty(role))
               {
                   MessageBox.Show("You must choose role");
                   return;
               }

                // Kiểm tra định dạng email
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    if (addr.Address != email)
                    {
                        MessageBox.Show("Invalid email format");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("Invalid email format");
                    return;
                }

                if (password.Length < 8)
                {
                    MessageBox.Show("Password must be at least 8 characters long.");
                    return; // Ngừng xử lý nếu mật khẩu không đủ điều kiện
                }

                SqlConnection connection = GetConnection();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "INSERT INTO Users ([Username],[Password],[FirstName],[LastName],[Email],[DateOfBirth],[Role]) \r\n Values (" +
                                   "@username ,@password, @firstname, @lastname, @email, @dob, @role)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("password", password);
                    command.Parameters.AddWithValue("firstname", firstname);
                    command.Parameters.AddWithValue("lastname", lastname);
                    command.Parameters.AddWithValue("email", email);
                    command.Parameters.Add("dob", SqlDbType.Date).Value = dob;
                    command.Parameters.AddWithValue("role", role);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Add user success");
                        GetUser();
                    }
                    else
                    {
                        MessageBox.Show("Cannot add user");
                    }
                }
                connection.Close();
            }
            else
            {
                MessageBox.Show("No User select");
            }

        }

        private void GetUser()
        {
            SqlConnection connection = GetConnection();
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                string query = "SELECT * FROM Users";
                SqlCommand command = new SqlCommand(@query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dtgUser.DataSource = data;

            }
            connection.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserID.Text))
            {
                int userID = Convert.ToInt32(txtUserID.Text);
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                string firstname = txtFirstname.Text;
                string lastname = txtLastname.Text;
                string email = txtEmail.Text;
                string dob = txtDob.Text;

                string role = string.Empty;
                if (rbTeacher.Checked)
                {
                    role = "Teacher";
                }
                else if (rbStudent.Checked)
                {
                    role = "Student";
                }

                if (password.Length < 8)
                {
                    MessageBox.Show("Password must be at least 8 characters long.");
                    return; // Ngừng xử lý nếu mật khẩu không đủ điều kiện
                }

                SqlConnection connection = GetConnection();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "UPDATE Users SET " +
                                   "Username = @Username ," +
                                   "password = @password ," +
                                   "firstname = @firstname ," +
                                   "lastname = @lastname ," +
                                   "email = @email ," +
                                   "DateOfBirth = @dob ," +
                                   "role = @role WHERE UserID = @UserID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("UserID", userID);
                    command.Parameters.AddWithValue("Username", username);
                    command.Parameters.AddWithValue("password", password);
                    command.Parameters.AddWithValue("firstname", firstname);
                    command.Parameters.AddWithValue("lastname", lastname);
                    command.Parameters.AddWithValue("email", email);
                    command.Parameters.Add("dob", SqlDbType.Date).Value = dob;
                    command.Parameters.AddWithValue("role", role);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Update user success");
                        GetUser();
                    }
                    else
                    {
                        MessageBox.Show("Cannot update user");
                    }
                }
                connection.Close();
            }
            else
            {
                MessageBox.Show("No User select");
            }
        }


    private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserID.Text))
            {
                int userID = Convert.ToInt32(txtUserID.Text);
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                string firstname = txtFirstname.Text;
                string lastname = txtLastname.Text;
                string email = txtEmail.Text;
                string dob = txtDob.Text;

                string role = string.Empty;
                if (rbTeacher.Checked)
                {
                    role = "Teacher";
                }
                else if (rbStudent.Checked)
                {
                    role = "Student";
                }
                SqlConnection connection = GetConnection();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "DELETE Users WHERE userID = @userID";
                    SqlCommand command = new SqlCommand(query, connection);
                    //SqlCommand command = new SqlCommand();
                    //command.Connection = connection;
                    //command.CommandText = "DELETE Users WHERE ID = @ID";
                    command.Parameters.AddWithValue("userID", userID);
                    //command.Parameters.Add("ID",1);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Delete user success");
                        GetUser();
                    }
                    else
                    {
                        MessageBox.Show("Cannot delete user");
                    }
                }
                connection.Close();
            }
            else
            {
                MessageBox.Show("No User select");
            }          
        }

        private void dtgUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dtgUser.SelectedCells[0].RowIndex;

            int userID = Convert.ToInt32(dtgUser.Rows[index].Cells[0].Value);
            txtUserID.Text = userID.ToString();
            txtFirstname.Text = Convert.ToString(dtgUser.Rows[index].Cells[3].Value);
            txtLastname.Text = Convert.ToString(dtgUser.Rows[index].Cells[4].Value);
            txtUsername.Text = Convert.ToString(dtgUser.Rows[index].Cells[1].Value);
            txtPassword.Text = Convert.ToString(dtgUser.Rows[index].Cells[2].Value);
            txtEmail.Text = Convert.ToString(dtgUser.Rows[index].Cells[5].Value);
            txtDob.Text = Convert.ToString(dtgUser.Rows[index].Cells[6].Value);
            if (Convert.ToString(dtgUser.Rows[index].Cells[7].Value) == "Teacher")
            {
                rbTeacher.Checked = true;
            }
            else
            {
                rbStudent.Checked = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirm exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();

                Form1 formlogin = new Form1();
                formlogin.ShowDialog();

                this.Close();
            }
        }

        private void User_Load(object sender, EventArgs e)
        {
            // Call GetUser to load data into DataGridView when the form loads
            GetUser();
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textSearch_Enter(object sender, EventArgs e)
        {
            
        }

        private void dtgUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchValue = textSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchValue))
            {
                // Kiểm tra xem giá trị tìm kiếm có phải là một số nguyên hợp lệ không
                if (int.TryParse(searchValue, out int userID))
                {
                    SqlConnection connection = GetConnection();
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        // Truy vấn để tìm kiếm người dùng bằng UserID
                        string query = "SELECT * FROM Users WHERE UserID = @UserID";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@UserID", userID);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable data = new DataTable();
                        adapter.Fill(data);

                        if (data.Rows.Count > 0)
                        {
                            dtgUser.DataSource = data;
                        }
                        else
                        {
                            MessageBox.Show("No users found.");
                            dtgUser.DataSource = null;
                        }
                    }
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a valid numeric UserID.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a UserID to search.");
            }
        }

        private void ClearData()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtLastname.Text = "";
            txtFirstname.Text = "";
            txtEmail.Text = "";
            txtDob.Text = "";
            rbStudent.Checked = false;
            rbTeacher.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearData();
        }
    }
}