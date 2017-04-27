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
        public static async Task Run(SavedSearch queueItem, CloudTable table, TraceWriter log)
        {
            var scraper = new KijijiScraper();
            var deepSearch = queueItem?.IsDeepSearch ?? false;

            var ads = deepSearch ? 
                (await scraper.GetAds(queueItem.SearchUrl, queueItem.NewerThan)).Cast<KijijiAdBase>() :
                (await scraper.GetShallowAds(queueItem.SearchUrl, queueItem.NewerThan));

            var adList = ads.ToList();
            
            log.Info($"Found {adList.Count} items for search {queueItem.SearchUrl} at {DateTime.UtcNow}");

            if (!adList.Any()) return;
            
            var newestDate = adList.Max(x => x.PostedAt);

            var client = new HttpClient();

            await client.PostAsJsonAsync(queueItem.WebhookUrl, adList);

            queueItem.NewerThan = newestDate;
            var operation = TableOperation.Replace(queueItem);
            await table.ExecuteAsync(operation);
        }
    }
}