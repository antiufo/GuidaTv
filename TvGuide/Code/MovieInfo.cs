using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace antiufo.TvGuide
{
    [Serializable]
    public struct MovieInfo
    {



        public MovieInfo(int MyMoviesId, byte genreIdAndFlags)
        {
            this._genreIdAndFlags = genreIdAndFlags;
            this._mymoviesId = MyMoviesId;
        }

        private int _mymoviesId;
        private byte _genreIdAndFlags;

        private const byte italianFlag = 1 << 7;

        public byte GenreId
        {
            get
            {
                return  (byte)(_genreIdAndFlags & ~italianFlag);
            }
        }

        public bool IsItalian {
            get {
                return (_genreIdAndFlags & italianFlag) != 0;
            }
        }

        public int MyMoviesId { get { return _mymoviesId; } }



    }
}
