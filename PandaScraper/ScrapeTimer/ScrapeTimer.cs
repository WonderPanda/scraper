using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace PandaScraper.ScrapeTimer
{
    public class ScrapeTimer
    {
        public static void Run(TimerInfo scrapeTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}