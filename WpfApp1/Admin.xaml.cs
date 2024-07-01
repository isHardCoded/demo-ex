using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        DataBase db = new DataBase();

        public Admin()
        {
            InitializeComponent();
            ShowData();
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            SearchData();
        }

        public void SearchData()
        {
            string query = $"SELECT * FROM Data WHERE CONCAT (id_app, equip, date, description, name, status, type) LIKE'%{SearchText.Text}%'";
            
            try
            {
                db.openConnection();
                SqlCommand cmd = new SqlCommand(query, db.GetConnection());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    Data.ItemsSource = dt.DefaultView;
                }

                else
                {
                    MessageBox.Show("Ничего не найдено");
                    db.closeConnection();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения базы данных. Ошибка: {ex.Message}");
            }
        }

        public void ShowData()
        {
            string query = "SELECT * FROM Data INNER JOIN Breakings ON Data.type = Breakings.id_breaking";

            try
            {
                db.openConnection();

                SqlCommand cmd = new SqlCommand(query, db.GetConnection());
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    Data.ItemsSource = dt.DefaultView;
                }

                else
                {
                    MessageBox.Show("Заявок не найдено.");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения базы данных. Ошибка: {ex.Message}");
                db.closeConnection();
            }
        }
    }
}
