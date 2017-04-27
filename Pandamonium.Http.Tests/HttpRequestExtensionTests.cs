using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pandamonium.Http.Tests
{
    [TestFixture]
    public class HttpRequestExtensionTests
    {
        [Test]
        public void Test()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("Authorization", "thisIsMyFakeJwt");

            var authorized = request.Authorized("fakeSecret");

            "".ToString();
        }
    }
}
