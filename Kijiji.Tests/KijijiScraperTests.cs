using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Kijiji.Tests
{
    [TestFixture]
    public class KijijiScraperTests
    {
        [Test]
        public async Task Test()
        {
            var scraper = new KijijiScraper();
            var newerThan = DateTime.Now - TimeSpan.FromHours(12.0);
            var ads = await scraper.GetAds("http://www.kijiji.ca/rss-srp-cars-trucks/london/toyota/k0c174l1700214?price=3500__6500", newerThan);
            "".ToString();
        }
    }
}
