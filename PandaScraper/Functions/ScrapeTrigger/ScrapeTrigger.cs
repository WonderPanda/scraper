using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace PandaScraper.Functions.ScrapeTrigger
{
    public class ScrapeTrigger
    {
        public static void Run(TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}