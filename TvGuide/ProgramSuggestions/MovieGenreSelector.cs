using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace antiufo.TvGuide.ProgramSuggestions
{

    [Serializable]
    public class MovieGenreSelector : ProgramSelector
    {


        public string GenreName {get; private set;}

        public MovieGenreSelector(string genre)
        {
            this.GenreName = genre;
        }


        public override bool Matches(TvProgram program)
        {
            if (program.Type != TvProgram.ProgramType.Film) return false;
            return program.Genre==GenreName;
        }

        public override int Strength
        {
            get {
                return 5;
            }
        }

        public override string DisplayName
        {
            get {
                switch (GenreName.ToLower())
                {

                    case "animazione": return "i film di animazione";
                    case "avventura": return "i film di avventura";
                    case "azione": return "i film d'azione";
                    case "biografico": return "i film biografici";
                    case "catastrofico": return "i film catastrofici";
                    case "cofanetto": return "i film cofanetti";
                    case "comico": return "i film comici";
                    case "commedia": return "le commedie";
                    case "commedia a episodi": return "le commedie a episodi";
                    case "commedia drammatica": return "le commedie drammatiche";
                    case "commedia musicale": return "le commedie musicali";
                    case "commedia nera": return "le commedie nere";
                    case "commedia rosa": return "le commedie rosa";
                    case "commedia sentimentale": return "le commedie sentimentali";
                    case "cortometraggio": return "i cortometraggi";
                    case "docu-fiction": return "le docu-fiction";
                    case "documentario": return "i film documentari";
                    case "documentario musicale": return "i film documentari musicali";
                    case "documentario drammatico": return "i film documentari drammatici";
                    case "drammatico": return "i film drammatici";
                    case "epico": return "i film epici";
                    case "episodi": return "i film a episodi";
                    case "erotico": return "i film erotici";
                    case "fantascienza": return "i film di fantascienza";
                    case "fantastico": return "i film fantastici";
                    case "farsesco": return "i film farseschi";
                    case "fiabesco": return "i film fiabeschi";
                    case "film a episodi": return "i film a episodi";
                    case "film di montaggio": return "i film di montaggio";
                    case "giallo": return "i film gialli";
                    case "giallo rosa": return "i gialli rosa";
                    case "grottesco": return "i film grotteschi";
                    case "guerra": return "i film di guerra";
                    case "hard boiled": return "i film hard boiled";
                    case "horror": return "i film horror";
                    case "mitologico": return "i film mitologici";
                    case "musical": return "i musical";
                    case "musicale": return "i film musicali";
                    case "noir": return "i film noir";
                    case "politico": return "i film politici";
                    case "poliziesco": return "i film polizieschi";
                    case "psicologico": return "i film psicologici";
                    case "ragazzi": return "i film per ragazzi";
                    case "religioso": return "i film religiosi";
                    case "satirico": return "i film satirici";
                    case "sentimentale": return "i film sentimentali";
                    case "sociologico": return "i film sociologici";
                    case "sperimentale": return "i film sperimentali";
                    case "spionaggio": return "i film di spionaggio";
                    case "sportivo": return "i film sportivi";
                    case "storico": return "i film storici";
                    case "storico biografico": return "i film storico-biografici";
                    case "teatro": return "i film teatrali";
                    case "thriller": return "i thriller";
                    case "western": return "i film western";


                    default: return "i film di genere " + GenreName.ToLower();
                }
            }
        }


    }
}
