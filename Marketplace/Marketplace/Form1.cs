using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Marketplace
{
    public partial class Form1 : Form
    {
        private DataTable productTable;
        private ListBox cartListBox;
        private int selectedRowIndex = -1;
        public Form1()
        {
            
            InitializeComponent();
            InitializeProductTable();
            InitializeCartListBox();
            LoadDataFromFile("products.txt");
            textBox2.PasswordChar = '*';

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = true;
            button4.Visible = true;
       }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = true;
            string username = textBox1.Text;
            string password = textBox2.Text;
            bool isCustomer = checkBox2.Checked;
            button3.Visible = true;
            
        }

        private bool IsPasswordValid(string password)
        {
            // Проверка длины пароля
            if (password.Length < 8)
                return false;

            // Проверка на наличие пробелов
            if (password.Contains(" "))
                return false;

            // Проверка на наличие цифр и букв разного регистра
            bool hasUppercase = false;
            bool hasLowercase = false;
            bool hasDigit = false;
            foreach (char c in password)
            {
                if (char.IsUpper(c))
                    hasUppercase = true;
                else if (char.IsLower(c))
                    hasLowercase = true;
                else if (char.IsDigit(c))
                    hasDigit = true;
            }
            if (!hasUppercase || !hasLowercase || !hasDigit)
                return false;

            // Проверка на наличие специальных символов
            string specialChars = "!»№;@%;?:";
            foreach (char c in specialChars)
            {
                if (password.Contains(c))
                    return true;
            }
            return false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
            string login = textBox1.Text;
            string password = textBox2.Text;
            bool isSeller = checkBox2.Checked;
           

            // Проверка пользователя в файле
            if (IsUserInFile(login, password))
            {
                if (isSeller)
                {
                    // Пользователь является продавцом
                    MessageBox.Show("Вход выполнен успешно.");
                    tabControl1.SelectedIndex = 2; // Переход на вкладку 1
                }
                else
                {
                    // Пользователь является покупателем
                    MessageBox.Show("Вход выполнен успешно.");
                    tabControl1.SelectedIndex = 1; // Переход на вкладку 2
                }
            }
            else
            {
                // Показать сообщение о неверном логине/пароле и предложить зарегистрироваться
                DialogResult result = MessageBox.Show(
                    "Неверный логин или пароль. Хотите зарегистрироваться?",
                    "Ошибка входа",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    button3.Visible = false;
                    button4.Visible = true;
                }
            }
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            bool isSeller = checkBox2.Checked;
            // Проверка пароля на соответствие требованиям
            if (!IsPasswordValid(password))
            {
                MessageBox.Show("Пароль не соответствует требованиям.");
                return;
            }
            // Регистрация пользователя в файле
            SaveUserToFile(login, password, isSeller);

            MessageBox.Show("Регистрация выполнена успешно.");

            // Переход на соответствующую вкладку
            if (isSeller)
            {
                tabControl1.SelectedIndex = 2; // Переход на вкладку 1 (для продавца)
            }
            else
            {
                tabControl1.SelectedIndex = 1; // Переход на вкладку 2 (для покупателя)
            }
        }
           

        private void SaveUserToFile(string login, string password, bool isSeller)
        {
            string userInfo = $"{login}|{password}|{(isSeller ? "продавец" : "покупатель")}";
            File.AppendAllText("users.txt", userInfo + Environment.NewLine);
        }
        private bool IsUserInFile(string login, string password)
        {
            string[] lines = File.ReadAllLines("users.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts[0] == login && parts[1] == password)
                    return true;
            }
            return false;
        }

        private bool IsSeller(string login, string password)
        {
            string[] lines = File.ReadAllLines("users.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts[0] == login && parts[1] == password)
                    return parts[2] == "продавец";
            }
            return false;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
        private void InitializeProductTable()
        {
            productTable = new DataTable();
            productTable.Columns.Add("ID");
            productTable.Columns.Add("Name");
            productTable.Columns.Add("Price");
            productTable.Columns.Add("Quantity");
            productTable.Columns.Add("Warehouse");
            productTable.Columns.Add("ImagePath");

            dataGridView1.DataSource = productTable;
            dataGridView2.DataSource = productTable;
            dataGridView3.DataSource = productTable;
        }

        private void SaveToFile()
        {
            using (StreamWriter writer = new StreamWriter("products.txt", false)) // Режим добавления, а не перезаписи
            {
                foreach (DataRow dataRow in productTable.Rows)
                {
                    writer.WriteLine(string.Join(",", dataRow.ItemArray));
                }
            }
        }

        private void ClearInputs()
        {
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            pictureBox1.Image = null;
            textBox8.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string[] row = new string[]
            {
                textBox3.Text,
                textBox4.Text,
                textBox5.Text,
                textBox6.Text,
                textBox7.Text,
                textBox8.Text
            };

            productTable.Rows.Add(row);
            SaveToFile();
            ClearInputs();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                   textBox8.Text = openFileDialog.FileName;
                }
            }
        }
        private void LoadDataFromFile(string filePath)
        {
            productTable.Clear(); // Очистка текущих данных

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        productTable.Rows.Add(values);
                    }
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void товарыToolStripMenuItem_Click(object sender, EventArgs e)
        {// Загрузка данных в второй DataGridView
         // Загрузка данных в второй DataGridView
            LoadDataFromFile("products.txt");
            dataGridView1.DataSource = productTable;
            /*LoadDataFromFile("products.txt"); // Предполагается, что файл называется "products.txt"
            dataGridView2.DataSource = productTable;*/ // Привязка второго DataGridView к той же таблице // Привязка второго DataGridView к той же таблице

            /* LoadDataFromFileToDataGridView("products.txt");*/ // Предполагается, что файл называется "products.txt"

        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void button7_MouseDown(object sender, MouseEventArgs e)
        {

            textBox2.PasswordChar = '\0';
        }

        private void button7_MouseUp(object sender, MouseEventArgs e)
        {
            textBox2.PasswordChar = '*';
        }
        private void LoadDataFromFileToDataGridView(string filePath)
        {
            try
            {
                // Clear the DataGridView, but keep the existing columns
                dataGridView1.Rows.Clear();

                // Read data from the file
                string[] lines = File.ReadAllLines(filePath);

                // Add the data to the DataGridView
                foreach (string line in lines.Skip(1)) // Skip the header row
                {
                    string[] values = line.Split(',');
                    dataGridView1.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from file: {ex.Message}");
            };
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int selectedRowIndex = e.RowIndex;
                string productId = productTable.Rows[selectedRowIndex]["ID"].ToString();
                string productName = productTable.Rows[selectedRowIndex]["Name"].ToString();
                decimal price = decimal.Parse(productTable.Rows[selectedRowIndex]["Price"].ToString());

                int quantity = GetQuantityFromUser(productName, price);
                if (quantity > 0)
                {
                    if (ConfirmAddToCart(productName, quantity))
                    {
                        AddToCart(productName, quantity);
                    }
                }
            }
          
        }

        private int GetQuantityFromUser(string productName, decimal price)
        {
            string message = $"Введите количество для '{productName}' (Цена: {price})";
            string title = "Add to Cart";
            int quantity = (int)numericUpDown1.Value;

            DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                return quantity;
            }
            else
            {
                return 0;
            }
        }
        private bool ConfirmAddToCart(string productName, int quantity)
        {
            string message = $"Вы хотите добавить {quantity} '{productName}' в корзину?";
            string title = "Подтвердите добавление в корзину";
            DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private void AddToCart(string productName, int quantity)
        {
            listBox1.Items.Add($"{productName} x {quantity}");
        }

        private void InitializeCartListBox()
        {
            cartListBox = new ListBox();
            cartListBox.Dock = DockStyle.Fill;
            panel1.Controls.Add(cartListBox);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void корзинаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel1.Visible = true;
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void SearchProducts()
        {
           
            string nameFilter = textBox9.Text.ToLower();
            string manufacturerFilter = textBox10.Text.ToLower();
            string priceFilter = textBox11.Text;

            var filteredRows = productTable.AsEnumerable()
                .Where(row =>
                    (string.IsNullOrEmpty(nameFilter) || row.Field<string>("Name").ToLower().Contains(nameFilter)) &&
                    (string.IsNullOrEmpty(manufacturerFilter) || row.Field<string>("Warehouse").ToLower().Contains(manufacturerFilter)) &&
                    (string.IsNullOrEmpty(priceFilter) || row.Field<string>("Price").Contains(priceFilter)) // Фильтрация по строке
                );

            dataGridView1.DataSource = filteredRows.CopyToDataTable();

            dataGridView1.Refresh();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            SearchProducts();
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            SearchProducts();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            SearchProducts();
        }

        private void найтиТоварToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel4.Visible = true;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                selectedRowIndex = dataGridView1.SelectedRows[0].Index;
            }
            else
            {
                selectedRowIndex = -1;
            }
        }

        private void товарыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadDataFromFile("products.txt");
            dataGridView3.DataSource = productTable;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SaveToFile();
            MessageBox.Show("Данные успешно сохраненны!!");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }
    }
}
