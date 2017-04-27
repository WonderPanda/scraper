using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kijiji
{
    public class KijijiAd : KijijiAdBase
    {
        public List<string> Images { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Info { get; set; }
    }
}
