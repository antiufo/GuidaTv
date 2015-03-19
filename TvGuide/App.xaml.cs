using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using antiufo.TvGuide.ProgramSuggestions;
using antiufo.ApplicationServices;


namespace antiufo.TvGuide
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        internal static PreferencesStore Preferences;
        internal static antiufo.ApplicationServices.ApplicationServices AppServices;

        public App()
        {
            var appdata=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TvGuide");
            Directory.CreateDirectory(appdata);
            System.Windows.Forms.Application.EnableVisualStyles();
            AppServices = new antiufo.ApplicationServices.ApplicationServices("TvGuide", "Guida TV", true, true);
            AppServices.DefaultEndpoint = "http://antiufo.altervista.org/services/index.php";
            AppServices.ProducerUrl = "http://antiufo.altervista.org/";
            AppServices.EndpointTag = "<!--Endpoint2 ";
            
            Preferences = Antiufo.SerializableStore.OpenOrCreate<PreferencesStore>(Path.Combine(appdata, "Settings.dat"));
            
          //  Preferences.AddRule(new InformationProgramSelector(), RuleType.Hide);
          //  Preferences.AddRule(new MovieGenreSelector("Commedia"), RuleType.Highlight);

            
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Preferences.Save();
        }


    }
}
