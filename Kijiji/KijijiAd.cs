using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kijiji
{
    public class KijijiAd
    {
        public string Title { get; set; }
        public string AdUrl { get; set; }
        public List<string> Images { get; set; }
        public string Description { get; set; }
        public DateTime PostedAt { get; set; }
        public Dictionary<string, string> Info { get; set; }
    }
}
