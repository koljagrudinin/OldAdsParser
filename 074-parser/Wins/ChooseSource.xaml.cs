using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _074_Parser.Wins
{
    /// <summary>
    /// Interaction logic for ChooseSource.xaml
    /// </summary>
    public partial class ChooseSource : Window
    {
        /// <summary>
        /// Выбранный источник данных
        /// </summary>
        public string ChoosedSource { get; set; }


        public ChooseSource()
        {
            InitializeComponent();
        }

        private void rbtnYellow_Click(object sender, RoutedEventArgs e)
        {
            ChoosedSource = "Желтые страницы";
        }
    }
}
