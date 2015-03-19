using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace antiufo.TvGuide.ProgramSuggestions
{

    [Serializable]
    public class TopicalProgramSelector : ProgramSelector
    {
        public override bool Matches(TvProgram program)
        {
            return program.Type == TvProgram.ProgramType.Attualità;
        }

        public override int Strength
        {
            get { return 3; }
        }

        public override string DisplayName
        {
            get { return "i programmi di attualità"; }
        }
    }
}
