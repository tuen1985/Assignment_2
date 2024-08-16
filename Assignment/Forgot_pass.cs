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
    public partial class Forgot_pass : Form
    {
        private string connectionString = "Data Source=WINDOWS-7R0K5VR\\SQLEXPRESS01;Initial Catalog=Assignment_2;Integrated Security=True;Trusted_Connection=Yes";
        SqlConnectionStringBuilder sqlstrbuilder = new SqlConnectionStringBuilder();
        public Forgot_pass()
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

        private void btnshowpass_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;

            SqlConnection sqlConnection = GetConnection();
            sqlConnection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = "SELECT Password FROM Users WHERE Email = @Email";
            cmd.Parameters.AddWithValue("@Email", email);
            try
            {
                var bien_tam = cmd.ExecuteScalar();
                var password = Convert.ToString(bien_tam);
                if (txtEmail.Text == "")
                {
                    MessageBox.Show("Please enter Email");
                    return;
                }
                else if (!string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Your password is: " + password);
                    return;

                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                return;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                {
                    MessageBox.Show("Invalid email format");
                    return;
                }
                else
                {
                    MessageBox.Show("Email does not exist. Please re-enter.");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Invalid email format");
                return;
            }
            
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}
