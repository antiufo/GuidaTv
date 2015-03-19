using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace antiufo.TvGuide.ProgramSuggestions
{

    [Serializable]
    public class ItalianSeriesSelector : ProgramSelector
    {
        public override bool Matches(TvProgram program)
        {
            if (program.Type != TvProgram.ProgramType.SerieTv) return false;

            return program.IsItalian;

          //  var movie = program as Movie;
          //  if (movie == null) return false;
        //    if (program.Cast == null) return false;
         //   return Utils.IsItalianCast(program.Cast);
            
        }

        public override int Strength
        {
            get { return 4; }
        }

        public override string DisplayName
        {
            get { return "le serie TV italiane"; }
        }
    }
}
