using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using antiufo.QuickTextParsing;
using Antiufo;

namespace antiufo.TvGuide
{
    public class DetailsInfo
    {

        public string ReviewText { get; private set; }
        public string ImageUrl { get; private set; }
        public string LegacyCode { get; private set; }
        public bool HasTrailer { get; private set; }




        internal DetailsInfo(string html, string id)
        {

            ReviewText = html.BetweenS("<p>", "</p>").Trim();

            ReviewText = Utils.TextFromHtml(ReviewText).Trim();
            var k = new StringBuilder(ReviewText);
            k.Replace((char)146, '\'');
            k.Replace((char)147, '"');
            k.Replace((char)148, '"');
            ReviewText = k.ToString();

            ImageUrl = html.BetweenS("padding: 3px; margin: 5px;\" src=\"", "\"", true);
            if (ImageUrl == null)
            {
                ImageUrl = html.BetweenS("padding:3px; margin:5px;\" src=\"", "\"", true);
            }
            if (ImageUrl != null) LegacyCode = ImageUrl.TryCapture(@"filmclub/(\d+/\d+/\d+)/");
            HasTrailer = LegacyCode != null && html.Contains("rec_link_disattivo\"><a title=\"Trailer");

        }

    }
}
