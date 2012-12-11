using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.RunningJournalApi
{
    public class SimpleWebTokenUserNameProjection : IUserNameProjection
    {
        public string GetUserName(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            if (request.Headers.Authorization == null)
                return null;

            SimpleWebToken swt;
            SimpleWebToken.TryParse(request.Headers.Authorization.Parameter, out swt);
            var userName = swt.Single(c => c.Type == "userName").Value;
            return userName;
        }
    }
}
