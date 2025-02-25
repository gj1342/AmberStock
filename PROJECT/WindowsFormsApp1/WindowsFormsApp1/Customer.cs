using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class Customer : Form
    {
        private string _ID;
        private string _userName;
        private string _password;

        public Customer(string ID,string userName, string password)
        {
            InitializeComponent();
            _ID = ID;
            _userName = userName;
            _password = password;
        }

        SqlConnection con = new SqlConnection("Data Source=GENE\\SQLEXPRESS01;Initial Catalog=BeneAmber;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        private void Customer_Load(object sender, EventArgs e)
        {
            loadRecordsCustomer();
            loadRecordsCustwSpend();
            numCust();
        }

        //form buttons
        private void button1_Click(object sender, EventArgs e)
        {
            Product prod = new Product(_ID,_userName, _password);
            this.Hide();
            prod.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Transaction transaction = new Transaction(_ID,_userName, _password);
            transaction.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            user_settings user = new user_settings(_ID, _userName, _password);
            user.Show();
            this.Hide();
        }

        //loaders
        private void loadRecordsCustomer()
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewCustomer", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void loadRecordsCustwSpend()
        {
            SqlCommand command = new SqlCommand("exec sp_GetCustomerTotalSpend", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            loadRecordsCustomer();
            loadRecordsCustwSpend();
            clearAll();
        }

        //clear
        private void clearAll()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }
        //INSERT
        private void button5_Click(object sender, EventArgs e)
        {
            String custID = textBox2.Text, fname = textBox3.Text, lname = textBox4.Text, town = textBox5.Text, city= textBox1.Text;

            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("exec dbo.sp_insertCustomer '" + int.Parse(custID) + "','" + fname + "','" + lname + "','" + town + "','" + city +"'", con);
                command.ExecuteNonQuery();
                MessageBox.Show("Customer Added");
                loadRecordsCustomer();
                loadRecordsCustwSpend(); loadRecordsCustwSpend();
                numCust();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        //edit
        private void button6_Click(object sender, EventArgs e)
        {
            String custID = textBox2.Text, fname = textBox3.Text, lname = textBox4.Text, town = textBox5.Text, city = textBox1.Text;

            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("exec dbo.sp_editCustomer '" + int.Parse(custID) + "','" + fname + "','" + lname + "','" + town + "','" + city + "'", con);
                command.ExecuteNonQuery();
                MessageBox.Show("Customer Updated");
                loadRecordsCustomer();
                loadRecordsCustwSpend();
                numCust();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        //delete
        private void button7_Click(object sender, EventArgs e)
        {
            String custID = textBox2.Text;
            try
            {
                if (MessageBox.Show("Are you sure you want to delete a customer?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_deleteCustomer '" + int.Parse(custID) + "'", con);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Customer Removed");
                    loadRecordsCustomer();
                    loadRecordsCustwSpend();
                    numCust();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        //search
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string id = textBox6.Text;

            if (int.TryParse(id, out int parseID))
            {
                using (SqlCommand command = new SqlCommand("dbo.sp_searchCust", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", parseID);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;
                    }
                    else
                    {
                        loadRecordsCustomer();
                    }
                }
                using (SqlCommand command = new SqlCommand("dbo.sp_GetCustomerTotalSpendSearch", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", parseID);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        dataGridView2.DataSource = dt;
                    }
                    else
                    {
                        loadRecordsCustomer();
                    }
                }
            }
        }

        //formula
        private void numCust()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("countCustomer", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                var result = sqlCommand.ExecuteScalar();

                if (result != null)
                {
                    label3.Text = result.ToString();
                }
                else
                {
                    label3.Text = "No data";
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

        //home
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Main main = new Main(_ID, _userName, _password);
            this.Hide();
            main.Show();
        }

        //print
        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView1 != null && dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";  // Corrected the filter
                save.FileName = "Customers_Master_List";
                bool ErrorMessage = false;

                if (save.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(save.FileName))
                    {
                        try
                        {
                            File.Delete(save.FileName);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = true;
                            MessageBox.Show("Unable to write data to disk: " + ex.Message);
                        }
                    }
                    if (!ErrorMessage)
                    {
                        try
                        {
                            PdfPTable pTable = new PdfPTable(dataGridView1.Columns.Count);
                            pTable.DefaultCell.Padding = 2;
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn col in dataGridView1.Columns)
                            {
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));
                                pTable.AddCell(pCell);
                            }

                            foreach (DataGridViewRow viewRow in dataGridView1.Rows)
                            {
                                foreach (DataGridViewCell dCell in viewRow.Cells)
                                {
                                    if (dCell.Value != null)  // Check for null value
                                    {
                                        pTable.AddCell(dCell.Value.ToString());
                                    }
                                    else
                                    {
                                        pTable.AddCell("");  // Add an empty cell if the value is null
                                    }
                                }
                            }

                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                                PdfWriter.GetInstance(document, fileStream);
                                document.Open();
                                document.Add(pTable);
                                document.Close();
                                fileStream.Close();
                            }
                            MessageBox.Show("Data Exported Successfully");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while exporting: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No records found");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView2 != null && dataGridView2.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";  
                save.FileName = "CustomerWithSpend_List";
                bool ErrorMessage = false;

                if (save.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(save.FileName))
                    {
                        try
                        {
                            File.Delete(save.FileName);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = true;
                            MessageBox.Show("Unable to write data to disk: " + ex.Message);
                        }
                    }
                    if (!ErrorMessage)
                    {
                        try
                        {
                            PdfPTable pTable = new PdfPTable(dataGridView2.Columns.Count);
                            pTable.DefaultCell.Padding = 2;
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn col in dataGridView2.Columns)
                            {
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));
                                pTable.AddCell(pCell);
                            }

                            foreach (DataGridViewRow viewRow in dataGridView2.Rows)
                            {
                                foreach (DataGridViewCell dCell in viewRow.Cells)
                                {
                                    if (dCell.Value != null) 
                                    {
                                        pTable.AddCell(dCell.Value.ToString());
                                    }
                                    else
                                    {
                                        pTable.AddCell("");  
                                    }
                                }
                            }

                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                                PdfWriter.GetInstance(document, fileStream);
                                document.Open();
                                document.Add(pTable);
                                document.Close();
                                fileStream.Close();
                            }
                            MessageBox.Show("Data Exported Successfully");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while exporting: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No records found");
            }
        }

        //unused
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
