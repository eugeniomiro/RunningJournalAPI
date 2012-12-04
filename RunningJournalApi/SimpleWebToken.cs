using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Ploeh.Samples.RunningJournalApi
{
    public class SimpleWebToken : IEnumerable<Claim>
    {
        private readonly IEnumerable<Claim> claims;

        public SimpleWebToken(params Claim[] claims)
        {
            this.claims = claims;
        }

        public override string ToString()
        {
            return this.claims
                .Select(c => c.Type + "=" + c.Value)
                .DefaultIfEmpty(string.Empty)
                .Aggregate((x, y) => x + "&" + y);
        }

        public static bool TryParse(string tokenString, out SimpleWebToken token)
        {
            token = null;
            return false;
        }

        public IEnumerator<Claim> GetEnumerator()
        {
            return this.claims.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
