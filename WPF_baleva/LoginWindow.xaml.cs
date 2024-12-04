using Baleva_bd_WPF;
using System;
using System.Collections.Generic;
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

using System.Data.SqlClient;
namespace WPF_baleva
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        database RPM = new database();
        public LoginWindow()
        {
            InitializeComponent();
        }
        public string Username { get; set; }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Username = UsernameTextBox.Text; //сохраняем значение логина
            string password = PasswordBox.Password; // и пароля
            var (isAuthenticated, role) = AuthenticateUser(Username, password); // кортеж значений

            if (isAuthenticated) // если вернуло true
            {
                OpenRoleBaseWindow(role); //открываем окно соотв. роли
                MessageBox.Show("Вход выполнен успешно!", "Уведомление");
                this.Close(); // Закрыть окно авторизации
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль.", "Ошибка");
            }
        }


        private void OpenRoleBaseWindow(string role)
        {
            Window windowToOpen; // экземпляр окна
            switch (role) // идем по роли
            {
                case "admin":
                    windowToOpen = new AdminWindow(Username); // сохраняем админское окно, передаем логин для отображения                     
                    break;
                case "user":
                   
                    windowToOpen = new MainWindow();
            
                    break;
                default:
                    MessageBox.Show("Неизвестный пользователь", "Предупреждение");
                    return;
            }
            windowToOpen.Show(); // открываем соотв. окно
        }




        private (bool isAuthenticated, string role) AuthenticateUser(string username, string password)
        {
            RPM.OpenConnection();
            string query = "SELECT roles FROM Logins WHERE Logins = @Username AND pass = @Password"; // запрос ищет роль по совпадению логина и пароля

            using (SqlCommand command = new SqlCommand(query, RPM.GetConnection()))
            {
                // добавляем параметры для поиска
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                var role = command.ExecuteScalar() as string; // роль сохраняем строкой
                return (role != null, role); // если не null возвращаем ее
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            string login = UsernameTextBox.Text; //сохранили значение логина
            string password = PasswordBox.Password; // и пароля
            RPM.OpenConnection();
            string checkUserQuery = "Select count(*) from logins where Logins = @Login"; //проверка существует ли такой пользователь
            SqlCommand cmd = new SqlCommand(checkUserQuery, RPM.GetConnection());
            cmd.Parameters.AddWithValue("@Login", login); // добавляем параметр для поиска
            int userExists = (int)cmd.ExecuteScalar(); // сохраняем результат запроса (кол-во найденных по логину пользователей)
            if (userExists > 0) // если их больше 0, значит такой логин уже занят
            {
                MessageBox.Show("Пользователь с таким логином уже существует.", "Ошибка");
                return;
            }
            else // иначе процесс регистрации
            {
                int userId = random.Next(10, 200); // рандомом определяем id нового пользователя
                string insertUserQuery = "insert into Logins (ID, Logins, pass, roles) " +
                    "values (@userId, @login, @password, @role)"; // запрос на добавление нового пользователя с параметрами
                SqlCommand inserCmd = new SqlCommand(insertUserQuery, RPM.GetConnection()); // создаем команду
                                                                                           // добавляем параметры для добавления
                inserCmd.Parameters.AddWithValue("@userId", userId);
                inserCmd.Parameters.AddWithValue("@login", login);
                inserCmd.Parameters.AddWithValue("@password", password);
                inserCmd.Parameters.AddWithValue("@role", SelectedRole);

                int rowsAffected = inserCmd.ExecuteNonQuery(); // сам запрос работает, здесь сохраняем количество новых строк
                if (rowsAffected > 0) MessageBox.Show("Регистрация прошла успешно!", "Успех"); // строк больше 0 - добавили
                else MessageBox.Show("Произошла ошибка при регистрации", "Ошибка"); // иначе ошибка
            }
            RPM.CloseConnection();
        }

        public string SelectedRole { get; set; }
        private void TypeAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // по выбранной роли в combobox получаем индекс
            switch (TypeAccount.SelectedIndex)
            {
                case 0: //если выбран первый элемент из списка 
                    SelectedRole = "admin"; // значит админ
                    break;
                case 1:
                    SelectedRole = "user"; // иначе пользователь
                    break;
                default:
                    break;
            }
        }


    }
}
