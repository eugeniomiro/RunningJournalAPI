using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploeh.Samples.RunningJournalApi.UnitTests
{
    public class SimpleWebTokenUserNameProjectionTests
    {
        [Fact]
        public void GetUserNameFromProperSimpleWebTokenReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new SimpleWebTokenUserNameProjection();

            var request = new HttpRequestMessage();
            request.Headers.Authorization = 
                new AuthenticationHeaderValue(
                    "Bearer",
                    new SimpleWebToken(new Claim("userName", "foo")).ToString());
            // Exercise system
            var actual = sut.GetUserName(request);
            // Verify outcome
            Assert.Equal("foo", actual);
            // Teardown
        }
    }
}
