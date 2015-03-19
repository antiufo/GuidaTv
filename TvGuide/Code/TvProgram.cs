using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using antiufo.QuickTextParsing;
using Antiufo;

namespace antiufo.TvGuide
{
    [Serializable]
    public class TvProgram
    {




        private string _id;




        public TimeSpan? Duration { get; internal set; }
        public DateTime? EndDate
        {
            get
            {
                if (Duration == null) return null;
                return Date + Duration;
            }
        }

        public TvChannel Channel { get; protected set; }

        public string Title { get; protected set; }
        public string Genre { get; protected set; }
        public string Cast { get; protected set; }
        public float? Rating { get; protected set; }
        public int? Year { get; protected set; }

        public ProgramType Type { get; private set; }

        public DateTime Date { get; private set; }

        public bool IsItalian { get; private set; }

        public TimeSpan TimeOfDay
        {
            get { return Date.TimeOfDay; }
        }


        public static TvProgram FromHtml(VirtualString html, DateTime day, TvChannel channel)
        {
            TvProgram prog;


            prog = new TvProgram(html, day);


            prog.Channel = channel;
            return prog;
        }

        protected string[] _grayText;

        public Uri Url { get; private set; }

        internal TvProgram(VirtualString html, DateTime day)
        {

            var textHour = html.BetweenS("<strong>Ore ", "</strong>").Split(',');
            var hour = new TimeSpan(int.Parse(textHour[0]), int.Parse(textHour[1]), 0);
            var date = day.Add(hour);


            var title = Utils.TextFromHtml(html.Between("<h2", "</h2>").After(">").AsString);
            _grayText = html.Between("<h3", "</h3>").After(">").AsString.Trim().Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (StringUtils.UpperCaseLettersProportion(title) > 0.3) Title = title.ToTitleCase();
            else Title = title;

            Date = date;

            if (_grayText.Length > 0)
            {
                var progType = _grayText[0];
                var idx = progType.IndexOf(" - ");

                if (idx != -1)
                {
                    progType = progType.Substring(0, idx);
                    Genre = progType;
                }
                Type = GetProgramType(progType);


                if (_grayText.Length >= 2)
                {
                    var yearString = _grayText[1].TryCapture(@"\((\d{4})\)");
                    if (yearString != null) Year = Convert.ToInt32(yearString);
                    System.Diagnostics.Debug.WriteLine(_grayText[1]);
                }
            }





            var textID = html.BetweenS("dizionario/", "\"", true);
            if (textID != null)
            {
                _id = textID;
                
                this.Url = new Uri("http://www.mymovies.it/dizionario/"+_id);
                var myMoviesId = textID.TryCapture(@"recensione\.asp\?id=(\d+)");
                if (myMoviesId != null && MoviesDatabase != null)
                {
                    var movie = MoviesDatabase.GetMovieInfo(int.Parse(myMoviesId));
                    if (movie != null)
                    {
                        Genre = MoviesDatabase._genres[movie.Value.GenreId];
                        IsItalian = movie.Value.IsItalian;
                        if (this.Type == ProgramType.Unknown) this.Type = ProgramType.Film;
                    }
                }

            }

            var ratingText = html.Between("valutazione media tra critica e pubblico: ", " stelle", true);
            if (ratingText != null)
            {
                Rating = Single.Parse(ratingText.AsString.Replace(',', '.'), System.Globalization.NumberFormatInfo.InvariantInfo) / 5;
            }

















        }


        private static MoviesInfoDatabase _MoviesDatabase;
        private static bool isMoviesDatabaseLoaded;
        public static MoviesInfoDatabase MoviesDatabase
        {
            get
            {

                if (!isMoviesDatabaseLoaded)
                {
                    try
                    {
                        _MoviesDatabase = SerializableStore.OpenExisting<MoviesInfoDatabase>(
                            System.Reflection.Assembly.GetExecutingAssembly().GetLocalFilePath("Movies.dat"));
                    }
                    catch (System.IO.FileNotFoundException) { }
                    isMoviesDatabaseLoaded = true;
                }
                return _MoviesDatabase;
            }
        }


        public static ProgramType GetProgramType(string text)
        {
            switch (text.ToLower())
            {
                case "serie tv": return ProgramType.SerieTv;
                case "film": return ProgramType.Film;
                case "spettacolo": return ProgramType.Spettacolo;
                case "attualit&#224;": return ProgramType.Attualità;
                case "informazione": return ProgramType.Informazione;
                case "documentari": return ProgramType.Documentario;
                default: return ProgramType.Unknown;
            }
        }

        public enum ProgramType
        {

            SerieTv = 1,
            Film = 2,
            Spettacolo = 3,
            Attualità = 4,
            Informazione = 5,
            Unknown = 6,
            Documentario
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1} {2}", Date.ToShortTimeString(), Type.ToString().ToUpper(), Title);
        }


        public bool HasDetailsTable
        {
            get
            {
                return _id != null;
            }
        }

        public DetailsInfo GetDetails(antiufo.Threading.BackgroundFunction<DetailsInfo> a)
        {
            if (!HasDetailsTable) throw new InvalidOperationException("Details are not available for this program.");
            var html = SimpleWebRequest.GetHtmlRobust("http://www.mymovies.it/dizionario/" + _id, true, "ISO-8859-1");

            return new DetailsInfo(html, _id);
        }


        internal void FixToNextDay()
        {
            Date = Date.AddDays(1);
        }



    }
}
