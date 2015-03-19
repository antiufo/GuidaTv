using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using antiufo.QuickTextParsing;
using System.Net;
using System.IO;

namespace antiufo.TvGuide
{

    [Serializable]
    public class ChannelDay
    {

        public ChannelDay(TvChannel Channel, DateTime Day)
        {
            this.Channel = Channel;
            this.Day = Day;
        }

        public TvChannel Channel { get; private set; }
        public DateTime Day { get; private set; }


        public string Url {
            get {
                return "http://www.mymovies.it/tv/" + Channel.Name + "/?g=" + string.Format("{2:00}{1:00}{0:0000}", Day.Year, Day.Month, Day.Day);
            }
        }

        public override int GetHashCode()
        {
            return Channel.GetHashCode() ^ Day.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var item2 = obj as ChannelDay;
            if (item2 == null) return false;

            return this.Day == item2.Day && this.Channel == item2.Channel;

        }

        private string _html;

        public void SetHtml(string html) {
            _html = html;
        }

        
        

        public IEnumerable<TvProgram> GetProgramsFromSavedHtmlIfAvailable()
        {
            if (_html == null) return Enumerable.Empty<TvProgram>();

            var p = new QuickTextParser(_html.BetweenS("<body", "class=\"piedipagina\""),
                "<div style=\"font-size:180%; text-align:right; ");


            var programs = new List<TvProgram>();

            bool midnightPassed = false;

            double time_prev = 0;
            double time_curr = 0;

            TvProgram previousProgram = null;

            foreach (var t in p)
            {


                if (!t.Contains("<strong>Ore")) continue;
                TvProgram prog;
                try
                {
                    prog = TvProgram.FromHtml(t, Day, Channel);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Errore TvProgram: " + ex.Message);
                    continue;
                }
                time_curr = prog.TimeOfDay.TotalSeconds;

                if (time_curr < time_prev)
                {
                    if (midnightPassed)
                    {
                        throw new InvalidDataException("Channels are not correctly ordered.");
                    }
                    else
                    {
                        midnightPassed = true;
                    }


                }


                if (midnightPassed)
                {
                    prog.FixToNextDay();
                }


                time_prev = time_curr;


                if (previousProgram != null) previousProgram.Duration = prog.Date - previousProgram.Date;

                previousProgram = prog;

                //    if (!midnightPassed && prog.time)
                programs.Add(prog);

                //if (t.Contains("ATTENZIONE: i seguenti programmi si riferiscono al giorno dopo.")) ;

            }

            return programs;

        }







    }
}
