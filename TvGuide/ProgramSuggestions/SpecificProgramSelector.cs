using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace antiufo.TvGuide.ProgramSuggestions
{

    [Serializable]
    public class SpecificProgramSelector : ProgramSelector
    {


        public string ProgramName { get; private set; }

        public SpecificProgramSelector(string programName){
            this.ProgramName = programName;
        }

        public override bool Matches(TvProgram program)
        {
            return program.Title == ProgramName;
        }

        public override int Strength
        {
            get { return 7; }
        }


        public override string DisplayName
        {
            get { return "le puntate di "+ProgramName; }
        }

    }
}
