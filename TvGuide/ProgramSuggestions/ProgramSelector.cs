using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using antiufo.TvGuide;

namespace antiufo.TvGuide.ProgramSuggestions
{

    [Serializable]
    public abstract class ProgramSelector
    {


        public abstract bool Matches(TvProgram program);
        public abstract int Strength { get; }


        public abstract string DisplayName { get;}


        public static readonly InformationProgramSelector InformationProgramSelector = new InformationProgramSelector();
        public static readonly TopicalProgramSelector TopicalProgramSelector = new TopicalProgramSelector();
        public static readonly ItalianMovieSelector ItalianMovieSelector = new ItalianMovieSelector();
        public static readonly ItalianSeriesSelector ItalianSeriesSelector = new ItalianSeriesSelector();
        public static readonly DocumentarySelector DocumentarySelector = new DocumentarySelector();


    }
}
