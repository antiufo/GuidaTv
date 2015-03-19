using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace antiufo.TvGuide.ProgramSuggestions
{

    [Serializable]
    class ProgramRule
    {



        public ProgramSelector ProgramType { get; private set; }
        public RuleType Type { get; private set; }

        public ProgramRule(ProgramSelector programType, RuleType rule)
        {
            this.ProgramType = programType;
            this.Type = rule;
        }

        public bool Matches(TvProgram program)
        {
            return ProgramType.Matches(program);
        }

        public string DisplayName
        {
            get
            {
                return string.Format("{0} {1}", (Type == RuleType.Highlight ? "Evidenzia" : "Nascondi"), ProgramType.DisplayName);
            }
        }


        public string InverseDisplayName
        {
            get
            {
                return string.Format("{0} {1}", (Type == RuleType.Highlight ? "Smetti di evidenziare" : "Smetti di nascondere"), ProgramType.DisplayName);
            }
        }






    }
}
