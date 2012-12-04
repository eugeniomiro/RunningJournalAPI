using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Ploeh.Samples.RunningJournalApi;
using System.Security.Claims;

namespace Ploeh.Samples.RunningJournalApi.UnitTests
{
    public class SimpleWebTokenTests
    {
        [Fact]
        public void SutIsIteratorOfClaims()
        {
            var sut = new SimpleWebToken();
            Assert.IsAssignableFrom<IEnumerable<Claim>>(sut);
        }

        [Fact]
        public void SutYieldsInjectedClaims()
        {
            var expected = new[]
            {
                new Claim("foo", "bar"),
                new Claim("baz", "qux"),
                new Claim("quux", "corge")
            };
            var sut = new SimpleWebToken(expected);
            Assert.True(expected.SequenceEqual(sut));
            Assert.True(
                expected.Cast<object>().SequenceEqual(
                    ((System.Collections.IEnumerable)sut).OfType<object>()));
        }

        [Fact]
        public void ToStringReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new SimpleWebToken(new Claim("foo", "bar"));
            // Exercise system
            var actual = sut.ToString();
            // Verify outcome
            Assert.Equal("foo=bar", actual);
            // Teardown
        }
    }
}
