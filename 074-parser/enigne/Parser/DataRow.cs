using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace _074_Parser.Enigne.Parser
{
    /// <summary>
    /// Столбец данных
    /// </summary>
    public class DataRow
    {
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>
        public string Adress { get; set; }
        /// <summary>
        /// Адрес 2
        /// </summary>
        public string Adress2 { get; set; }
        /// <summary>
        /// Рубрика?
        /// </summary>
        public string Rubric { get; set; }
        /// <summary>
        /// Справочник?
        /// </summary>
        public string Sprav { get; set; }
        /// <summary>
        /// Рисунок
        /// </summary>
        public Bitmap bmp { get; set; }
    }
}
