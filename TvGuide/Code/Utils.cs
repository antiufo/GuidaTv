using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antiufo;

namespace antiufo.TvGuide
{
    class Utils
    {
        public static string TextFromHtml(string html)
        {
            StringBuilder sb = new StringBuilder(html.Length);
            bool insideTag = false;
            bool insideComment = false;
            for (int i = 0; i < html.Length; i++)
            {
                char ch = html[i];
                switch (ch)
                {
                    case '<':
                        insideTag = true;
                        if (html.ContainsIndex(i+3) && html[i + 1] == '!' && html[i + 2] == '-' && html[i + 3] == '-')
                        {
                            insideComment = true;
                        }
                        break;

                    case '>':
                        if (insideComment&& html.ContainsIndex(i - 2) && html[i - 1] == '-' && html[i - 2] == '-')
                        {
                            insideComment = false;
                        }
                        insideTag = false;
                        break;

                    default:
                        if (!insideTag && !insideComment)
                        {
                            sb.Append(ch);
                        }
                        break;
                }
            }
            return antiufo.QuickTextParsing.SimpleWebRequest.UnescapeHTML(sb.ToString());
        }
    }
}
