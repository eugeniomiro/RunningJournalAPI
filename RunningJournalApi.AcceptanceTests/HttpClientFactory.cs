using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Web.Http.SelfHost;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Ploeh.Samples.RunningJournalApi.AcceptanceTests
{
    public class HttpClientFactory
    {
        public static HttpClient Create()
        {
            var baseAddress = new Uri("http://localhost:8765");
            var config = new HttpSelfHostConfiguration(baseAddress);
            new Bootstrap().Configure(config);
            var server = new HttpSelfHostServer(config);
            var client = new HttpClient(server);
            try
            {
                client.BaseAddress = baseAddress;
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        new SimpleWebToken(new Claim("userName", "foo")).ToString());
                return client;
            }
            catch
            {
                client.Dispose();
                throw;
            }
        }
    }
}
