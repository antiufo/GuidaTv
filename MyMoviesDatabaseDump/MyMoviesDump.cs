using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antiufo;
using antiufo.QuickTextParsing;
using HtmlAgilityPack;
using System.Net;
using antiufo.TvGuide;

namespace MyMoviesDatabaseDump
{
    public class MyMoviesDump
    {

        private readonly Model1Container db;

        public MyMoviesDump(Model1Container db)
        {
            this.db = db;
        }

        private WebClient web = new WebClient() { 
            CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.CacheIfAvailable) ,
            Encoding = System.Text.Encoding.GetEncoding("iso-8859-1")
        };

        private class EndOfPagesException:Exception{
        }

        public void DumpPage(int year, int page)
        {


            var url = string.Format("http://www.mymovies.it/film/{0}/?pagina={1}", year, page);

            Console.WriteLine(url);

           // var pageHtml = SimpleWebRequest.GetHtmlRobust(url, Encoding: "iso-8859-1");

            web.Headers[HttpRequestHeader.UserAgent] = SimpleWebRequest.DefaultUserAgent;
            var pageHtml = web.DownloadString(url);


            if (pageHtml.Contains("Si è verificato un errore nella pagina, riprova a collegarti facendo clic all'indirizzo qui sotto"))
                throw new EndOfPagesException();


            var parser = new QuickTextParser(pageHtml.BetweenS("<table class=\"struttura\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">", "<!--***** Inizio Sinistra *****-->"), "<a", "<div style=\"clear:both; height:10px;\"></div>");
           


            foreach (var movieHtmlVs in parser)
            {
                var movieHtml = "<a" + movieHtmlVs.AsString;
                var node = movieHtml.AsHtmlNode();

                var movie = new Movie()
                {

                    MyMoviesId = int.Parse(node.FindAll("script").ElementAt(1).InnerText.TryCapture(@"schiarisci(\d+)\(")),
                    Title = (node.FindSingle("h2 a") ?? node.FindSingle("a")).GetText(),
                    Rating = TryGet(() => Single.Parse(movieHtml.BetweenS("valutazione media tra critica e pubblico: ", " stelle").Replace(',', '.'), System.Globalization.NumberFormatInfo.InvariantInfo)),
                    Genre = TryGet(() => movieHtml.TryCapture(@"Genere\s*[^>]*>([^<]+)")),
                    Year = year,
                    Summary = TryGet(() => node.FindSingle("p[style='margin-top:5px']").GetText()),
                    ImageCode = TryGet(() => movieHtml.TryCapture(@"http\://pad\.mymovies\.it/filmclub\/([\d/]+)\/imm2")),
                    TrailerCode = TryGet(() => movieHtml.TryCapture(@"videotrailer_centrale\.asp\?codicefilm=([\d/]+)&")),
                    ShortDescription = TryGet(() => node.FindSingle("h3").GetText()),
                    ShortName = TryGet(() => node.FindSingle("h2 a").GetAttributeValue("href", string.Empty).TryCapture(@"film/\d{4}/(.+?)/"))

                };

                if (db.Movies.Any(x => x.MyMoviesId == movie.MyMoviesId))
                {
                    Console.WriteLine("Duplicato: {0} {1}", movie.MyMoviesId, movie.Title);
                    continue;
                }


                Console.WriteLine(movie.Title);
                var suggested = movieHtml.TryCapture(">Consigliato: (.+?)<");
                switch (suggested)
                {
                    case "Assolutamente No": movie.Suggestion = 1; break;
                    case "No": movie.Suggestion = 2; break;
                    case "N&igrave;": movie.Suggestion = 3; break;
                    case "S&igrave;": movie.Suggestion = 4; break;
                    case "Assolutamente S&igrave;": movie.Suggestion = 5; break;
                    case null: break;
                    default: throw new Exception();
                }

                var actorsHtmls = new QuickTextParser(node.FindSingle(".linkblu").InnerHtml, "http://www.mymovies.it/biografia/?a=", "</a>");
                foreach (var act in actorsHtmls)
                {
                    var s = act.AsString;
                    var actorCode = int.Parse(s.TryCapture(@"(\d+)"));
                    var actorName = s.TryCapture(@">([^<]+)").DeEntitize();
                    var actor = db.Actors.Where(x => x.MyMoviesId == actorCode).SingleOrDefault();
                    if (actor == null)
                    {
                        actor = new Actor();
                        actor.MyMoviesId = actorCode;
                        actor.Name = actorName;
                        db.Actors.AddObject(actor);
                        db.SaveChanges();
                    }
                    movie.Actors.Add(actor);
                }

                var countries = movieHtml.TryCapture(@"produzione ([^<]+),\s*<a");
                if (countries != null)
                {
                    foreach (var cntr in countries.DeEntitize().Split(','))
                    {
                        var name = cntr.Trim();
                        var country = db.Countries.Where(x => x.Name == name).SingleOrDefault();
                        if (country == null)
                        {
                            country = new Country();
                            country.Name = name;
                            db.Countries.AddObject(country);
                        }
                        movie.Countries.Add(country);
                        db.SaveChanges();
                    }
                }

                var directorIdString = movieHtml.TryCapture(@"biografia/\?r=(\d+)");
                if (directorIdString != null)
                {
                    var directorId = int.Parse(directorIdString);
                    var directorName = movieHtml.TryCapture(@"biografia/\?r=\d+\x22>(.+?)<").DeEntitize();
                    var dir = db.Directors.Where(x => x.MyMoviesId == directorId).SingleOrDefault();
                    if (dir == null)
                    {
                        dir = new Director();
                        dir.MyMoviesId = directorId;
                        dir.Name = directorName;
                        db.Directors.AddObject(dir);
                    }

                    movie.Director = dir;
                }

                if (movie.EntityState == System.Data.EntityState.Detached) {
                    db.Movies.AddObject(movie);
                }
               // db.Movies.AddObject(movie);

                db.SaveChanges();


            }
        }


        public static Single? TryGet(Func<Single> func)
        {
            try
            {
                return func();
            }
            catch (Exception)
            {

                return null;
            }
        }


        public static T TryGet<T>(Func<T> func) where T : class
        {
            try
            {
                return func();
            }
            catch (Exception)
            {

                return null;
            }
        }


        public void DumpYear(int year)
        {

            var page = 1;
            while (true)
            {
                try
                {
                    DumpPage(year, page);
                    page++;
                }
                catch (WebException ex)
                {
                    var r = ex.Response as HttpWebResponse;
                    if (r == null) throw;
                    if (r.StatusCode == HttpStatusCode.InternalServerError) break;
                    if (r.StatusCode == HttpStatusCode.NotFound) return;
                    throw;
                }
                catch (EndOfPagesException)
                {
                    break;
                }
            }
        }






        internal void SaveGenreToBinaryFile(MoviesFileGenerator store)
        {
            var movies = db.Movies.OrderBy(x => x.MyMoviesId);

            foreach (var movie in movies)
            {
                Console.WriteLine(movie.Title);
                store.AddMovie(movie);
            }
   
        }
    }
}
