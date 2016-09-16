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
using System.Windows.Navigation;
using System.Windows.Shapes;
using _074_Parser.Wins;
using _074_Parser.Enigne.Parser;
using System.ComponentModel;
using _074_Parser.Enigne;

namespace _074_Parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> tempUrls = new List<string>();
        public List<string> innerUrls = new List<string>();
        private BackgroundWorker bw = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnChooseSource_Click(object sender, RoutedEventArgs e)
        {
            Wins.ChooseSource csWin = new Wins.ChooseSource();
            csWin.Owner = this;
            csWin.Closed += new EventHandler(csWin_Closed);
            csWin.ShowDialog();
        }

        void csWin_Closed(object sender, EventArgs e)
        {
            ChooseSource cs = sender as ChooseSource;
            if (cs != null)
            {
                if (cs.ChoosedSource != "")
                {
                    txtSourceName.Text = cs.ChoosedSource;
                    ParserValuesList.SelectedSourceType=cs.ChoosedSource;
                    grdChoooseMove.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            SelectFilter sf = new SelectFilter
            {
                Owner=this,
            };
            sf.Closed += new EventHandler(sf_Closed);
            sf.ShowDialog();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            SetNewURL();

        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Random rnd = new Random();
            int time = rnd.Next(1000, 3000);
            System.Threading.Thread.Sleep(time);
        }

        void sf_Closed(object sender, EventArgs e)
        {
            isStopped = false;
            Data.DATA = new System.Collections.ObjectModel.ObservableCollection<DataRow>();
            dataGrid1.ItemsSource = Data.DATA;
            webBrowser1.Source = new Uri(ParserValuesList.SelectedFilter.URL);

            btnStart.Visibility = Visibility.Collapsed;

            
        }

        private bool isFirst = true;
        private bool isHead = true;

        private void webBrowser1_LoadCompleted(object sender, NavigationEventArgs e)
        {

                dynamic o = webBrowser1.Document;
                string a = o.documentElement.InnerHtml;

                if (isFirst)
                {
                    tempUrls = ParserNavigator.GetURLs(a);
                    isFirst = false;
                    bw.RunWorkerAsync();
                }
                else
                {
                    if (isHead)
                    {
                        innerUrls = ParserNavigator.GetInnerURLs(a);
                        isHead = false;
                        bw.RunWorkerAsync();
                    }
                    else
                    {
                        ParserNavigator.ParseRow(a);
                        bw.RunWorkerAsync();
                    }
                }
        }

        private void SetNewURL()
        {
            if (isStopped == false)
            {
                if (isHead)
                {
                    if (tempUrls.Count() > 0)
                    {
                        string newURL = tempUrls[0];
                        tempUrls.RemoveAt(0);
                        webBrowser1.Source = new Uri(newURL);
                    }
                }
                else
                {
                    if (innerUrls.Count() > 0)
                    {
                        string newURL = innerUrls[0];
                        innerUrls.RemoveAt(0);
                        webBrowser1.Source = new Uri(newURL);
                    }
                    else
                    {
                        isHead = true;
                        SetNewURL();
                    }
                }
            }
        }

        bool isStopped = false;

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnStart.Visibility = Visibility.Visible;
            isStopped = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.DefaultExt = ".xls";

				

                dlg.Filter = "Excell (.xls)|*.xls";
                if (dlg.ShowDialog() == true)
                {
                    string filename = dlg.FileName;
                    Saver s = new Saver();
                    s.Save(filename);
                    MessageBox.Show("Сохранение завершено.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
