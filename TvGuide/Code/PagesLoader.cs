using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using antiufo.Threading;

namespace antiufo.TvGuide
{
    class PagesLoader
    {

        private EventWaitHandle _ewh;
        private int _totalTasks;
        private int _executedTasks;
        private BackgroundUIFunction<IEnumerable<TvProgram>> _op;

        private List<TvProgram> _programs;

        public IEnumerable<TvProgram> ExecuteLoadPrograms(BackgroundUIFunction<IEnumerable<TvProgram>> op, List<ChannelDay> pages)
        {
            _op = op;
            op.ReportProgress = false;
            op.UserText = "Caricamento programmi";
            _programs = new List<TvProgram>();
            var missingPages = new List<ChannelDay>();
            foreach (var page in pages)
            {
                var html = GetHtml(page.Url, true);
                if (html == null)
                {
                    missingPages.Add(page);
                }
                else
                {
                    page.SetHtml(html);
                }

            }

            if (missingPages.Any())
            {

                _ewh = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset);

                _totalTasks = missingPages.Count;
                _executedTasks = 0;


                op.Progress = 1 / (float)_totalTasks;

                op.ReportProgress = true;
                op.Progress = 0;

                foreach (var page in missingPages)
                {
                    var thisPage = page;
                    var task = new BackgroundFunction<string>(x => GetHtml(thisPage.Url, false));
                    task.Finished += new BackgroundOperationBase.FinishedEventHandler((o) => { task_Finished(o, thisPage); });
                    task.InvokeAsync();
                }


                _ewh.WaitOne();
                op.Progress = 1;
            }

            op.UserText = "Attendere...";

            foreach (var page in pages)
            {
                try
                {
                    _programs.AddRange(page.GetProgramsFromSavedHtmlIfAvailable());
                }
                catch(Exception ex)
                {
                    ex.Data.Add("OriginalStackTrace", ex.StackTrace);
                    this.exception = ex;

#if DEBUG
                    throw;
#endif
                }
            }

            if (_programs.Count < 20 && exception != null)
                throw exception;

            return _programs.OrderBy(x => x.Date);

        }

        private Exception exception;

        void task_Finished(BackgroundOperationBase sender, ChannelDay page)
        {
            var task = (BackgroundFunction<string>)sender;
            var executed = System.Threading.Interlocked.Increment(ref _executedTasks);


            _op.UserText = page.Channel.DisplayName;


            _op.Progress = (float)executed / _totalTasks;

            if (task.Completed)
            {
                page.SetHtml(task.Result);
            }
            else
            {

            }
            if (_executedTasks == _totalTasks) _ewh.Set();

        }


        private static System.Net.Cache.RequestCachePolicy _policyCacheOnly = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.CacheOnly);
        private static System.Net.Cache.RequestCachePolicy _policyStandard = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.Reload);

        static public HttpWebRequest CreateRequest(string url)
        {

            var req = (HttpWebRequest)System.Net.HttpWebRequest.Create(url);
            req.UserAgent = "Mozilla 4.0 (compatible; GetTvGuide .NET)";
            req.Timeout = 10000;
            req.ReadWriteTimeout = 10000;
            return (HttpWebRequest)req;
        }


        public static string GetHtml(string url, bool onlyFromCache)
        {
            var req = CreateRequest(url);
            req.CachePolicy = onlyFromCache ? _policyCacheOnly : _policyStandard;
            WebResponse resp;
            try
            {
                resp = req.GetResponse();
            }
            catch (WebException)
            {
                if (!onlyFromCache) throw;
                return null;
            }
            string html;
            using (var c = new StreamReader(resp.GetResponseStream(), System.Text.Encoding.Default, false))
            {
                html = c.ReadToEnd();
            }
            if (onlyFromCache)
            {
                var t = html.Trim();
                return null;
            }

            return html;

        }


    }
}
