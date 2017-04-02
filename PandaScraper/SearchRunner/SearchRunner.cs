using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.WebJobs.Host;
using PandaScraper.Models;
using Kijiji;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System.Net.Http;

namespace PandaScraper.SearchRunner
{
    public class SearchRunner
    {
        //public static void Run(SavedSearch queueItem,
        //    DateTimeOffset expirationTime,
        //    DateTimeOffset insertionTime,
        //    DateTimeOffset nextVisibleTime,
        //    string queueTrigger,
        //    string id,
        //    string popReceipt,
        //    int dequeueCount,
        //    TraceWriter log)

        public static async Task Run(SavedSearch queueItem, CloudTable table, TraceWriter log)
        {
            //log.Info($"C# Queue trigger function processed: {queueItem}\n" +
            //    $"queueTrigger={queueTrigger}\n" +
            //    $"expirationTime={expirationTime}\n" +
            //    $"insertionTime={insertionTime}\n" +
            //    $"nextVisibleTime={nextVisibleTime}\n" +
            //    $"id={id}\n" +
            //    $"popReceipt={popReceipt}\n" +
            //    $"dequeueCount={dequeueCount}");

            var scraper = new KijijiScraper();
            var ads = await scraper.GetAds(queueItem.SearchUrl, queueItem.NewerThan);
            log.Info($"Found {ads.Count} items for search {queueItem.SearchUrl} at {DateTime.UtcNow}");

            if (!ads.Any()) return;
            
            var newestDate = ads.Max(x => x.PostedAt);
            
            var client = new HttpClient();

            await client.PostAsJsonAsync(queueItem.WebhookUrl, ads);

            queueItem.NewerThan = newestDate;
            var operation = TableOperation.Replace(queueItem);
            await table.ExecuteAsync(operation);
        }
    }
}