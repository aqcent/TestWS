using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;

namespace TestWS.Agent
{
    public class CacheWarmingUpThread
    {
        private readonly ConcurrentQueue<string> _urlsQueue;

        public CacheWarmingUpThread(ConcurrentQueue<string> urlsQueue)
        {
            _urlsQueue = urlsQueue;
        }

        public void Run()
        {
            while (_urlsQueue.TryDequeue(out string url))
            {
                ProcessItem(url);
            }
        }

        private void ProcessItem(string url)
        {
            var stopWatch = new Stopwatch();
            try
            {
                stopWatch.Start();
                new WebClient().DownloadData(new Uri(Constants.Domain + url));
            }
            finally
            {
                stopWatch.Stop();
            }

        }
    }
}