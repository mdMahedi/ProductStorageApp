using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;



namespace ProductStorageApp
{
    public partial class productStorageUI : Form
    {
        public productStorageUI()
        {
            InitializeComponent();
        }
        Products pr=new Products();

        private int id;
        private string des;
        private int qty;

        public string connectionString = ConfigurationManager.ConnectionStrings["productDBConn"].ConnectionString; 
        private void saveButton_Click(object sender, EventArgs e)
        {
            pr.code = id;
            pr.description = des;
            pr.quantity = qty;
            
            if (IsNull() == true)
            {
                MessageBox.Show("Don't Input Null value...");
            }
            else
            {
                id = Convert.ToInt32(codeTextBox.Text);
                des = descriptionTextBox.Text;
                qty = Convert.ToInt32(quantityTextBox.Text);

                if (IsIdValidated())
                {
                    MessageBox.Show("Code must be minumum 3 character...");
                }
                else
                {
                    if (Convert.ToInt32(quantityTextBox.Text) < 0)
                    {
                        MessageBox.Show("Don't input negative value...");
                    }
                    else
                    {
                        if (IsExistCode(id))
                        {
                            if (UpdateData(des, qty, id))
                            {
                                codeTextBox.Clear();
                                descriptionTextBox.Clear();
                                quantityTextBox.Clear();
                                MessageBox.Show("Data updated...");
                            }
                            else
                            {
                                MessageBox.Show("Data not updated...");
                            }
                        }
                        else
                        {
                            SqlConnection connection = new SqlConnection(connectionString);
                            string query = "Insert Into dbo.Products Values ('" + id + "','" + des + "','" + qty + "')";
                            SqlCommand command = new SqlCommand(query, connection);
                            connection.Open();
                            if (command.ExecuteNonQuery() > 0)
                            {
                                codeTextBox.Clear();
                                descriptionTextBox.Clear();
                                quantityTextBox.Clear();
                                MessageBox.Show("Insert succesfully");
                                ShowAllProduct();
                            }
                            else
                            {
                                MessageBox.Show("Not inserted!!!");
                            }
                        }
                    }
                }
            }
        }
        public bool IsNull()
        {
            if (codeTextBox.Text == "" || descriptionTextBox.Text == "" || quantityTextBox.Text == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsIdValidated()
        {
            if (codeTextBox.Text.Length < 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool nonNumberEntered = false;
        private void Check_Key_Down(object sender, KeyEventArgs e)
        {
            nonNumberEntered = false;
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    if (e.KeyCode != Keys.Back)
                    {
                        nonNumberEntered = true;
                    }
                }
            }
        }
        private void Check_Key_Press(object sender, KeyPressEventArgs e)
        {
            if (nonNumberEntered == true)
            {
                MessageBox.Show("Please enter numeric value only...");
                e.Handled = true;
            }
        }
        public bool IsExistCode(int id)
        {
            bool isExistingCode;
            int val=0;
            SqlConnection connection=new SqlConnection(connectionString);
            string query = "select code from Products where code='"+id+"'";
            SqlCommand command=new SqlCommand(query,connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                val = Convert.ToInt32(reader["code"]);
            }
            if (val==id)
            {
                isExistingCode = true;
            }
            else
            {
                isExistingCode = false;
            }
            return isExistingCode;
        }
        public bool UpdateData(string desc, int qty, int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Update dbo.Products set [description]='" + desc + "', [quantity]=[quantity]+'" + qty + "' Where code=" + id;
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            if (command.ExecuteNonQuery() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void showButton_Click(object sender, EventArgs e)
        {
            ShowAllProduct();
        }

        public void ShowAllProduct()
        {
            int total = 0;
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT * FROM [ProductDB].[dbo].[Products]";
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Products> ProductList = new List<Products>();
            while (reader.Read())
            {
                Products products = new Products();
                products.code = Convert.ToInt32(reader["code"].ToString());
                products.description = reader["description"].ToString();
                products.quantity = Convert.ToInt32(reader["quantity"].ToString());
                total += Convert.ToInt32(reader["quantity"]);
                ProductList.Add(products);
            }
            reader.Close();
            connection.Close();
            LoadProductList(ProductList);
            totalAmountTextBox.Text = total.ToString();
        }

        public void LoadProductList(List<Products>productses )
        {
            displayListView.Items.Clear();
            foreach (var p in productses)
            {
                ListViewItem item=new ListViewItem(p.code.ToString());
                item.SubItems.Add(p.description);
                item.SubItems.Add(p.quantity.ToString());
                displayListView.Items.Add(item);
            }
        }

        private void productStorageUI_Load(object sender, EventArgs e)
        {
            ShowAllProduct();
        }
    }
}
