using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Web.Http.SelfHost;
using System.Net.Http;
using Ploeh.Samples.RunningJournalApi;
using System.Configuration;
using Simple.Data;
using System.Dynamic;
using Xunit.Extensions;

namespace Ploeh.Samples.RunningJournalApi.AcceptanceTests
{
    public class HomeJsonTests
    {
        [Fact]
        [UseDatabase]
        public void GetReturnsResponseWithCorrectStatusCode()
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("").Result;

                Assert.True(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }

        [Fact]
        [UseDatabase]
        public void GetReturnsJsonContent()
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("").Result;

                Assert.Equal(
                    "application/json",
                    response.Content.Headers.ContentType.MediaType);
                var json = response.Content.ReadAsJsonAsync().Result;
                Assert.NotNull(json);
            }
        }

        [Fact]
        [UseDatabase]
        public void PostEntrySucceeds()
        {
            using (var client = HttpClientFactory.Create())
            {
                var json = new
                {
                    time = DateTimeOffset.Now,
                    distance = 8500,
                    duration = TimeSpan.FromMinutes(44)
                };

                var response = client.PostAsJsonAsync("", json).Result;

                Assert.True(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }

        [Fact]
        [UseDatabase]
        public void AfterPostingEntryGetRootReturnsEntryInContent()
        {
            using (var client = HttpClientFactory.Create())
            {
                var json = new
                {
                    time = DateTimeOffset.Now,
                    distance = 8000,
                    duration = TimeSpan.FromMinutes(41)
                };
                var expected = json.ToJObject();
                client.PostAsJsonAsync("", json).Wait();

                var response = client.GetAsync("").Result;

                var actual = response.Content.ReadAsJsonAsync().Result;
                Assert.Contains(expected, actual.entries);
            }
        }

        [Theory]
        [UseDatabase]
        [InlineData("foo")]
        [InlineData("bar")]
        [InlineData("baz")]
        public void GetRootReturnsCorrectEntryFromDatabase(string userName)
        {
            dynamic entry = new ExpandoObject();
            entry.time = DateTimeOffset.Now;
            entry.distance = 6000;
            entry.duration = TimeSpan.FromMinutes(31);

            var expected = ((object)entry).ToJObject();

            var connStr =
                ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var db = Database.OpenConnection(connStr);
            db.User.Insert(UserName: userName);
            var userId = db.User.FindAllByUserName(userName).Single().UserId;
            entry.UserId = userId;
            db.JournalEntry.Insert(entry);

            using (var client = HttpClientFactory.Create(userName))
            {
                var response = client.GetAsync("").Result;

                var actual = response.Content.ReadAsJsonAsync().Result;
                Assert.Contains(expected, actual.entries);
            }
        }
    }
}
