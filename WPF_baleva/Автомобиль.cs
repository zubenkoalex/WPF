using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baleva_bd_WPF
{
    public class Автомобиль
    {
        public int Автомобиль_ID { get; set; }
        public string Марка { get; set; }
        public string Модель { get; set; }
        public string Поколение { get; set; }

        public string Тип_топлива { get; set; }
        public string Объем_двигателя { get; set; }
        public int Мощность_двигателя { get; set; }
        public string Тип_КПП { get; set; }
        public string Тип_привода { get; set; }
        public string Тип_кузова { get; set; }
        public string Цвет_кузова { get; set; }
        public string Руль { get; set; }
        public string Название_комплектации { get; set; }
        public int Пробег { get; set; }
        public int Цена { get; set; }
        public int Год_выпуска { get; set; }

        public string Изображение { get; set; }
    }
}

