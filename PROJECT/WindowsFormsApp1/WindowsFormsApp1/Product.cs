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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{

    public partial class Product : Form
    {
        SqlConnection con = new SqlConnection("Data Source=GENE\\SQLEXPRESS01;Initial Catalog=BeneAmber;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private string _ID;
        private string _userName;
        private string _password;

        public Product(string ID, string userName, string password)
        {
            InitializeComponent();
            _ID = ID;
            _userName = userName;
            _password = password;
        }

        private void Product_Load(object sender, EventArgs e)
        {
            loadRecordsBrand();
            loadRecordsCategory();
            loadRecordsPrices();
            loadRecordsConditions();
            loadRecordsQuality();
            loadRecordsInventory();
            loadRecordsProduct();
            totalCostPrice();
            totalProfit();
            totalQuantity();
            displayProdIDcombo();
            displayBrandIDcombo();
            displayCategoryIDcombo();
            displayCondIDcombo();
        }

        //exit
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //form buttons
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer customer = new Customer(_ID,_userName, _password);
            customer.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            loadRecordsBrand();
            loadRecordsCategory();
            loadRecordsPrices();
            loadRecordsConditions();
            loadRecordsQuality();
            loadRecordsInventory();
            loadRecordsProduct();
            displayBrandIDcombo();
            displayCategoryIDcombo();
            clearAll();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Transaction transaction = new Transaction(_ID, _userName, _password);
            transaction.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            user_settings user = new user_settings(_ID, _userName, _password);
            user.Show();
            this.Hide();
        }

        //Reload or Refresh classes
        private void loadRecordsBrand()
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewBrand", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void loadRecordsCategory()
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewCat", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void loadRecordsPrices()
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewPrice", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView4.DataSource = dt;
        }

        private void loadRecordsConditions()
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewCondition", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView5.DataSource = dt;
        }

        private void loadRecordsQuality()
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewQuality", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView6.DataSource = dt;
        }

        private void loadRecordsInventory0Stock()
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewInventory0", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView3.DataSource = dt;
        }

        private void loadRecordsInventory()
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_viewInventory", con);
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
            dataGridView7.DataSource = dt;
        }
        
        //For combo boxes

        private void displayCondIDcombo()
        {
            SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_displayCond", con);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = sqlCommand;
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            comboBox6.DataSource = dataTable;
            comboBox6.DisplayMember = "Condition";
            comboBox6.ValueMember = "ConditionID";
        }

        private void displayProdIDcombo()
        {
            SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_displayProdID", con);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = sqlCommand;
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            comboBox3.DataSource = dataTable;
            comboBox3.DisplayMember = "Product";
            comboBox3.ValueMember = "ProductID";
            comboBox4.DataSource = dataTable;
            comboBox4.DisplayMember = "Product";
            comboBox4.ValueMember = "ProductID";
            comboBox5.DataSource = dataTable;
            comboBox5.DisplayMember = "Product";
            comboBox5.ValueMember = "ProductID";
        }

        private void displayBrandIDcombo()
        {
            SqlCommand sqlCommand = new SqlCommand("exec sp_displayBrandName", con);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = sqlCommand;
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            comboBox1.DataSource = dataTable;
            comboBox1.DisplayMember = "BrandName";
            comboBox1.ValueMember = "BrandID";
        }

        private void displayCategoryIDcombo()
        {
            SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_displayCatName", con);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = sqlCommand;
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            comboBox2.DataSource = dataTable;
            comboBox2.DisplayMember = "CategoryName";
            comboBox2.ValueMember = "CategoryID";
        }

        //Insert
        private void button5_Click(object sender, EventArgs e)
        {
            String BrandID = textBox1.Text, BrandName = textBox8.Text;
            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("exec dbo.sp_insertBrand '" + int.Parse(BrandID) + "','" + BrandName + "'", con);
                command.ExecuteNonQuery();
                MessageBox.Show("Brand Added");
                loadRecordsBrand();
                displayBrandIDcombo();
                clearAll();
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

        private void button10_Click(object sender, EventArgs e)
        {
            String CatID = textBox2.Text, CatName = textBox9.Text;
            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("exec dbo.sp_insertCategory '" + int.Parse(CatID) + "','" + CatName + "'", con);
                command.ExecuteNonQuery();
                MessageBox.Show("Category Added");
                loadRecordsCategory();
                displayCategoryIDcombo();
                clearAll();
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

        private void button17_Click(object sender, EventArgs e)
        {
            String CondID = textBox18.Text, ConDesc = textBox17.Text;
            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("exec dbo.sp_insertCond '" + int.Parse(CondID) + "','" + ConDesc+ "'", con);
                command.ExecuteNonQuery();
                MessageBox.Show("Condition Added");
                loadRecordsConditions();
                displayCondIDcombo();
                clearAll();
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

        private void button25_Click(object sender, EventArgs e)
        {
            String ProdID = textBox7.Text, ProdName = textBox5.Text;
            int BrandID = Convert.ToInt32(comboBox1.SelectedValue);
            int CatID = Convert.ToInt32(comboBox2.SelectedValue);
            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("exec dbo.sp_insertProduct '" + int.Parse(ProdID) + "','" + ProdName + "','" + BrandID + "','" + CatID + "'", con);
                command.ExecuteNonQuery();
                MessageBox.Show("Product Added");
                loadRecordsProduct();
                displayProdIDcombo();
                displayBrandIDcombo();
                displayCategoryIDcombo();
                clearAll();
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

        private void button16_Click(object sender, EventArgs e)
        {
            string ProdID = comboBox5.Text;
            string sellPrice = textBox11.Text;
            string costPrice = textBox10.Text;
            string profit = textBox12.Text;

            try
            {
                con.Open();

                // Use parameters to prevent SQL injection
                SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_insertPrice @ProdID, @SellPrice, @CostPrice, @Profit", con);
                sqlCommand.Parameters.AddWithValue("@ProdID", int.Parse(ProdID));
                sqlCommand.Parameters.AddWithValue("@SellPrice", decimal.Parse(sellPrice));
                sqlCommand.Parameters.AddWithValue("@CostPrice", decimal.Parse(costPrice));
                sqlCommand.Parameters.AddWithValue("@Profit", decimal.Parse(profit));

                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Price Added");
                loadRecordsPrices();
                totalCostPrice();
                totalProfit();
                clearAll();
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

        private void button13_Click(object sender, EventArgs e)
        {
            String ProdID = comboBox3.Text, quantity = textBox13.Text;
            try
            {
                con.Open();
                SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_insertInventory '" + int.Parse(ProdID) + "','" + int.Parse(quantity)+ "'", con);
                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Stock Added");
                loadRecordsInventory();
                totalQuantity();
                clearAll();
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

        private void button20_Click(object sender, EventArgs e)
        {
            string productID = comboBox4.Text;
            string conditionID = comboBox6.Text;
            string quality = "";

            if (radioButton1.Checked)
            {
                quality = "poor";
            }
            else if (radioButton2.Checked)
            {
                quality = "good";
            }
            else if (radioButton3.Checked)
            {
                quality = "very good";
            }
            else if (radioButton4.Checked)
            {
                quality = "excellent";
            }

            try
            {
                con.Open();

                SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_insertQuality @ProductID, @ConditionID, @Quality", con);
                sqlCommand.Parameters.AddWithValue("@ProductID", int.Parse(productID));
                sqlCommand.Parameters.AddWithValue("@ConditionID", int.Parse(conditionID));
                sqlCommand.Parameters.AddWithValue("@Quality", quality);

                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Quality Added");
                loadRecordsQuality();
                clearAll();
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

        //Edit
        private void button6_Click(object sender, EventArgs e)
        {
            String BrandID = textBox1.Text, BrandName = textBox8.Text;
            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("exec dbo.sp_editBrand '" + int.Parse(BrandID) + "','" + BrandName + "'", con);
                command.ExecuteNonQuery();
                MessageBox.Show("Brand Updated");
                loadRecordsBrand();
                displayBrandIDcombo();
                clearAll();
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

        private void button9_Click(object sender, EventArgs e)
        {
            String CatID = textBox2.Text, CatName = textBox9.Text;
            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("exec dbo.sp_editCat '" + int.Parse(CatID) + "','" + CatName + "'", con);
                command.ExecuteNonQuery();
                MessageBox.Show("Category Updated");
                loadRecordsCategory();
                displayCategoryIDcombo();
                clearAll();
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

        private void button19_Click(object sender, EventArgs e)
        {
            String CondID = textBox18.Text, ConDesc = textBox17.Text;
            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("exec dbo.sp_editCondition '" + int.Parse(CondID) + "','" + ConDesc + "'", con);
                command.ExecuteNonQuery();
                MessageBox.Show("Condition Updated");
                loadRecordsConditions();
                displayCondIDcombo();
                clearAll();
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

        private void button24_Click(object sender, EventArgs e)
        {
            String ProdID = textBox7.Text, ProdName = textBox5.Text;
            int BrandID = Convert.ToInt32(comboBox1.SelectedValue);
            int CatID = Convert.ToInt32(comboBox2.SelectedValue);
            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("exec dbo.sp_editProduct '" + int.Parse(ProdID) + "','" + ProdName + "','" + BrandID + "','" + CatID + "'", con);
                command.ExecuteNonQuery();
                MessageBox.Show("Product Updated");
                loadRecordsProduct();
                displayProdIDcombo();
                displayBrandIDcombo();
                displayCategoryIDcombo();
                clearAll();
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

        private void button15_Click(object sender, EventArgs e)
        {
            String ProdID = comboBox5.Text, sellPrice = textBox11.Text, costPrice = textBox10.Text, profit = textBox12.Text;
            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("exec dbo.sp_editPrice '" + int.Parse(ProdID) + "','" + decimal.Parse(sellPrice) + "','" + decimal.Parse(costPrice) + "','" + decimal.Parse(profit) + "'", con);
                command.ExecuteNonQuery();
                MessageBox.Show("Price Updated");
                loadRecordsPrices();
                totalCostPrice();
                totalProfit();
                clearAll();
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

        private void button12_Click(object sender, EventArgs e)
        {
            String ProdID = comboBox3.Text, quantity = textBox13.Text;
            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("exec dbo.sp_editInventory '" + int.Parse(ProdID) + "','" + int.Parse(quantity) + "'", con);
                command.ExecuteNonQuery();
                MessageBox.Show("Inventory Updated");
                loadRecordsInventory();
                totalQuantity();
                clearAll();
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

        private void button22_Click(object sender, EventArgs e)
        {
            string productID = comboBox4.Text;
            string conditionID = comboBox6.Text;
            string quality = "";

            if (radioButton1.Checked)
            {
                quality = "poor";
            }
            else if (radioButton2.Checked)
            {
                quality = "good";
            }
            else if (radioButton3.Checked)
            {
                quality = "very good";
            }
            else if (radioButton4.Checked)
            {
                quality = "excellent";
            }

            try
            {
                con.Open();
                SqlCommand command = new SqlCommand("exec dbo.sp_editQuality'" + int.Parse(productID) + "','" + int.Parse(conditionID) + "','" + quality + "'", con);
                command.ExecuteNonQuery();
                MessageBox.Show("Quality Updated");
                loadRecordsQuality();
                clearAll();
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

        //Delete
        private void button18_Click(object sender, EventArgs e)
        {
            String ConditionID = textBox18.Text;
            try
            {
                if (MessageBox.Show("Are you sure you want to delete a Condition?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_deleteCondition '" + int.Parse(ConditionID) + "'", con);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Condition Deleted");
                    loadRecordsConditions();
                    displayCondIDcombo();
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

        private void button7_Click(object sender, EventArgs e)
        {
            String BrandID = textBox1.Text;
            try
            {
                if (MessageBox.Show("Are you sure you want to delete a Brand?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_deleteBrand '" + int.Parse(BrandID) + "'", con);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Brand Deleted");
                    loadRecordsBrand();
                    displayBrandIDcombo();
                    clearAll();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: ", ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            string productID = comboBox4.Text;

            try
            {
                if (MessageBox.Show("Are you sure you want to delete a Quality?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_deleteQuality '" + int.Parse(productID) + "'", con);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Quality Deleted");
                    loadRecordsQuality();
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

        private void button8_Click(object sender, EventArgs e)
        {
            String CatID = textBox2.Text;
            try
            {
                if (MessageBox.Show("Are you sure you want to delete a Category?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_deleteCat '" + int.Parse(CatID) + "'", con);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Category Deleted");
                    loadRecordsCategory();
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

        private void button23_Click(object sender, EventArgs e)
        {
            String ProdID = textBox7.Text;
            try
            {
                if (MessageBox.Show("Are you sure you want to delete a Product?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_deleteProd '" + int.Parse(ProdID) + "'", con);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Product Deleted");
                    loadRecordsProduct();
                    displayProdIDcombo();
                    displayBrandIDcombo();
                    displayCategoryIDcombo();
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

        private void button14_Click(object sender, EventArgs e)
        {
            String ProdID = comboBox5.Text;
            try
            {
                if (MessageBox.Show("Are you sure you want to delete a price?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_deletePrice '" + int.Parse(ProdID) + "'", con);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Price Deleted");
                    loadRecordsPrices();
                    totalCostPrice();
                    totalProfit();
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

        private void button11_Click(object sender, EventArgs e)
        {
            String ProdID = comboBox3.Text;
            try
            {
                if (MessageBox.Show("Are you sure you want to delete stock?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    con.Open();
                    SqlCommand sqlCommand = new SqlCommand("exec dbo.sp_deleteInventory '" + int.Parse(ProdID) + "'", con);
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Inventory Deleted");
                    loadRecordsPrices();
                    totalQuantity();
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

        private void clearAll()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox5.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox17.Clear();
            textBox18.Clear();
        }

        //Load
        private void button27_Click(object sender, EventArgs e)
        {
            loadRecordsBrand();
            loadRecordsCategory();
            loadRecordsPrices();
            loadRecordsConditions();
            loadRecordsQuality();
            loadRecordsInventory();
            loadRecordsProduct();
            displayProdIDcombo();
            displayBrandIDcombo();
            displayCategoryIDcombo();
            totalCostPrice();
            totalProfit();
        }

        //Formula
        private void textBox12_TextChanged_1(object sender, EventArgs e)
        {
            

        }

        private void textBox10_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                string sellPrice = textBox11.Text, costPrice = textBox10.Text;
                decimal sellPrices = decimal.Parse(sellPrice);
                decimal costPrices = decimal.Parse(costPrice);
                decimal totPrices = sellPrices - costPrices;
                string totPrice = totPrices.ToString();

                textBox12.Text = totPrice;
            }
            catch (FormatException)
            {
                textBox12.Text = "Invalid Quantity";
            }
        }
        private void totalCostPrice()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("sp_totalCostPrice", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                var result = sqlCommand.ExecuteScalar();

                if (result != null)
                {
                    label27.Text = result.ToString();
                }
                else
                {
                    label27.Text = "No data";
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
                    label29.Text = result.ToString();
                }
                else
                {
                    label29.Text = "No data";
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

        private void totalQuantity()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand sqlCommand = new SqlCommand("sp_totalQuantity", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                var result = sqlCommand.ExecuteScalar();

                if (result != null)
                {
                    label25.Text = result.ToString();
                }
                else
                {
                    label25.Text = "No data";
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

        //search

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            string ID = textBox21.Text;

            if (int.TryParse(ID, out int parseID))
            {
                using (SqlCommand command = new SqlCommand("dbo.sp_searchBrand", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BrandID", parseID);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;
                    }
                    else
                    {
                        loadRecordsBrand();
                    }
                }
                using (SqlCommand command = new SqlCommand("dbo.sp_searchCat", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoryID", parseID);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        
                        dataGridView2.DataSource = dt;
                    }
                    else
                    {
                        loadRecordsCategory();
                    }
                }
                using (SqlCommand command = new SqlCommand("dbo.sp_searchQuality", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProdID", parseID);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {

                        dataGridView6.DataSource = dt;
                    }
                    else
                    {
                        loadRecordsQuality();
                    }
                }
                using (SqlCommand command = new SqlCommand("dbo.sp_searchConditions", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ConditionID", parseID);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {

                        dataGridView5.DataSource = dt;
                    }
                    else
                    {
                        loadRecordsConditions();
                    }
                }
                using (SqlCommand command = new SqlCommand("dbo.sp_searchPrice", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProdID", parseID);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {

                        dataGridView4.DataSource = dt;
                    }
                    else
                    {
                        loadRecordsPrices();
                    }
                }
                using (SqlCommand command = new SqlCommand("dbo.sp_searchProd", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProdID", parseID);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {

                        dataGridView7.DataSource = dt;
                    }
                    else
                    {
                        loadRecordsPrices();
                    }
                }
                using (SqlCommand command = new SqlCommand("dbo.sp_searchInventory", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProdID", parseID);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {

                        dataGridView3.DataSource = dt;
                    }
                    else
                    {
                        loadRecordsPrices();
                    }
                }
            }
        }

        //home
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Main main = new Main(_ID, _userName, _password);
            this.Hide();
            main.Show();
        }

        //PRINT
        private void button26_Click(object sender, EventArgs e)
        {
            if (dataGridView7 != null && dataGridView7.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = "Product_Inventory";
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
                            PdfPTable pTable = new PdfPTable(dataGridView7.Columns.Count);
                            pTable.DefaultCell.Padding = 2;
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn col in dataGridView7.Columns)
                            {
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));
                                pTable.AddCell(pCell);
                            }

                            foreach (DataGridViewRow viewRow in dataGridView7.Rows)
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
        private void button27_Click_1(object sender, EventArgs e)
        {
            if (dataGridView4 != null && dataGridView4.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf"; 
                save.FileName = "Product_Prices";
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
                            PdfPTable pTable = new PdfPTable(dataGridView4.Columns.Count);
                            pTable.DefaultCell.Padding = 2;
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn col in dataGridView4.Columns)
                            {
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));
                                pTable.AddCell(pCell);
                            }

                            foreach (DataGridViewRow viewRow in dataGridView4.Rows)
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

        private void button28_Click(object sender, EventArgs e)
        {
            if (dataGridView3 != null && dataGridView3.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";  
                save.FileName = "Product_InventoryQuantity";
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
                            PdfPTable pTable = new PdfPTable(dataGridView3.Columns.Count);
                            pTable.DefaultCell.Padding = 2;
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn col in dataGridView3.Columns)
                            {
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));
                                pTable.AddCell(pCell);
                            }

                            foreach (DataGridViewRow viewRow in dataGridView3.Rows)
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

        //OSTOCKS
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                loadRecordsInventory0Stock();

                if (checkBox2.Checked)
                {
                    viewInventory2();
                }
            }
            else
            {
                loadRecordsInventory();
            }
        }

        //shownames
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                viewProdwJoin();

                viewPrice1();

                viewInventory1();

                viewQuality2();

                if (checkBox1.Checked)
                {
                    viewInventory2();
                }
                else
                {
                    viewInventory1();
                }
            }
            else
            {
                loadRecordsBrand();
                loadRecordsCategory();
                loadRecordsPrices();
                loadRecordsConditions();
                loadRecordsQuality();
                loadRecordsInventory();
                loadRecordsProduct();
            }
        }

        private void viewQuality2()
        {
            SqlCommand command1 = new SqlCommand("exec dbo.sp_viewQuality2", con);
            SqlDataAdapter da1 = new SqlDataAdapter(command1);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            dataGridView6.DataSource = dt1;
        }
        private void viewProdwJoin()
        {
            SqlCommand command1 = new SqlCommand("exec dbo.sp_viewProdwJoin1", con);
            SqlDataAdapter da1 = new SqlDataAdapter(command1);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            dataGridView7.DataSource = dt1;
        }

        private void viewPrice1()
        {
            SqlCommand command2 = new SqlCommand("exec dbo.sp_viewPrice1", con);
            SqlDataAdapter da2 = new SqlDataAdapter(command2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView4.DataSource = dt2;
        }

        private void viewInventory1() {
            SqlCommand command3 = new SqlCommand("exec dbo.sp_viewInventory1", con);
            SqlDataAdapter da3 = new SqlDataAdapter(command3);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView3.DataSource = dt3;
        }

        private void viewInventory2()
        {
            SqlCommand command4 = new SqlCommand("exec dbo.sp_viewInventory2", con);
            SqlDataAdapter da4 = new SqlDataAdapter(command4);
            DataTable dt4 = new DataTable();
            da4.Fill(dt4);
            dataGridView3.DataSource = dt4;
        }

        //Unused
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
