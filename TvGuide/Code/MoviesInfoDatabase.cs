using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace antiufo.TvGuide
{
    [Serializable]
    public class MoviesInfoDatabase : Antiufo.SerializableStore
    {

        public MoviesInfoDatabase(MovieInfo[] movies, string[] genres)
        {
            this._movies = movies;
            this._genres = genres;
        }

        private MovieInfo[] _movies;
        internal string[] _genres;


        public MovieInfo? GetMovieInfo(int mymoviesId)
        {
            int lower = 0;
            int upper = _movies.Length - 1;

            while (lower <= upper)
            {
                int middle = (lower + upper) / 2;
                int comparisonResult = mymoviesId - _movies[middle].MyMoviesId;
                if (comparisonResult == 0)
                    return _movies[middle];
                else if (comparisonResult < 0)
                    upper = middle - 1;
                else
                    lower = middle + 1;
            }

            return null;

        }





    }
}
