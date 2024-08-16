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
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=WINDOWS-7R0K5VR\\SQLEXPRESS01;Initial Catalog=Assignment_2;Integrated Security=True;Trusted_Connection=Yes";

        public Form1()
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
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Kiểm tra độ dài mật khẩu
            if (password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.");
                return; // Ngừng xử lý nếu mật khẩu không đủ điều kiện
            }

            SqlConnection sqlConnection = GetConnection();
            sqlConnection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = "SELECT Role FROM Users WHERE username = @username AND password = @password";
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            try
            {
                // Thực thi câu lệnh SQL và lấy vai trò của người dùng
                string role = (string)cmd.ExecuteScalar();

                if (!string.IsNullOrEmpty(role))
                {
                    // Nếu vai trò được xác định, mở form tương ứng
                    MessageBox.Show("Login successful!");

                    switch (role)
                    {
                        case "Student":
                            this.Hide();
                            Transcript formStudent = new Transcript();
                            formStudent.ShowDialog();
                            break;

                        case "Teacher":
                            this.Hide();
                            Teacher formTeacher = new Teacher();
                            formTeacher.ShowDialog();
                            break;

                        default:
                            MessageBox.Show("Role undefined!");
                            break;
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("The username or password is blank or incorrect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            SetDefaultText(txtUsername, "User name");
            SetDefaultText(txtPassword, "Password");
        }

        //private void ClearData()
        //{
        //    txtUsername.Clear();
        //    txtPassword.Clear();
        //    txtUsername.Focus();
        //}

        private void ClearData()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtPassword.PasswordChar = '\0';

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
            SetDefaultText(txtUsername, "User name");
            SetDefaultText(txtPassword, "Password");
        }

        private void SetDefaultText(TextBox textBox, string defaultText)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = defaultText;
                textBox.ForeColor = Color.Gray;
            }

            textBox.Leave += (s, e) => {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = defaultText;
                    textBox.ForeColor = Color.Gray;
                }
            };

            textBox.Enter += (s, e) => {
                if (textBox.Text == defaultText)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };
        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "User name")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                txtUsername.Text = "User name";
                txtUsername.ForeColor = Color.Gray;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Password")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                txtPassword.PasswordChar = '*';
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Text = "Password";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.PasswordChar = '\0';
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Forgot_pass forgotPasswordForm = new Forgot_pass();
            forgotPasswordForm.ShowDialog();
            this.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            User formlogin = new User();
            formlogin.ShowDialog();
            this.Close();
        }
    }
}
