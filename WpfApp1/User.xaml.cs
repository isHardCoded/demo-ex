using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    public partial class User : Window
    {
        DataBase db = new DataBase();
        public User()
        {
            InitializeComponent();
        }

        // Авторизация и регистрация
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            AuthUser();
            ShowData();
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            RegUser();
        }

        private void SendToData_Click(object sender, RoutedEventArgs e)
        {
            SendToData();
            ShowData();
        }
        // Методы

        // Авторизация
        public void AuthUser()
        {
            string _login = login.Text;
            string _password = password.Text;
            string query = "SELECT * FROM Users WHERE login = @Login AND password = @Password";

            try
            {
                db.openConnection();

                SqlCommand cmd = new SqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@Login", _login);
                cmd.Parameters.AddWithValue("@Password", _password);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataBase.name = dt.Rows[0]["login"].ToString();
                    UserContent.Visibility = Visibility.Visible;
                    UserAuth.Visibility = Visibility.Collapsed;
                    AppData.Visibility = Visibility.Visible;
                }

                else
                {
                    MessageBox.Show("Такого пользователя не существует.");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения базы данных. Ошибка: {ex.Message}");
            }
        }

        // Регистрация
        public void RegUser()
        {
            string _login = RegLogin.Text;
            string _password = RegPassword.Text;
            string query = "INSERT INTO Users (login, password) VALUES (@Login, @Password)";

            try
            {
                db.openConnection();
                
                if (ValidateUser())
                {
                    SqlCommand cmd = new SqlCommand(query, db.GetConnection());
                    cmd.Parameters.AddWithValue("@Login", _login);
                    cmd.Parameters.AddWithValue("@Password", _password);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Вы были успешно зарегистрированы.");
                }
            }

            catch ( Exception ex )
            {
                MessageBox.Show($"Ошибка подключения базы данных. Ошибка: {ex.Message}");
            }
        }

        // Валидация
        public bool ValidateUser()
        {
            string _login = RegLogin.Text;
            string _password = RegPassword.Text;
            string query = "SELECT * FROM Users WHERE login = @Login AND password = @Password";

            try
            {
                db.openConnection();

                SqlCommand cmd = new SqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@Login", _login);
                cmd.Parameters.AddWithValue("@Password", _password);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Такой пользователь уже существует.");
                    return false;
                }

                else
                {
                    return true;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения базы данных. Ошибка: {ex.Message}");
                return false;
            }
        }

        // Добавление заявки

        public void SendToData()
        {
            string _equip = equip.Text;
            string _desc = desc.Text;
            var _typeList = typeList.SelectedIndex;
            DateTime _date = DateTime.Today;

            string query = "INSERT INTO Data (equip, date, description, type, name, status) VALUES (@Equip, @Date, @Desc, @Type, @Name, 'В ожидании')";

            try
            {
                db.openConnection();

                SqlCommand cmd = new SqlCommand(query, db.GetConnection());
                cmd.Parameters.AddWithValue("@Equip", _equip);
                cmd.Parameters.AddWithValue("@Date", _date);
                cmd.Parameters.AddWithValue("@Desc", _desc);
                cmd.Parameters.AddWithValue("@Type", _typeList + 1);
                cmd.Parameters.AddWithValue("@Name", DataBase.name);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Заявка успешна добавлена.");
            }
            
            catch ( Exception ex )
            {
                MessageBox.Show($"Ошибка подключения базы данных. Ошибка: {ex.Message}");
                db.closeConnection();
            }
        }

        // Вывод данных

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

        // Кнопки перехода
        private void MoveReg_Click(object sender, RoutedEventArgs e)
        {
            UserAuth.Visibility = Visibility.Collapsed;
            UserReg.Visibility = Visibility.Visible;
        }
    }
}
