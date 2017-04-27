using JWT;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pandamonium.Http
{
    public static class HttpRequestExtensions
    {
        private static Func<HttpRequestMessage, string> _defaultTokenGetter = (req) => req.Headers.Authorization?.ToString();

        public static bool Authorized(this HttpRequestMessage request, string secret, Func<HttpRequestMessage, string> tokenGetter = null)
        {
            tokenGetter = tokenGetter ?? _defaultTokenGetter;

            var token = tokenGetter(request);

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(token, secret, verify: true);
                Console.WriteLine(json);
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
            }

            return true;
        }
    }
}
