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
using Antiufo;

namespace antiufo.TvGuide
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            lblVersion.Content = string.Format("Versione {0}", App.AppServices.ProductVersion.ToString());
        }



        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
         
        }

        private void lnkWebSite_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://at-my-window.blogspot.com/?page=tvguide");
        }

        private void btnSendFeedback_Click(object sender, RoutedEventArgs e)
        {

            App.AppServices.ShowFeedbackForm(this.AsWin32Window());
        }

        private void btnDonate_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://antiufo.altervista.org/donate?product=Guida+TV&eur=1&from=TvGuideAboutBox&version="+App.AppServices.ProductVersion);
        }

        private void lnkMyMovies_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mymovies.it/");
        }
    }
}
