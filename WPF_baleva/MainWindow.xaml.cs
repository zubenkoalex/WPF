using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_baleva;
using static Azure.Core.HttpHeader;
using static Baleva_bd_WPF.database;

namespace Baleva_bd_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        database RPM = new database();
        public MainWindow()
        {
            InitializeComponent();
            LoadDataHeaders();
        }

        private void LoadDataHeaders()
        {
            List<Автомобиль> autos = new List<Автомобиль>(); // лист типа Автомобиль для сохранения значений из БД
            HashSet<string> Marks = new HashSet<string>();

            try
            {
                RPM.OpenConnection();
                string query = "SELECT \r\n    a.ID AS 'Автомобиль_ID',\r\n    m.Название_марки AS 'Марка',\r\n    mo.Название_модели AS 'Модель',\r\n    p.Название_поколения AS 'Поколение',\r\n    k.Тип_топлива,\r\n    k.Объем_двигателя,\r\n    k.Мощность_двигателя,\r\n    k.Тип_КПП,\r\n    k.Тип_привода,\r\n    k.Тип_кузова,\r\n    k.Цвет_кузова,\r\n    k.Руль,\r\n    k.Название_комплектации,\r\n    a.Пробег,\r\n    a.Цена,\r\n    a.Год_выпуска,\r\n    a.Изображение\r\nFROM \r\n    Автомобиль a\r\nINNER JOIN \r\n    Марка m ON a.Марка_ID = m.ID\r\nINNER JOIN \r\n    Модель mo ON m.Модель_ID = mo.ID\r\nINNER JOIN \r\n    Поколение p ON mo.Поколение_ID = p.ID\r\nINNER JOIN \r\n    Комплектация k on a.ID = k.ID";

                SqlCommand cmd = new SqlCommand(query, RPM.GetConnection());

                

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) // пока есть что считывать
                {
                    autos.Add(new Автомобиль // добавляем в список элемент типа Автомобиль
                    {
                       
                        Марка = reader.GetString(1), // переменные класса Автомобиль
                      
                    });
                    Marks.Add(reader.GetString(1)); // добавляем марку в HashSet
                }

                MarkaCB.ItemsSource = Marks; // добавляем уникальные марки в комбобокс

                autogrid.ItemsSource = RPM.SelectData("select * from SelectJoinAuto").DefaultView; // жизнь теперь максимально легка и прекрасна
                RPM.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void MarkaCB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (MarkaCB.SelectedItem != null)
            {
                string selectedmarks = MarkaCB.SelectedItem.ToString();
                LoadCerbyMarks(selectedmarks);
            }
        }
        private void LoadCerbyMarks(string marks)
        {
            string query = $"select * from SelectJoinAuto WHERE Марка LIKE '%{marks}%'";
            List<Автомобиль> autoss = new List<Автомобиль>(); // Предположим, у вас есть класс Book
            autoss.Clear();
            autogrid.ItemsSource = RPM.SelectData(query).DefaultView;
        }
       
        private void SearchTb_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            RPM.OpenConnection();
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT \r\n    a.ID AS 'Автомобиль_ID',\r\n    m.Название_марки AS 'Марка',\r\n    mo.Название_модели AS 'Модель',\r\n    p.Название_поколения AS 'Поколение',\r\n    k.Тип_топлива,\r\n    k.Объем_двигателя,\r\n    k.Мощность_двигателя,\r\n    k.Тип_КПП,\r\n    k.Тип_привода,\r\n    k.Тип_кузова,\r\n    k.Цвет_кузова,\r\n    k.Руль,\r\n    k.Название_комплектации,\r\n    a.Пробег,\r\n    a.Цена,\r\n    a.Год_выпуска,\r\n    a.Изображение\r\nFROM \r\n    Автомобиль a\r\nINNER JOIN \r\n    Марка m ON a.Марка_ID = m.ID\r\nINNER JOIN \r\n    Модель mo ON m.Модель_ID = mo.ID\r\nINNER JOIN \r\n    Поколение p ON mo.Поколение_ID = p.ID\r\nINNER JOIN \r\n    Комплектация k on a.ID = k.ID \r\n where concat" +
        "(a.ID, a.Год_выпуска) LIKE '%" + SearchTb.Text + "%'", RPM.GetConnection());
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable); // меняем содержимое таблицы
            autogrid.ItemsSource = dataTable.DefaultView; // обновляем ее
            RPM.CloseConnection();
        }

     
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}

