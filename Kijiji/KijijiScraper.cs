using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using System.Text.RegularExpressions;

namespace Kijiji
{
    public class KijijiScraper
    {
        public async Task<List<KijijiAdBase>> GetShallowAds(string address, DateTime newerThan)
        {
            var config = Configuration.Default.WithDefaultLoader();

            var document = await BrowsingContext.New(config).OpenAsync(address);

            var filteredAds = document.QuerySelectorAll("item")
                .Select(item =>
                {
                    var postedAt = DateTime.Parse(item.QuerySelector("pubDate").TextContent).ToUniversalTime();
                    var adUrl = item.QuerySelector("guid").TextContent;
                    var title = item.QuerySelector("title").TextContent;
                    return new KijijiAdBase
                    {
                        AdUrl = adUrl,
                        Title = title,
                        PostedAt = postedAt   
                    };
                })
                .Where(item => item.PostedAt > newerThan)
                .ToList();

            return filteredAds;
        }

        public async Task<List<KijijiAd>> GetAds(string address, DateTime newerThan)
        {
            var shallowAds = await GetShallowAds(address, newerThan);
            var config = Configuration.Default.WithDefaultLoader();

            var adDocuments = await Task.WhenAll(shallowAds.Select(x => BrowsingContext.New(config).OpenAsync(x.AdUrl)));
            var zipped = shallowAds.Zip(adDocuments, (shallow, adDoc) =>
            {
                return new
                {
                    shallow.AdUrl,
                    shallow.PostedAt,
                    adDoc
                };
            });

            var ads = zipped.Select(item =>
            {
                var ad = new KijijiAd
                {
                    Title = item.adDoc.QuerySelector("span[itemprop=name]").TextContent.Trim(),
                    Description = item.adDoc.QuerySelector("span[itemprop=description]").TextContent.Trim(),
                    AdUrl = item.AdUrl,
                    PostedAt = item.PostedAt
                };

                var images = item.adDoc.QuerySelectorAll("img[itemprop=image]")
                    //.Select(x => Regex.Replace(x.GetAttribute("src"), "$_\\d+\\.JPG$/", "/$_57.JPG"))
                    .Select(x => x.GetAttribute("src"))
                    .ToList();

                ad.Images = images;

                ad.Info = item.adDoc.QuerySelectorAll("table.ad-attributes tr")
                    .Select(ele =>
                    {
                        var field = ele.QuerySelector("th")?.TextContent.Trim();
                        var value = ele.QuerySelector("td")?.TextContent.Trim();

                        return new
                        {
                            field,
                            value
                        };
                    })
                    .Where(x => x.field != null)
                    .ToDictionary(x => x.field, x => x.value);

                return ad;
            }).ToList();

            return ads;
        }
    }
}
