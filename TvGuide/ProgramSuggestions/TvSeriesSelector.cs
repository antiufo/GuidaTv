using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using antiufo.TvGuide;

namespace antiufo.TvGuide.ProgramSuggestions
{

    [Serializable]
    public class TvSeriesSelector : ProgramSelector
    {


        public string SeriesName {get; private set;}

        public TvSeriesSelector(string seriesName) {
            this.SeriesName = seriesName;
        }


        public override bool Matches(TvProgram program)
        {
            return (program.Type==TvProgram.ProgramType.SerieTv && program.Title == SeriesName);
        }

        public override int Strength
        {
            get {
                return 5;
            }
        }

        public override string DisplayName
        {
            get {return "gli episodi di "+ SeriesName; }
        }
    }
}
