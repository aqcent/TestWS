using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TestWS.Agent
{
    public class CacheWarmingUpAgent
    {
        private const int WorkersCount = 1;
        public void Run()
        {
            var urlsConfig = Constants.UrlsToWarmUp;
            if (urlsConfig != null && urlsConfig.AllKeys.Any())
            {
                var urls = urlsConfig.AllKeys.Select(key => urlsConfig[key]).ToList();
                var queue = new ConcurrentQueue<string>(urls);
                var workers = StartWorker(queue, WorkersCount);
                Task.WaitAll(workers.ToArray());
            }
        }

        private IEnumerable<Task> StartWorker(ConcurrentQueue<string> queue, int workersCount)
        {
            var tasks = new List<Task>();
            for (int i = 0; i < workersCount; i++)
            {
                var worker = new CacheWarmingUpThread(queue);
                var workerTask = Task.Run(() => worker.Run());
                tasks.Add(workerTask);
            }
            return tasks;
        }
    }
}