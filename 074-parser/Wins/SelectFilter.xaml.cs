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
using System.ComponentModel;
using _074_Parser.Enigne.Parser;

namespace _074_Parser.Wins
{
    /// <summary>
    /// Interaction logic for SelectFilter.xaml
    /// </summary>
    public partial class SelectFilter : Window
    {
        private BackgroundWorker bw = new BackgroundWorker();

        public SelectFilter()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.RunWorkerAsync();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            grdChoose.Visibility = Visibility.Visible;
            grdFilterInfo.Visibility = Visibility.Collapsed;
            cbxFilters.ItemsSource = ParserValuesList.FilterOptions;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            ParserNavigator.DoStart("");
        }

        private void cbxFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxFilters.SelectedIndex != -1)
            {
                btnStart.Visibility = Visibility.Visible;
                ParserValuesList.SelectedFilter = cbxFilters.SelectedItem as ParserValuesList.OneRow;
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
