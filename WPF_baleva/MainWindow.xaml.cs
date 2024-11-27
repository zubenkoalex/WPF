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
            HashSet<string> Marks = new HashSet<string>(); // переместили сюда

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
                        Автомобиль_ID = reader.GetInt32(0), // по индексам присваиваем значения в 
                        Марка = reader.GetString(1), // переменные класса Автомобиль
                        Модель = reader.GetString(2),
                        Поколение = reader.GetString(3),
                        Тип_топлива = reader.GetString(4),
                        Объем_двигателя = reader.GetString(5),
                        Мощность_двигателя = reader.GetInt32(6),
                        Тип_КПП = reader.GetString(7),
                        Тип_привода = reader.GetString(8),
                        Тип_кузова = reader.GetString(9),
                        Цвет_кузова = reader.GetString(10),
                        Руль = reader.GetString(11),
                        Название_комплектации = reader.GetString(12),
                        Пробег = reader.GetInt32(13),
                        Цена = reader.GetInt32(14),
                        Год_выпуска = reader.GetInt32(15),
                        Изображение = reader.GetString(16)
                    });
                    Marks.Add(reader.GetString(1)); // добавляем марку в HashSet
                }

                MarkaCB.ItemsSource = Marks; // добавляем уникальные марки в комбобокс
                autogrid.ItemsSource = autos; // передаем в datagrid получившийся список
                RPM.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SelectDB(string query, List<Автомобиль> autos, SqlCommand cmd)
        {

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) // пока есть что считывать
            {
                autos.Add(new Автомобиль // добавляем в список элемент типа Автомобиль
                {
                    Автомобиль_ID = reader.GetInt32(0), // по индексам присваиваем значения в 
                    Марка = reader.GetString(1), // переменные класса Автомобиль
                    Модель = reader.GetString(2),
                    Поколение = reader.GetString(3),
                    Тип_топлива = reader.GetString(4),
                    Объем_двигателя = reader.GetString(5),
                    Мощность_двигателя = reader.GetInt32(6),
                    Тип_КПП = reader.GetString(7),
                    Тип_привода = reader.GetString(8),
                    Тип_кузова = reader.GetString(9),
                    Цвет_кузова = reader.GetString(10),
                    Руль = reader.GetString(11),
                    Название_комплектации = reader.GetString(12),
                    Пробег = reader.GetInt32(13),
                    Цена = reader.GetInt32(14),
                    Год_выпуска = reader.GetInt32(15),
                    Изображение = reader.GetString(16)
                });
            }
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
            string query = "SELECT \r\n    A.ID AS Автомобиль_ID,\r\n    M.Название_Марки,\r\n    MD.Название_модели,\r\n    P.Название_поколения,\r\n" +
                "    C.Название_комплектации,\r\n    A.Пробег,\r\n    A.Цена,\r\n    A.Год_выпуска,\r\n    C.Тип_топлива,\r\n  " +
                "  C.Объем_двигателя,\r\n    C.Мощность_двигателя,\r\n    C.Тип_КПП,\r\n    C.Тип_привода,\r\n    C.Тип_кузова,\r\n  " +
                "  C.Цвет_кузова,\r\n    C.Руль,\r\n    A.Изображение\r\nFROM \r\n    Автомобиль A\r\nJOIN \r\n   " +
                " Марка M ON A.Марка_ID = M.ID\r\nJOIN \r\n    Модель MD ON M.Модель_ID = MD.ID\r\nJOIN \r\n  " +
                "  Поколение P ON MD.Поколение_ID = P.ID\r\nJOIN \r\n    Комплектация C ON A.Комплектация_ID = C.ID\r\nWHERE \r\n  " +
                "  M.Название_Марки = @marks";
            //string query1 = "select Модель_ID from Марка where Название_марки = @marks";

            //SqlCommand cmd = new SqlCommand(query1, RPM.GetConnection());
            //cmd.Parameters.AddWithValue("@marks", marks);
            //SqlDataReader rdr = cmd.ExecuteReader();
            //while(rdr.Read())
            //{
            //    int ans = rdr.GetInt32(0);
            //    Debug.WriteLine(ans + "  SDSDSD");
            //}

            List<Автомобиль> autoss = new List<Автомобиль>(); // Предположим, у вас есть класс Book

            SqlCommand command = new SqlCommand(query, RPM.GetConnection());
            command.Parameters.AddWithValue("@marks", marks); // Добавляем параметр запроса
            RPM.OpenConnection();

            SelectDB(query, autoss, command);

            //using (SqlDataReader reader = command.ExecuteReader())
            //{
            //    while (reader.Read()) // пока есть что считывать
            //    {
            //        autoss.Add(new Автомобиль // добавляем в список элемент типа Автомобиль
            //        {
            //            Автомобиль_ID = reader.GetInt32(0), // по индексам присваиваем значения в 
            //            Марка = reader.GetString(1), // переменные класса Автомобиль
            //            Модель = reader.GetString(2),
            //            Поколение = reader.GetString(3),
            //            Тип_топлива = reader.GetString(4),
            //            Объем_двигателя = reader.GetInt32(5).ToString(),
            //            Мощность_двигателя = reader.GetInt32(6),
            //            Тип_КПП = reader.GetInt32(7).ToString(),
            //            Тип_привода = reader.GetString(8),
            //            Тип_кузова = reader.GetString(9),
            //            Цвет_кузова = reader.GetInt32(10).ToString(),
            //            Руль = reader.GetString(11),
            //            Название_комплектации = reader.GetString(12),
            //            Пробег = Convert.ToInt32(reader.GetString(13)),
            //            Цена = Convert.ToInt32(reader.GetString(14)),
            //            Год_выпуска = Convert.ToInt32(reader.GetString(15)),
            //            Изображение = reader.GetString(16)
            //        });
            //        //Debug.WriteLine(reader.GetInt32(0) + "   fgfgfg");
            //        //Debug.WriteLine(reader.GetString(1) + "   fgfgfg");
            //        //Debug.WriteLine(reader.GetString(2) + "   fgfgfg");
            //        //Debug.WriteLine(reader.GetString(3) + "   fgfgfg");
            //        //Debug.WriteLine(reader.GetString(4) + "   fgfgfg");
            //        //Debug.WriteLine(reader.GetInt32(5) + "   fgfgfg");
            //        //Debug.WriteLine(reader.GetInt32(6) + "   fgfgfg");
            //        //Debug.WriteLine(reader.GetInt32(7) + "   fgfgfg");
            //        //Debug.WriteLine(reader.GetString(8) + "   fgfgfg");
            //        //Debug.WriteLine(reader.GetString(9) + "   fgfgfg");
            //        //Debug.WriteLine(reader.GetInt32(10) + "   fgfgfg");
            //        //Debug.WriteLine(reader.GetString(11) + " gdgfg");
            //        //Debug.WriteLine(reader.GetString(12) + " gfgfg");
            //        //Debug.WriteLine(reader.GetString(13) + " ddddd");
            //        //Debug.WriteLine(reader.GetString(14) + " ddddd");
            //        //Debug.WriteLine(reader.GetString(15) + "   fgfgfg");
            //        //Debug.WriteLine(reader.GetString(16) + "   fgfgfg");
            //    }
            //}

            autogrid.ItemsSource = autoss;
            // обновляем ее

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}

