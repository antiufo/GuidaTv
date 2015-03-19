using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antiufo;
using System.IO;
using System.Net;

namespace UpdateChannelsList
{
    class Program
    {
        static void Main(string[] args)
        {
            var seenChannels = new List<string>();
      
            var links = "http://www.mymovies.it/tv/"
                .AsUri()
                .DownloadHtmlNode()
                .FindAll(".linkblu a")
                .Select(x => new { Name = x.GetAttributeValue("href", "").TryCapture(@"/tv/([^/]+)"), Title = x.GetText() })
                .Where(x=>x.Name!=null)
                .ToList();

            var iconsFolder=@"..\..\..\TvGuide\ChannelIcons";
            var w = new WebClient();
            foreach (var channel in links)
            {
                if (seenChannels.Contains(channel.Name)) continue;
                seenChannels.Add(channel.Name);
                var filename=Path.Combine(iconsFolder, channel.Name+".png");
                if (!File.Exists(filename)) {
                    w.DownloadFile("http://pad.mymovies.it/v7/img/canalitv/" + channel.Name + ".png", filename);
                }

                Console.WriteLine("new TvChannel(\"{0}\", \"{1}\"),", channel.Name, channel.Title);


            }

            Console.ReadKey();
        }
    }
}
