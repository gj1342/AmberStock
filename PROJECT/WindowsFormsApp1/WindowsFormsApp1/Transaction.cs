using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class Transaction : Form
    {
        private string _ID;
        private string _userName;
        private string _password;
        SqlConnection con = new SqlConnection("Data Source=GENE\\SQLEXPRESS01;Initial Catalog=BeneAmber;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        public Transaction(string ID, string userName, string password)
        {
            InitializeComponent();
            _ID = ID;
            _userName = userName;
            _password = password;

        }

        private void Transaction_Load(object sender, EventArgs e)
        {
            loadRecordTransac();
            loadRecordsInventory();
            loadRecordsCustomer();
        }

        //loader
        private void loadAll()
        {
            loadRecordTransac();
            loadRecordsInventory();
            loadRecordsCustomer();
        }
        private void loadRecordTransac()
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewTransac", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void loadRecordsInventory()
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewInventory", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        //buttons
        private void button2_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer(_ID,_userName, _password);
            customer.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Product product = new Product(_ID, _userName, _password);
            product.Show();
            this.Hide();
        }

        //refresh
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            loadAll();
            clearAll();
        }

        //clearAll
        private void clearAll()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            user_settings user_Settings = new user_settings(_ID, _userName, _password);
            user_Settings.Show();
            this.Hide();
        }

        //search
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string id = textBox5.Text;

            if (int.TryParse(id, out int parseID))
            {
                using (SqlCommand command = new SqlCommand("dbo.sp_searchTransac", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@transacID", parseID);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        dataGridView2.DataSource = dt;
                    }
                    else
                    {
                        loadAll();
                    }
                }
            }
        }

        private void loadRecordsCustomer()
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewCustomer", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView3.DataSource = dt;
        }

        //add
        private void button10_Click(object sender, EventArgs e)
        {
            string totQuantity = label19.Text;
            string transacID = textBox2.Text, productID = textBox3.Text, quantity = textBox1.Text, totalPrice = label18.Text, custID = textBox4.Text;
            DateTime transacDate = dateTimePicker1.Value;

            try
            {
                if (int.Parse(totQuantity) >= int.Parse(quantity) && int.Parse(quantity) > 0)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("exec dbo.sp_insertTransac '" + int.Parse(transacID) + "','" + int.Parse(productID) + "','" + transacDate + "','" + int.Parse(custID) + "','" + int.Parse(quantity) + "','" + decimal.Parse(totalPrice) + "'", con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Transaction Added");
                    loadRecordTransac();
                    loadRecordsInventory();
                    loadAll();
                    clearAll();
                }
                else
                {
                    MessageBox.Show("Invalid Quantity");
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

        //edit
        private void button9_Click(object sender, EventArgs e)
        {
            string transacID = textBox2.Text, productID = textBox3.Text, quantity = textBox1.Text, totalPrice = label18.Text, custID = textBox4.Text;
            DateTime transacDate = dateTimePicker1.Value;

            try
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand("select quantityBought from transac where transactionID = '" + transacID + "'", con);
                var qnty = sqlCommand.ExecuteScalar();

                if (qnty != null && int.Parse(qnty.ToString()) <= int.Parse(quantity) && int.Parse(quantity) > 0)
                {
                    if (int.Parse(label19.Text) >= int.Parse(quantity) )
                    {
                        SqlCommand command = new SqlCommand("exec dbo.sp_updateminTransac '" + int.Parse(transacID) + "','" + int.Parse(productID) + "','" + transacDate + "','" + int.Parse(custID) + "','" + int.Parse(quantity) + "','" + decimal.Parse(totalPrice) + "'", con);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Transaction Edited");
                        loadRecordTransac();
                        loadRecordsInventory();
                        loadAll();
                        clearAll();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Quantity");
                    }
                }
                else if (qnty != null && int.Parse(qnty.ToString()) >= int.Parse(quantity) && int.Parse(quantity) > 0)
                {
                    if (int.Parse(label19.Text) >= int.Parse(quantity) && int.Parse(quantity) > 0)
                    {
                        SqlCommand command = new SqlCommand("exec dbo.sp_updateaddTransac '" + int.Parse(transacID) + "','" + int.Parse(productID) + "','" + transacDate + "','" + int.Parse(custID) + "','" + int.Parse(quantity) + "','" + decimal.Parse(totalPrice) + "'", con);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Transaction Edited");
                        loadRecordTransac();
                        loadRecordsInventory();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Quantity");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Quantity");
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


        //delete
        private void button8_Click(object sender, EventArgs e)
        {
            string transacID = textBox2.Text;
            try
            {
                if (MessageBox.Show("Are you sure you want to delete a transaction?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_deleteTransac '" + int.Parse(transacID) + "'", con);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Transaction Deleted");
                    loadRecordTransac();
                    loadRecordsInventory();
                    loadAll();
                    clearAll();
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


        //PRINT
        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView2 != null && dataGridView2.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = "Transaction_Master_List";
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

        private void button5_Click(object sender, EventArgs e)
        {
            string transacID = textBox2.Text;
            try
            {
                if (MessageBox.Show("Are you sure you want to delete a transaction?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_deleteTransacOnly '" + int.Parse(transacID) + "'", con);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Transaction Deleted");
                    loadRecordTransac();
                    loadRecordsInventory();
                    loadAll();
                    clearAll();
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

        //text prodID
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string prodID = textBox3.Text;

            if (int.TryParse(prodID, out int parsedProdID))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand("dbo.sp_viewPricecbBox", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductID", parsedProdID);

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            label10.Text = dt.Rows[0]["Selling Price"].ToString();
                        }
                        else
                        {
                            label10.Text = "No data";
                        }
                    }
                    using (SqlCommand command = new SqlCommand("dbo.sp_viewQuantInv", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductID", parsedProdID);

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            label19.Text = dt.Rows[0]["Quantity"].ToString();
                        }
                        else
                        {
                            label19.Text = "No data";
                        }
                    }

                    using(SqlCommand command = new SqlCommand("dbo.sp_viewProdNameinTrans", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductID", parsedProdID);

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            label21.Text = dt.Rows[0]["Product Name"].ToString();
                        }
                        else
                        {
                            label21.Text = "No data";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                label10.Text = "Invalid Product ID";
            }
        }

        //text quantity
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string quan = textBox1.Text;
                decimal sellPrice = decimal.Parse(label10.Text);
                decimal totPrice = decimal.Parse(quan) * sellPrice;

                label18.Text = totPrice.ToString();
            }
            catch (FormatException)
            {
                label18.Text = "Invalid Quantity";
            }
        }

        //text customer
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
            try
            {
                string custID = textBox4.Text, custName = label4.Text;
                using (SqlCommand command = new SqlCommand("dbo.sp_viewCustomerinTrans", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerID", custID);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        label4.Text = dt.Rows[0]["Customer Name"].ToString();
                    }
                    else
                    {
                        label4.Text = "No data";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        //home
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Main main = new Main(_ID, _userName, _password);
            this.Hide();
            main.Show();
        }


        //show name
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                SqlCommand command = new SqlCommand("exec dbo.sp_viewCustNameTransac", con);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;

            }
            else {
                loadRecordTransac();
            }
        }

        //exit
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //unused
        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }
    }
}
