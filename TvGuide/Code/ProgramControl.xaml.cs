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
using antiufo.TvGuide.ProgramSuggestions;
using Antiufo;

namespace antiufo.TvGuide
{
    /// <summary>
    /// Interaction logic for Program.xaml
    /// </summary>
    public partial class ProgramControl : UserControl
    {
        //TODO usa databinding
        public readonly TvProgram program;



        public ProgramControl(TvProgram p)
        {
            InitializeComponent();
            program = p;
            // lblDate.Content = p.Date.ToString("ddd dd");



            lblProgramType.Text = GetProgramTypeDisplayName(p.Type);
            lblProgramType.Background = GetProgramTypeDisplayColor(p.Type);
            lblProgramType.Foreground = GetProgramTypeTextColor(p.Type);


            lblTime.Content = p.Date.ToString("HH:mm");
            lblTitle.Text = p.Title;
            if (p.Channel.Logo != null)
            {
                try
                {
                    picChannelLogo.Source = ConvertToBitmapSource((System.Drawing.Bitmap)p.Channel.Logo);
                }
                catch
                {

                }
            }

            if (program.Genre != null)
                lblGenre.Text = program.Genre;
            else
                lblGenre.Visibility = System.Windows.Visibility.Collapsed;


            if (program.Year != null)
                lblYear.Text = "(" + program.Year + ")";
            else
                lblYear.Visibility = System.Windows.Visibility.Collapsed;


            if (program.Rating != null)
            {
                var rating = "";
                for (double i = 0; i < program.Rating; i += 0.2)
                {
                    rating += "«";
                }
                lblRating.Text = rating;
                byte v = (byte)(program.Rating * 255);
                if (program.Cast != null) lblCast.Content = program.Cast;
                //  lblRating.Foreground = new SolidColorBrush(Color.FromArgb((byte)255, (byte)(255 - v), v, (byte)0));
            }
            else
            {
                lblRating.Visibility = System.Windows.Visibility.Collapsed;
            }


            //  if (program.Type != TvProgram.ProgramType.SerieTv && program.Type != TvProgram.ProgramType.Film)

            //if(program is Movie) if(antiufo.TvGuide.Program.ProgramSuggestions.Utils.IsItalianCast((program as Movie).Cast)  )  Collapsed = true;
            // Collapsed = true;

            UpdateViewState();
            // this.isEven = isEven;
            //this.Background = StandardBackgroundColor;
        }

        public void UpdateViewState()
        {
            var rule = App.Preferences.GetRuleForProgram(program);
            var ruleType = rule == null ? RuleType.Undefined : rule.Type;
            this.Collapsed = ruleType == ProgramSuggestions.RuleType.Hide;
            this.Background = ruleType == ProgramSuggestions.RuleType.Highlight ? HighlightBrush : Brushes.Transparent;

        }




        private Brush HighlightBrush = new SolidColorBrush(Color.FromRgb(0xAA, 0xFF, 0xAA));

        private string GetProgramTypeDisplayName(TvProgram.ProgramType programType)
        {
            switch (programType)
            {
                case TvProgram.ProgramType.Attualità: return "Attualità";
                case TvProgram.ProgramType.Film: return "Film";
                case TvProgram.ProgramType.Informazione: return "Informazione";
                case TvProgram.ProgramType.SerieTv: return "Serie TV";
                case TvProgram.ProgramType.Spettacolo: return "Spettacolo";
                case TvProgram.ProgramType.Documentario: return "Documentario";
                case TvProgram.ProgramType.Unknown: return string.Empty;
                default: return Enum.GetName(typeof(TvProgram.ProgramType), programType);
            }
        }

        private Brush GetProgramTypeDisplayColor(TvProgram.ProgramType programType)
        {
            switch (programType)
            {
                case TvProgram.ProgramType.Attualità: return Brushes.LightCyan;
                case TvProgram.ProgramType.Film: return Brushes.DarkRed;
                case TvProgram.ProgramType.Informazione: return Brushes.LightCyan;
                case TvProgram.ProgramType.SerieTv: return Brushes.Purple;
                case TvProgram.ProgramType.Documentario: return Brushes.Navy;
                case TvProgram.ProgramType.Spettacolo: return Brushes.Fuchsia;
                default: return Brushes.Transparent;
            }
        }
        private Brush GetProgramTypeTextColor(TvProgram.ProgramType programType)
        {
            switch (programType)
            {
                case TvProgram.ProgramType.Attualità: return Brushes.Black;
                case TvProgram.ProgramType.Film: return Brushes.White;
                case TvProgram.ProgramType.Informazione: return Brushes.Black;
                case TvProgram.ProgramType.SerieTv: return Brushes.White;
                case TvProgram.ProgramType.Spettacolo: return Brushes.White;
                case TvProgram.ProgramType.Documentario: return Brushes.White;
                default: return Brushes.Black;
            }
        }


        /*    private Brush StandardBackgroundColor {
                get {
                    return Brushes.White;
                }
            }*/


        private void UiProgram_Load(object sender, EventArgs e)
        {
            //TODO   picChannelLogo.Source= program.Channel.Logo ;
        }

        /* private void UiProgram_MouseEnter(object sender, EventArgs e)
         {
             this.BackColor = hoverColor;
         }*/


        /*     private static readonly Brush evenColor = Brushes.WhiteSmoke;
             private static readonly Brush oddColor = Brushes.WhiteSmoke;*/
        // private static readonly Brush hoverColor = ;




        // private bool focused;

        /*  private void Grid_MouseEnter(object sender, MouseEventArgs e)
          {
              if (!this.program.HasDetailsTable) return;
           //   this.Background = program.HasDetailsTable? Brushes.LightGreen : Brushes.Beige;
          }*/
        /*
                private void Grid_MouseLeave(object sender, MouseEventArgs e)
                {
                    this.Background =   StandardBackgroundColor;
                }*/



        private bool _collapsed;

        public bool Collapsed
        {
            get
            {
                return _collapsed;
            }
            set
            {
                if (_collapsed == value) return;
                _collapsed = value;

                //   this.Background = _collapsed ? LightLightGray : Brushes.Transparent;
                this.Foreground = _collapsed ? Brushes.LightGray : Brushes.Black;

                //   this.lblDate.Visibility = _collapsed ? Visibility.Collapsed : Visibility.Visible;

            }
        }

        //   private static Brush LightLightGray = new SolidColorBrush(Color.FromRgb(242, 242, 242));

        private bool _detailsVisible;

        public bool DetailsVisible
        {
            get
            {
                return _detailsVisible;
            }
            set
            {
                if (_detailsVisible == value) return;
                _detailsVisible = value;



                //  if (!program.HasDetailsTable) return;


                if (!_detailsVisible)
                {
                    pnlDetails.Visibility = System.Windows.Visibility.Collapsed;
                    var browser = (tabTrailer.Content as WebBrowser);
                    if (browser != null) browser.Dispose();
                    tabTrailer.Content = null;
                    //trailerWebBrowser.Navigate(new Uri("about:blank"));
                    return;
                }

                tabs.SelectedIndex = 0;

                pnlDetails.Visibility = System.Windows.Visibility.Visible;

                if (detailsLoaded)
                {
                    WriteTrailerHtml();
                    return;
                }

                //var d = delegate { program.GetDetails(); };

                if (program.HasDetailsTable)
                {
                    var f = new antiufo.Threading.BackgroundFunction<DetailsInfo>(program.GetDetails);
                    f.Finished += new antiufo.Threading.BackgroundOperationBase.FinishedEventHandler(OnDetailsReceive);
                    f.InvokeAsync();

                }
                else
                {
                    tabDescription.Visibility = System.Windows.Visibility.Collapsed;
                    lblLoadingImage.Visibility = System.Windows.Visibility.Collapsed;
                    tabs.SelectedItem = tabInfo;
                    // tabs.Visibility = System.Windows.Visibility.Hidden;
                }

                if (program.Duration == null)
                {
                    txtDuration.Text = "Inizio: " + program.Date.TimeOfDay.ToString();
                }
                else
                {
                    txtDuration.Text = string.Format("{0:0}:{1:00} - {2:0}:{3:00} ({4} minuti)",
                        program.Date.TimeOfDay.Hours,
                        program.Date.TimeOfDay.Minutes,
                        program.EndDate.Value.TimeOfDay.Hours,
                        program.EndDate.Value.TimeOfDay.Minutes,
                        program.Duration.Value.TotalMinutes);
                }

                txtProgramName.Inlines.Clear();
                txtProgramName.Inlines.Add(program.Title);
                txtDate.Text = program.Date.ToShortDateString();
                txtChannel.Text = program.Channel.DisplayName;





            }
        }


        private delegate DetailsInfo DetailsInfoDelegate();

        private void OnDetailsReceive(BackgroundOperationBase opbase)
        {
            var op = (BackgroundFunction<DetailsInfo>)opbase;

            if (op.ExceptionThrown)
            {
                DetailsVisible = false;
                return;
            }

            details = op.Result;

            pnlReview.Text = details.ReviewText;
            
            tabTrailer.Visibility = details.HasTrailer.AsVisibility();

            WriteTrailerHtml();

            if (details.ImageUrl != null && !imageLoaded)
            {

                imageLoaded = true;
                //  picImage.Margin = new Thickness(2);
                lblLoadingImage.Visibility = System.Windows.Visibility.Visible;
                var s = new System.Windows.Media.Imaging.BitmapImage();
                s.BeginInit();
                s.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                s.UriCachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.CacheIfAvailable);
                s.DownloadCompleted += new EventHandler(s_DownloadCompleted);
                s.UriSource = new Uri(details.ImageUrl);
                s.EndInit();
                picImage.ImageFailed += new EventHandler<ExceptionRoutedEventArgs>(picImage_ImageFailed);
                picImage.Source = s;

            }

            detailsLoaded = true;
        }

        void picImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            lblLoadingImage.Visibility = System.Windows.Visibility.Collapsed;
        }


        private void WriteTrailerHtml()
        {


            if (!details.HasTrailer) return;
            var trailerWebBrowser = new WebBrowser();
            trailerWebBrowser.Height = 480;
            tabTrailer.Content = trailerWebBrowser;

            trailerWebBrowser.NavigateToString("<html><head></head><body bgcolor=black scroll=\"no\" style=\"padding:0px; margin:0px;\">" +
    "<embed style=\"background:black; margin:0px; padding:0px;\" type=\"application/x-shockwave-flash\" src=\"http://www.mymovies.it/v8/script/player/player-licensed-viral.swf\" quality=\"high\" allowfullscreen=\"true\" " +
    "flashvars=\"" +
    "file=http://pad.mymovies.it/filmclub/" + details.LegacyCode + "/trailer.flv&plugins=gapro-1&gapro.accountid=UA-259522-1&image=http://www.mymovies.it/filmclub/" + details.LegacyCode + "/cover.jpg&skin=http://www.mymovies.it/v8/script/player/snel.swf&lightcolor=0xFF0066&frontcolor=0xFFFFFF&backcolor=0x002040&autostart=true&volume=80&logo=http://www.mymovies.it/video/logo.png"
    +
    "\" height=\"100%\" width=\"100%\"></body></html>");
            /*  Trailer.NavigateToString("<html><head></head><body bgcolor=black scroll=\"no\" style=\"padding:0px; margin:0px;\">" +
              "<embed style=\"background:black; margin:0px; padding:0px;\" type=\"application/x-shockwave-flash\" src=\"http://www.mymovies.it/v8/script/player/player-licensed-viral.swf\" quality=\"high\" allowfullscreen=\"true\" " +
              "flashvars=\"autostart=true&amp;logo=http://www.mymovies.it/video/logo.png&amp;image=http://www.mymovies.it/filmclub/" + filmId + "/cover.jpg&amp;skin=http://www.mymovies.it/v8/script/player/snel.swf&amp;lightcolor=0xFF0066&amp;frontcolor=0xFFFFFF&amp;backcolor=0x002040&amp;plugins=gapro-1&amp;file=http://www.mymovies.it/v9/include/player/playlist/editoriale.asp?codicefilm=" + filmId + "&amp;repeat=list&amp;gapro.accountid=UA-259522-1\" " +
              "height=\"100%\" width=\"100%\"></body></html>");*/
            //var f=Trailer.Document=.GetType();
        }


        private DetailsInfo details;
        private bool detailsLoaded = false;


        private bool imageLoaded = false;

        private void pnlInfo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // if (DetailsVisible) DetailsVisible = false;
            //(ListView)(this.Parent).s
        }

        void s_DownloadCompleted(object sender, EventArgs e)
        {
                lblLoadingImage.Visibility = System.Windows.Visibility.Hidden;
        }

        public static BitmapSource ConvertToBitmapSource(System.Drawing.Bitmap gdiPlusBitmap)
        {

            IntPtr hBitmap = gdiPlusBitmap.GetHbitmap();

            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        }

        private void cmdHideProgram_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdAddAlert_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtProgramName_Click(object sender, RoutedEventArgs e)
        {
            string url;
            if (program.Url != null)
            {
                url = program.Url.ToString();
            }
            else
            {
                var query = new List<string>();
                query.Add(program.Title);

                if (program.Type == TvProgram.ProgramType.Film && program.Year != null)
                {
                    query.Add(program.Year.ToString());
                }

                if (program.Type != TvProgram.ProgramType.SerieTv && program.Type != TvProgram.ProgramType.Film)
                {
                    query.Add(program.Channel.DisplayName);
                }
                url = "http://www.google.it/search?hl=it&q=" + Uri.EscapeDataString(string.Join(" ", query.ToArray()));
            }
            System.Diagnostics.Process.Start(url);
        }




        /*  private void UiProgram_MouseLeave(object sender, EventArgs e)
          {
            //  if(Cursor.)
            
          }*/

        /*  private void UiProgram_MouseMove(object sender, MouseEventArgs e)
          {
              var s = this.Bounds.Contains(e.X, e.Y);
              if (s == focused) return;
              focused = s;
              if (focused)
              {
                  this.BackColor = hoverColor;
              }
              else {
                  this.BackColor = isEven ? evenColor : oddColor;
              }
          }*/

    }
}
