using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using antiufo.TvGuide;
using Antiufo;

namespace MyMoviesDatabaseDump
{
    class MoviesFileGenerator
    {

        public MoviesFileGenerator() {
            _moviesList = new List<MovieInfo>();
            _genresDictionary = new Dictionary<string, byte>();
            nextGenreId = 1;
        }


        [NonSerialized]
        private List<MovieInfo> _moviesList;
        [NonSerialized]
        private Dictionary<string, byte> _genresDictionary;
        [NonSerialized]
        private byte nextGenreId;

        public void AddMovie(Movie movie)
        {

            byte genreId;
            if (movie.Genre == null)
            {
                genreId = 0;
            }
            else
            {
                if (!_genresDictionary.TryGetValue(movie.Genre, out genreId))
                {
                    _genresDictionary.Add(movie.Genre, nextGenreId);
                    genreId = nextGenreId;
                    nextGenreId++;
                }
            }
            if (genreId >= 128) throw new Exception();
            var countries=movie.Countries.Select(x=>x.Name).ToList();
            var isItalian = countries.All(x => x == "Italia");
            if (!isItalian) {
                var aBitItalian = countries.Any(x => x == "Italia");
                if (aBitItalian) {
                    if (countries.Count <= 3 && !countries.Any(x => x.In(seriousCountries)))
                    {
                        isItalian = true;
                    }
                    else {
                        isItalian = false;
                    }
                    var k = movie.Countries.ToList();
                }
            }
            _moviesList.Add(new MovieInfo(movie.MyMoviesId, (byte)( genreId | (isItalian?1<<7:0))));
        }

        private static readonly string[] seriousCountries = new[] {"USA", "Germania", "Gran Bretagna", "Norvegia", "Svezia", "Danimarca", "Canada", "Giappone" , "Australia" ,"Belgio", "Paesi Bassi", "Irlanda", "Finlandia", "Taiwan", "Inghilterra", "Regno Unito"};

        public MoviesInfoDatabase GetStore()
        {
            
            var _movies = _moviesList.ToArray();

          var  _genres = new string[nextGenreId];
            foreach (var genre in _genresDictionary)
            {
                _genres[genre.Value] = genre.Key;
            }

            return new MoviesInfoDatabase(_movies, _genres);
        }


        internal void Save(string p)
        {
            var s = GetStore();
            s.SaveAs(p);
        }
    }
}
