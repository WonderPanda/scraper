using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using PandaScraper.Models;

namespace PandaScraper.ScrapeTimer
{
    public class ScrapeTimer
    {
        public static void Run(TimerInfo scrapeTimer, IQueryable<SavedSearch> tableBinding, ICollector<SavedSearch> queueBinding, TraceWriter log)
        {
            var searches = tableBinding.ToList();
            searches.ForEach(queueBinding.Add);
        }
    }
}