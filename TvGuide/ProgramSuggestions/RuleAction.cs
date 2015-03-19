using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace antiufo.TvGuide.ProgramSuggestions
{

    [Serializable]
    class RuleAction
    {


        public ProgramRule Rule { get; private set; }
        public RuleActionType Action { get; private set; }

        public RuleAction(ProgramRule rule, RuleActionType action) {
            this.Rule = rule;
            this.Action = action;
        }

        public string DisplayName {
            get {
                return Action == RuleActionType.Add ? Rule.DisplayName : Rule.InverseDisplayName;
            }
        }

        public override string ToString()
        {
            return DisplayName;
        }

    }
}
