using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class Main : Form
    {
        private string _ID;
        private string _userName;
        private string _password;

        public Main(string ID, string userName, string password)
        {
            InitializeComponent();
            _ID = ID;
            _userName = userName;
            _password = password;
        }

        SqlConnection con = new SqlConnection("Data Source=GENE\\SQLEXPRESS01;Initial Catalog=BeneAmber;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        private void Main_Load(object sender, EventArgs e)
        {
            loadRecordsRecProducts();
            mostStockedBrand();
            totalProfit();
            currentInv();
            loadRecordsProduct();
            loadRecordsCustomers();
        }

        //form buttons
        private void button1_Click_2(object sender, EventArgs e)
        {
            this.Hide();
            Product product = new Product(_ID,_userName,_password);
            product.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Customer customer = new Customer(_ID,_userName, _password);
            customer.Show();
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            Transaction trans = new Transaction(_ID, _userName, _password);
            this.Hide();
            trans.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            user_settings user_Settings = new user_settings(_ID, _userName, _password);
            this.Hide();
            user_Settings.Show();
        }

        //loader
        private void loadRecordsRecProducts()
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewProdwJoin", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void loadRecordsCustomers() {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewCustomer", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView3.DataSource = dt;
        }
        private void loadRecordsProduct()
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewProd", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        //formula
        private void totalProfit()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("sp_totalProfit", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                var result = sqlCommand.ExecuteScalar();

                if (result != null)
                {
                    label4.Text = "₱" + result.ToString();
                }
                else
                {
                    label4.Text = "No data";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void mostStockedBrand()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand("sp_FindMostStockedBrand", con);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    label9.Text = dt.Rows[0]["Most Stocked Brand"].ToString() + " - " + dt.Rows[0]["Total Quantity"].ToString();
                }
                else
                {
                    label9.Text = "No data found";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void currentInv()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("sp_sumInv", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                var result = sqlCommand.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    label8.Text = result.ToString() + " Items";
                }
                else
                {
                    label8.Text = "No data";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        //exit
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //unused
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void sidebar_Tick(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
