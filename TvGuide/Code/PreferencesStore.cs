using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using antiufo.TvGuide.ProgramSuggestions;

namespace antiufo.TvGuide
{

    [Serializable]
    class PreferencesStore : Antiufo.SerializableStore
    {


        public List<ProgramRule> ProgramRules;

        public Dictionary<TvChannel, bool> ChannelRules;

        public void AddRule(ProgramSelector programType, RuleType ruleType)
        {
            ProgramRules.Add(new ProgramRule(programType, ruleType));
        }


        

        public PreferencesStore()
        {
            ProgramRules = new List<ProgramRule>();
            ChannelRules = new Dictionary<TvChannel, bool>();

            AddRule(new InformationProgramSelector(), RuleType.Hide);
            AddRule(new SpecificProgramSelector("Cinema Festival"), RuleType.Hide);
            AddRule(new SpecificProgramSelector("I Bellissimi di Rete 4"), RuleType.Hide);
            AddRule(new SpecificProgramSelector("Movie flash"), RuleType.Hide);
            AddRule(new SpecificProgramSelector("Anica flash"), RuleType.Hide);
            AddRule(new SpecificProgramSelector("Appuntamento al cinema"), RuleType.Hide);
            AddRule(new SpecificProgramSelector("Mediashopping"), RuleType.Hide);
        }


        public bool IsChannelEnabled(TvChannel channel) {
            bool result;
            if (ChannelRules.TryGetValue(channel, out result)) return result;
            return defaultChannels.Contains(channel.Name);
        }


        private readonly static string[] defaultChannels = new string[] {
            "raiuno", "raidue", "raitre", "rete4", "canale5", "italia1", "la7", "mtv", "rai4", "iris" 
        };


        public ProgramRule GetRuleForProgram(TvProgram program)
        {
            var matchingRules = GetAllMatchingRulesForProgram(program).ToList();
            if (!matchingRules.Any()) return null;

            var activeRule = matchingRules[0];

            for (int i = 1; i < matchingRules.Count; i++)
            {
                var currRule = matchingRules[i];
                if (currRule.Type == activeRule.Type) activeRule = currRule;
            }

            return activeRule;

        }


        public IOrderedEnumerable<ProgramRule> GetAllMatchingRulesForProgram(TvProgram program)
        {
            return ProgramRules
                  .Where(x => x.Matches(program))
                  .OrderByDescending(x => x.ProgramType.Strength);
        }




        public IEnumerable<RuleAction> GetRulesMenuForProgram(TvProgram program)
        {
            var rule = GetRuleForProgram(program);
            var ruleType = rule == null ? RuleType.Undefined : rule.Type;

            var actions = new List<RuleAction>();

            if (ruleType == RuleType.Highlight)
            {
                actions.Add(new RuleAction(rule, RuleActionType.Remove));
            }
            else if (ruleType == RuleType.Hide)
            {
                actions.Add(new RuleAction(rule, RuleActionType.Remove));
            }
            else
            {

                if (program.Type == TvProgram.ProgramType.SerieTv)
                {

                    AddRules(actions, new TvSeriesSelector(program.Title), ProgramRuleCompatibility.Both);
                }
                else if (program.Type == TvProgram.ProgramType.Film)
                {
                    if (!string.IsNullOrEmpty(program.Genre))
                    {
                        AddRules(actions, new MovieGenreSelector(program.Genre), ProgramRuleCompatibility.Both);
                    }
                }
                else
                {
                    AddRules(actions, new SpecificProgramSelector(program.Title), ProgramRuleCompatibility.Both);

                }

                if (ProgramSelector.InformationProgramSelector.Matches(program))
                    AddRules(actions, ProgramSelector.InformationProgramSelector, ProgramRuleCompatibility.CanHide);
                if (ProgramSelector.DocumentarySelector.Matches(program))
                    AddRules(actions, ProgramSelector.DocumentarySelector, ProgramRuleCompatibility.Both);
                if (ProgramSelector.TopicalProgramSelector.Matches(program))
                    AddRules(actions, ProgramSelector.TopicalProgramSelector, ProgramRuleCompatibility.Both);
                if (ProgramSelector.ItalianMovieSelector.Matches(program))
                    AddRules(actions, ProgramSelector.ItalianMovieSelector, ProgramRuleCompatibility.CanHide);
                if (ProgramSelector.ItalianSeriesSelector.Matches(program))
                    AddRules(actions, ProgramSelector.ItalianSeriesSelector, ProgramRuleCompatibility.CanHide);


            }

            return actions.OrderBy(x=>x.DisplayName);
        }

        private void AddRules(List<RuleAction> list, ProgramSelector selector, ProgramRuleCompatibility mode)
        {
            if (mode == ProgramRuleCompatibility.CanHighlight || mode == ProgramRuleCompatibility.Both)
            {
                list.Add(new RuleAction(new ProgramRule(selector, RuleType.Highlight), RuleActionType.Add));
            }
            if (mode == ProgramRuleCompatibility.CanHide || mode == ProgramRuleCompatibility.Both)
            {
                list.Add(new RuleAction(new ProgramRule(selector, RuleType.Hide), RuleActionType.Add));
            }

        }



        internal void ApplyRuleChange(RuleAction action)
        {
            if (action.Action == RuleActionType.Add)
            {
                ProgramRules.Add(action.Rule);
            }
            else {
                ProgramRules.Remove(action.Rule);
            }
        }
    }
}
