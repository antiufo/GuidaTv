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
using antiufo.TvGuide;
using antiufo.Threading;
using Antiufo;
using System.Net;
using System.IO;
using System.Windows.Media.Animation;


namespace antiufo.TvGuide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
/*
            var s = System.Diagnostics.Stopwatch.StartNew();
            var web = new WebBrowser();
            web.NavigateToString(string.Empty);
          
            s.Stop();
            System.Diagnostics.Debug.WriteLine("Tempo trascorso:"+ s.Elapsed.ToString());
*/
            /*   this.Width = 800;
               this.Height = 600;*/
            /*  this.Width = System.Windows.SystemParameters.PrimaryScreenWidth - 100;
              this.Height = System.Windows.SystemParameters.PrimaryScreenHeight - 100;*/
            //storePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "TvGuideCache.dat");

        }


        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


             date = DateTime.Today;
            ShowEveningPrograms = false;

            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;


            LoadProgramsAsync();
            pnlPrograms.Focus();

            App.AppServices.UpdateChecker.AsyncAutomaticUpdatesCheck();
     


        }

        void pnlPrograms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                var lvi = item as ListBoxItem;
                if (lvi == null) continue;
                ((ProgramControl)(lvi.Content)).DetailsVisible = true;
            }
            foreach (var item in e.RemovedItems)
            {
                var lvi = item as ListBoxItem;
                if (lvi == null) continue;
                ((ProgramControl)(lvi.Content)).DetailsVisible = false;
            }

            // btnNext.IsEnabled = pnlPrograms.SelectedIndex != pnlPrograms.Items.Count - 1;
            // btnBack.IsEnabled = pnlPrograms.SelectedIndex != 0;

        }







       // private TvGuideStore store;

 




        //private void Window_Closed(object sender, EventArgs e)
        //{
        //    store.Save(storePath);
        //}

        /*  public static IList<TvProgram> GetInterestingPrograms(IEnumerable<TvProgram> src)
          {

              return src.OrderBy(x => x.Date).Cast<TvProgram>().ToList();

              // var interesting = new List<TvProgram>();
               foreach (TvProgram t  in src)
               {
                 
               }
              //return null;
          }*/



        /*    private bool _showAllPrograms;
            public bool ShowAllPrograms
            {
                get
                {
                    return _showAllPrograms;
                }
                set
                {
                    _showAllPrograms = value;
                    SetVisibility(btnAllPrograms, !_showAllPrograms);
                    SetVisibility(btnSuggestedPrograms, _showAllPrograms);

                }
            }*/

        private bool _thisEvening;
        public bool ShowEveningPrograms
        {
            get
            {
                return _thisEvening;
            }
            set
            {
                _thisEvening = value;
               // SetVisibility(btnEvening, !_thisEvening);
              //  SetVisibility(btnNowOnTv, _thisEvening);
            }
        }

        private static void SetVisibility(UIElement el, bool value)
        {
            el.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        /*  private void btnAllPrograms_Click(object sender, RoutedEventArgs e)
          {
              ShowAllPrograms = true;
              LoadPrograms();
          }

          private void btnSuggestedPrograms_Click(object sender, RoutedEventArgs e)
          {
              ShowAllPrograms = false;
              LoadPrograms();
          }*/

        private void btnNowOnTv_Click(object sender, RoutedEventArgs e)
        {
            ShowEveningPrograms = false;
            LoadProgramsAsync();
        }

        private void btnEvening_Click(object sender, RoutedEventArgs e)
        {
            ShowEveningPrograms = true;
            LoadProgramsAsync();
        }

        /*  private void btnNext_Click(object sender, RoutedEventArgs e)
          {
              if (pnlPrograms.SelectedIndex == -1)
              {
                  pnlPrograms.SelectedIndex = 0;
              }
              else
              {
                  pnlPrograms.SelectedIndex++;
              }
              ((FrameworkElement)(pnlPrograms.SelectedItem)).BringIntoView();

          }

          private void btnBack_Click(object sender, RoutedEventArgs e)
          {
              if (pnlPrograms.SelectedIndex == -1)
              {
                  pnlPrograms.SelectedIndex = pnlPrograms.Items.Count - 1;
              }
              else
              {
                  pnlPrograms.SelectedIndex--;
              }
              ((FrameworkElement)(pnlPrograms.SelectedItem)).BringIntoView();
          }

          */


        private BackgroundUIFunction<IEnumerable<TvProgram>> loadProgramsOperation;


        private DateTime date;
        

        private void LoadProgramsAsync()
        {



            if (loadProgramsOperation != null)
            {
                loadProgramsOperation.RequestCancellation();
            }
            pgbUpdating.BeginAnimation(ProgressBar.ValueProperty, null, HandoffBehavior.SnapshotAndReplace);
            pgbUpdating.Value = 0;
            
            var activeChannels = TvChannel.AllChannels.Where(x => App.Preferences.IsChannelEnabled(x));
            var day = date.Date;
            var pages = activeChannels.Select(x => new ChannelDay(x, day)).ToList();
            var loader = new PagesLoader();
            loadProgramsOperation = new BackgroundUIFunction<IEnumerable<TvProgram>>(op => loader.ExecuteLoadPrograms(op, pages).OrderBy(x => x.Date).ToList());
            loadProgramsOperation.Finished += new BackgroundOperationBase.FinishedEventHandler(loadProgramsOperation_Finished);
            loadProgramsOperation.UserTextChange += new BackgroundUIAction.UserTextChangeEventHandler(loadProgramsOperation_UserTextChange);
            loadProgramsOperation.ProgressChange += new BackgroundUIAction.ProgressChangeEventHandler(loadProgramsOperation_ProgressChange);
            loadProgramsOperation.ReportProgressChange += new BackgroundUIAction.ReportProgressChangeEventHandler(loadProgramsOperation_ReportProgressChange);

            pnlPrograms.Visibility = System.Windows.Visibility.Collapsed;
            pnlUpdating.Visibility = System.Windows.Visibility.Visible;

            loadProgramsOperation.InvokeAsync();

        }





        void loadProgramsOperation_ReportProgressChange(bool reportProgress)
        {
            pgbUpdating.IsIndeterminate = !reportProgress;
        }

        void loadProgramsOperation_ProgressChange(float progress)
        {
         
            pgbUpdating.BeginAnimation(ProgressBar.ValueProperty, new DoubleAnimation(progress * 100, TimeSpan.FromSeconds(2)), HandoffBehavior.SnapshotAndReplace);
        }

        void loadProgramsOperation_UserTextChange(string userText)
        {
            txtUpdatingChannel.Text = userText;
        }



        void loadProgramsOperation_Finished(BackgroundOperationBase op_base)
        {
            var op = (BackgroundUIFunction<IEnumerable<TvProgram>>)op_base;
            if (op.Aborted) return;



            if (op.ExceptionThrown)
            {
                App.AppServices.ErrorReporter.ShowErrorBox("Impossibile caricare la guida TV.", op.Exception, this.AsWin32Window());

                pnlUpdating.Visibility = System.Windows.Visibility.Collapsed;
                pnlPrograms.Visibility = System.Windows.Visibility.Visible;
                return;
            }



            pnlPrograms.BeginInit();
            pnlPrograms.Items.Clear();


            var progs = op.Result;

            var now = DateTime.Now;




            if (date.Date == DateTime.Today)
            {
                DateTime maxEndDate = now.Date.AddDays(1).AddHours(2);
                progs = progs.Where(x =>
                        (
                           x.EndDate.HasValue ?
                              (x.EndDate >= now) :
                              ((x.Date - now).TotalMinutes > -30)
                        )
                        &&
                        (
                           x.EndDate.HasValue ?
                                x.EndDate < maxEndDate :
                                x.Date < maxEndDate
                        )
                    );
            }

            //if (!ShowAllPrograms) progs = progs.Where(x => x is Movie);


            foreach (var item in progs)
            {
                //   even = !even;
                var u = new ProgramControl(item);
                var m = new ListBoxItem();
                m.Foreground = Brushes.Black;
                //     m.Background = Brushes.Green;
                m.Content = u;

                this.pnlPrograms.Items.Add(m);
            }
            //   pnlPrograms.SelectedIndex = -1;
            pnlPrograms.EndInit();
            pnlUpdating.Visibility = System.Windows.Visibility.Collapsed;
            pnlPrograms.Visibility = System.Windows.Visibility.Visible;
        }


        public void UpdateViewStates() {
            foreach (ListBoxItem lbi in pnlPrograms.Items)
            {
                var prog=(ProgramControl)lbi.Content;
                prog.UpdateViewState();
            }
        }


        private void Window_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var listboxItem = pnlPrograms.SelectedItem as ListBoxItem;
            if (listboxItem == null) { e.Handled = true; return; }

            var program = (listboxItem.Content as ProgramControl).program;

            var actions = App.Preferences.GetRulesMenuForProgram(program);

            cxtMenu.Items.Clear();

            foreach (var action in actions)
            {
                var item = new MenuItem();
                item.Header = action.DisplayName;
                item.Tag = action;
                item.Click += new RoutedEventHandler(item_Click);
                cxtMenu.Items.Add(item);
            }

            if (!actions.Any()) {
                cxtMenu.Items.Add( new MenuItem() { 
                    Header="(Nessuna opzione disponibile)",
                    IsEnabled=false
                });
            }

        }

        void item_Click(object sender, RoutedEventArgs e)
        {
            var action = (e.Source as MenuItem).Tag as TvGuide.ProgramSuggestions.RuleAction;
            App.Preferences.ApplyRuleChange(action);
            UpdateViewStates();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            var f = new SettingsWindow();
            f.Owner = this;
            f.ShowDialog();
            if(f.ShouldUpdateProgramsList)  LoadProgramsAsync();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            var f = new AboutWindow();
            f.Owner = this;
            f.ShowDialog();
          
        }



        private void btnPreviousDay_Click(object sender, RoutedEventArgs e)
        {
            date -= TimeSpan.FromDays(1);
            UpdateWindowTitle();
            LoadProgramsAsync();
        }

        private void btnNextDay_Click(object sender, RoutedEventArgs e)
        {
            date += TimeSpan.FromDays(1);
            UpdateWindowTitle();
            LoadProgramsAsync();
        }

        private void UpdateWindowTitle() {
            var day = date.Date;

            btnNextDay.IsEnabled = day - DateTime.Now < TimeSpan.FromDays(5);
            btnPreviousDay.IsEnabled = DateTime.Now - day < TimeSpan.FromDays(5);
           // if (day == DateTime.Today) this.Title = "Guida TV";
            this.Title = "Guida TV - "+ GetUserFriendlyDate(day);
        }

        private string GetUserFriendlyDate(DateTime day)
        {
            if (day == DateTime.Today) return "Oggi";
            if (day == DateTime.Today - TimeSpan.FromDays(1)) return "Ieri";
            if (day == DateTime.Today + TimeSpan.FromDays(1)) return "Domani";
            return day.ToString("dddd d MMMM");
        }

        private void btnSendFeedback_Click(object sender, RoutedEventArgs e)
        {
            App.AppServices.ShowFeedbackForm(this.AsWin32Window());
        }

    }

    [ValueConversion(typeof(int), typeof(int))]
    public class WidthReducer : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Math.Max(50d, ((double)value - double.Parse((string)parameter)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
