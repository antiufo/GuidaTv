using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace antiufo.TvGuide.ProgramSuggestions
{

    [Serializable]
    public class ItalianMovieSelector : ProgramSelector
    {
        public override bool Matches(TvProgram program)
        {
            if (program.Type != TvProgram.ProgramType.Film) return false;

            return program.IsItalian;


        }

        public override int Strength
        {
            get { return 4; }
        }

        public override string DisplayName
        {
            get { return "i film italiani"; }
        }
    }
}
