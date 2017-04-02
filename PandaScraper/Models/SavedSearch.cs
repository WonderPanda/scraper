using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PandaScraper.Models
{
    public class SavedSearch : TableEntity
    {
        public string SearchUrl { get; set; }
        public string WebhookUrl { get; set; }
        public DateTime NewerThan { get; set; }
    }
}