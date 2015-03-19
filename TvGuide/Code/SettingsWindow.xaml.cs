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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private List<TvChannelRule> _channels;

        public SettingsWindow()
        {
            InitializeComponent();

            lstRules.ItemsSource = App.Preferences.ProgramRules.OrderBy(x => x.Type).ThenBy(x => x.DisplayName).ToList();


            _channels = TvChannel.AllChannels.Select(x => new TvChannelRule(x)).ToList();
            lstChannels.ItemsSource = _channels;

            chkAutoCheckUpdates.IsChecked = App.AppServices.UpdateChecker.AutomaticallyCheckForUpdates;

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            App.AppServices.UpdateChecker.AutomaticallyCheckForUpdates = chkAutoCheckUpdates.IsChecked.Value;


            foreach (TvChannelRule item in lstChannels.Items)
            {
                if (item.IsEnabled != App.Preferences.IsChannelEnabled(item.Channel))
                {
                    ShouldUpdateProgramsList = true;
                    if (App.Preferences.ChannelRules.ContainsKey(item.Channel))
                        App.Preferences.ChannelRules[item.Channel] = item.IsEnabled;
                    else
                        App.Preferences.ChannelRules.Add(item.Channel, item.IsEnabled);
                }


            }

            Close();
        }

        private void edtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var queryWords = System.Text.RegularExpressions.Regex.Split(this.edtSearchBox.Text.ToLower(), @"\W");
            lstChannels.ItemsSource = _channels.Where(x => ContainsAllStrings(x.DisplayName, queryWords));
         
        }

        private static bool ContainsAllStrings(string text, IEnumerable<string> lowerSubstrings)
        {
            var textLower = text.ToLower();
            foreach (var queryWord in lowerSubstrings)
            {
                if (!textLower.Contains(queryWord)) return false;
            }
            return true;
        }


        private class TvChannelRule
        {
            public TvChannelRule(TvChannel channel)
            {
                this.Channel = channel;
                this.DisplayName = channel.DisplayName;
                this.IsEnabled = App.Preferences.IsChannelEnabled(channel);
            }
            public TvChannel Channel { get; private set; }
            public string DisplayName { get; private set; }
            public bool IsEnabled { get; set; }
        }

        public bool ShouldUpdateProgramsList { get; private set; }

        private void lnkCheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            App.AppServices.UpdateChecker.ManualUpdatesCheck(this.AsWin32Window());
        }



        private void chkAutoCheckUpdates_Unchecked(object sender, RoutedEventArgs e)
        {
            if (e.Source != chkAutoCheckUpdates) return;
            if (!chkAutoCheckUpdates.IsChecked.Value)
            {
                if (MessageBoxResult.OK != MessageBox.Show(this, "Disattivando la ricerca automatica degli aggiornamenti, il programma potrebbe smettere di funzionare correttamente nel caso vengano apportate modifiche alla struttura di MyMovies.it. Continuare?", "Guida TV", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel))
                {
                    chkAutoCheckUpdates.IsChecked = true;
                }

            }
        }



        private void lstChannels_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space) return;
             var t = lstChannels.GetContainerOfSelectedItem();
             if (t != null)
             {
                 var checkbox = t.Find<CheckBox>("channelCheckBox");
                 checkbox.IsChecked = !checkbox.IsChecked;
             }
        
            e.Handled = true;
        }
    }
}
