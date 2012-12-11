using System;
using System.Net.Http;

namespace Ploeh.Samples.RunningJournalApi
{
    public interface IUserNameProjection
    {
        string GetUserName(HttpRequestMessage request);
    }
}
