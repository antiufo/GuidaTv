using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using antiufo;
using System.Net;
using System.IO;
using System.Drawing;

namespace antiufo.TvGuide
{
    [Serializable]
    public class TvChannel
    {


        private string _name;
        private string _displayName;
        public TvChannel(string name, string displayName)
        {
            _name = name;
            _displayName = displayName;

        }

        public string Name
        {
            get
            {
                return _name;
            }
        }


        


        private static IList<TvChannel> _channels;
        public static IList<TvChannel> AllChannels
        {
            get
            {
                if (_channels == null)
                {
                    _channels = new List<TvChannel>();
                    var ch = new TvChannel[]{

new TvChannel("raiuno", "RaiUno"),
new TvChannel("raidue", "RaiDue"),
new TvChannel("raitre", "RaiTre"),
new TvChannel("rete4", "Rete4"),
new TvChannel("canale5", "Canale 5"),
new TvChannel("italia1", "Italia 1"),
new TvChannel("la7", "La7"),
new TvChannel("mtv", "MTV"),
new TvChannel("deejaytv", "Deejay TV"),
new TvChannel("7gold", "7Gold"),
new TvChannel("odeon", "Odeon"),
new TvChannel("rai4", "Rai4"),
new TvChannel("rai5", "Rai5"),
new TvChannel("raigulp", "Rai Gulp"),
new TvChannel("raiyoyo", "Rai Yoyo"),
new TvChannel("raipremium", "Rai Premium"),
new TvChannel("raimovie", "Rai Movie"),
new TvChannel("rainews", "Rai News"),
new TvChannel("raisport1", "Rai Sport 1"),
new TvChannel("raistoria", "Rai Storia"),
new TvChannel("la5", "La5"),
new TvChannel("italia2", "Italia 2"),
new TvChannel("mediasetextra", "Mediaset Extra"),
new TvChannel("iris", "Iris"),
new TvChannel("boing", "Boing"),
new TvChannel("cartoonito", "Cartoonito"),
new TvChannel("mtvmusic", "MTV Music"),
new TvChannel("la7d", "La7d"),
new TvChannel("cielo", "Cielo"),
new TvChannel("virginradiotv", "Virgin Radio TV"),
new TvChannel("k2", "K2"),
new TvChannel("frisbee", "Frisbee"),
new TvChannel("animegold", "Anime Gold"),
new TvChannel("tv2000", "Tv 2000"),
new TvChannel("realtime", "Real Time"),
new TvChannel("dmax", "Dmax"),
new TvChannel("arturo", "Arturo"),
new TvChannel("nuvolari", "Nuvolari"),
new TvChannel("supertennis", "Super Tennis"),
new TvChannel("sportitalia2", "Sport Italia 2"),
new TvChannel("sportitalia", "Sport Italia"),
new TvChannel("pokeritalia24", "Poker Italia 24"),
new TvChannel("premiumcinema", "Premium Cinema"),
new TvChannel("premiumcinemaemotion", "Premium Cinema Emotion"),
new TvChannel("premiumcinemaenergy", "Premium Cinema Energy"),
new TvChannel("studiouniversal", "Studio Universal"),
new TvChannel("premiumcinemacomedy", "Premium Cinema Comedy"),
new TvChannel("premiumcrime", "Premium Crime"),
new TvChannel("joi", "Joi"),
new TvChannel("mya", "Mya"),
new TvChannel("steel", "Steel"),
new TvChannel("focus", "Focus"),
new TvChannel("laeffe", "laeffe"),
new TvChannel("giallo", "Giallo"),
new TvChannel("disneychannel", "Disney Channel"),
new TvChannel("playhousedisney", "Disney Junior"),
new TvChannel("cartoonnetwork", "Cartoon Network"),
new TvChannel("bbcknowledge", "BBC Knowledge"),
new TvChannel("discoveryworld", "Discovery World"),
new TvChannel("qvc", "QVC"),
new TvChannel("skyuno", "Sky Uno"),
new TvChannel("skycinema1", "Sky Cinema 1"),
new TvChannel("skycinema24", "Sky Cinema + 24"),
new TvChannel("skycinemafamily", "Sky Cinema Family"),
new TvChannel("skycinemamax", "Sky Cinema Max"),
new TvChannel("skycinemahits", "Sky Cinema Hits"),
new TvChannel("skycinemapassion", "Sky Cinema Passion"),
new TvChannel("skycinemacomedy", "Sky Cinema Comedy"),
new TvChannel("skycinemaclassics", "Sky Cinema Classics"),
new TvChannel("bbcworld", "BBC World News"),
new TvChannel("cult", "Cult"),
new TvChannel("france24fr", "France 24 FR"),
new TvChannel("france24uk", "France 24 UK"),
new TvChannel("mgmchannel", "MGM Channel"),
new TvChannel("foxcrime", "Fox Crime"),
new TvChannel("fox", "Fox"),
new TvChannel("foxlife", "Fox Life"),
new TvChannel("foxretro", "Fox Retro"),
new TvChannel("comedycentral", "Comedy Central"),
new TvChannel("lei", "Lei"),
new TvChannel("ladychannel", "Lady Channel"),
new TvChannel("divauniversal", "Diva Universal"),
new TvChannel("axn", "AXN"),
new TvChannel("axnscifi", "Axn Sci-Fi"),
new TvChannel("horrorchannel", "Horror Channel"),
new TvChannel("man-ga", "Man-ga"),
new TvChannel("nichelodeon", "Nichelodeon"),
new TvChannel("nationalgeographicchannel", "National Geographic Channel"),
new TvChannel("discoverychannel", "Discovery Channel"),
new TvChannel("historychannel", "History Channel"),
new TvChannel("skysport1", "Sky Sport 1"),
new TvChannel("skysport2", "Sky Sport 2"),
new TvChannel("skysport3", "Sky Sport 3"),
new TvChannel("skysportextra", "Sky Sport Extra"),
new TvChannel("skysupercalcio", "Sky Supercalcio"),
new TvChannel("eurosport", "Eurosport"),





                    };
                    _channels = ch.ToList();

                }
                return _channels;
            }
        }


        public string DisplayName
        {
            get
            {
                if (_displayName != null) return _displayName;
                return _name;
            }
        }










        public static bool operator ==(TvChannel ch1, TvChannel ch2)
        {
            if (Object.ReferenceEquals(ch1, null) && Object.ReferenceEquals(ch2, null)) return true;
            if (Object.ReferenceEquals(ch1, null) ^ Object.ReferenceEquals(ch2, null)) return false;

            return ch1.Equals(ch2);
        }

        public static bool operator !=(TvChannel ch1, TvChannel ch2)
        {
            return !(ch1 == ch2);
        }


        public override bool Equals(object obj)
        {
            var item2 = obj as TvChannel;
            if (item2 == null) return false;
            return item2._name == this._name;
        }


        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }


        [NonSerialized]
        private Image _logo;
        public Image Logo
        {
            get
            {
                if (_logo == null) _logo = (Image)antiufo.TvGuide.Properties.Resources.ResourceManager.GetObject(_name);
                return _logo;
            }
        }



    }





}
