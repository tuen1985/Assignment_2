using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM_STD_ATT
{
    public partial class Teacher : Form
    {
        private string connectionString = "Data Source=WINDOWS-7R0K5VR\\SQLEXPRESS01;Initial Catalog=Assignment_2;Integrated Security=True;Trusted_Connection=Yes";
        public Teacher()
        {
            InitializeComponent();
        }

        private void btnEditTranscript_Click(object sender, EventArgs e)
        {
            this.Hide();
            TeacherTranscript formlogin = new TeacherTranscript();
            formlogin.ShowDialog();
            this.Close();
            button4.BackColor = Color.White;
        }

        private void btnEditTranscript_MouseDown(object sender, MouseEventArgs e)
        {
            btnEditTranscript.BackColor = Color.SteelBlue; // Màu khi button được nhấn
        }

        private void btnEditTranscript_MouseUp(object sender, MouseEventArgs e)
        {
            btnEditTranscript.BackColor = SystemColors.Control; // Màu trở lại khi button được thả
        }


        private void dtgUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
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
        private void button4_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.SteelBlue;
        }

        private void Teacher_Load(object sender, EventArgs e)
        {
            button4.BackColor = Color.SteelBlue;
        }
    }
}
