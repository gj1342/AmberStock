using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class user_settings : Form
    {
        private string _ID;
        private string _userName;
        private string _password;

        public user_settings(string ID, string userName, string password)
        {
            InitializeComponent();
            _ID = ID;
            _userName = userName;
            _password = password;

        }

        SqlConnection con = new SqlConnection("Data Source=GENE\\SQLEXPRESS01;Initial Catalog=BeneAmber;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        private void user_settings_Load(object sender, EventArgs e)
        {
            panel9.Hide();
            panel11.Hide();

            label3.Text = _userName;
            label7.Text = _userName;

            textBox2.UseSystemPasswordChar = true;
            textBox3.UseSystemPasswordChar = true;

        }

        //exit
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //refresh
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        //navigation buttons
        private void button3_Click(object sender, EventArgs e)
        {
            Transaction transaction = new Transaction(_ID, _userName, _password);
            transaction.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer(_ID, _userName, _password);
            customer.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Product product = new Product(_ID, _userName, _password);
            product.Show();
            this.Hide();
        }


        //show buttons
        private void button5_Click(object sender, EventArgs e)
        {
            panel9.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel11.Show();
        }

        //buttons
        private void button8_Click(object sender, EventArgs e)
        {
            string newUserName = textBox1.Text;

            try
            {
                if (!newUserName.Any(char.IsDigit))
                {
                    MessageBox.Show("Username must contain at least one number");

                }
                else if (newUserName == null)
                {
                    MessageBox.Show("Input a username");
                }
                else
                {
                    if (newUserName != label7.Text)
                    {
                        if (MessageBox.Show("Are you sure you want to change username?", "Change UserName", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            con.Open();
                            SqlCommand cmd = new SqlCommand("exec dbo.sp_updateUserName '" + _ID + "','" + newUserName + "'", con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Username Updated, Hello " + newUserName);
                            label3.Text = newUserName;
                            label7.Text = newUserName;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Similar UserName");
                    }
                } 
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string pass = textBox2.Text, checkPass = textBox3.Text;
            try
            {
                if(pass != null)
                {
                    if (pass.Length < 8)
                    {
                        MessageBox.Show("Password should be at least 8 characters long");
                        return;
                    }

                    if (pass == checkPass && pass != _password)
                    {
                        if (MessageBox.Show("Are you sure you want to change username?", "Change Password", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            con.Open();
                            SqlCommand cmd = new SqlCommand("exec dbo.sp_updatePassword '" + _ID + "','" + pass + "'", con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Password Updated");
                        }
                    }
                    else if (pass == _password)
                    {
                        MessageBox.Show("Your password is your current password");
                    }
                    else
                    {
                        MessageBox.Show("Password is not similar");
                    }
                }
                else
                {
                    MessageBox.Show("Input a password");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        //logout
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to Log out?", "Log out", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Login login = new Login();
                    this.Hide();
                    login.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close() ;
            }
        }

        //for pass
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox3.UseSystemPasswordChar = false;
                textBox2.UseSystemPasswordChar = false;
                label10.Text = _password;
            }
            else
            {
                textBox3.UseSystemPasswordChar = true;
                textBox2.UseSystemPasswordChar = true;
                label10.Text = "................................................";
            }
        }

        //hide panel
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            panel9.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            panel11.Hide();
        }

        //home
        private void pictureBox1_Click(object sender, EventArgs e)
        {

            Main main = new Main(_ID, _userName, _password);
            this.Hide();
            main.Show();

        }
        //unused
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        
    }
}
