using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using PandaScraper.Models;

namespace PandaScraper.Searches
{
    public class AddSearch
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, IAsyncCollector<SavedSearch> tableBinding, TraceWriter log)
        {
            log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

            dynamic data = await req.Content.ReadAsAsync<object>();
            var searchUrl = data?.searchUrl.ToString();
            var webhookUrl = data?.webhookUrl.ToString();

            // TODO: Properly verify searchUrl and webhookUrl before persisting the search

            if (searchUrl == null || webhookUrl == null) return req.CreateResponse(HttpStatusCode.BadRequest);

            var savedSearch = new SavedSearch
            {
                PartitionKey = "SavedSearches",
                RowKey = WebUtility.UrlEncode(searchUrl),
                SearchUrl = searchUrl,
                WebhookUrl = webhookUrl,
                NewerThan = DateTime.UtcNow - TimeSpan.FromHours(5)
            };

            await tableBinding.AddAsync(savedSearch);

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}