using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace antiufo.TvGuide.ProgramSuggestions
{

    [Serializable]
    enum ProgramRuleCompatibility
    {
        CanHighlight=0,
        CanHide=1,
        Both=2
    }
}
