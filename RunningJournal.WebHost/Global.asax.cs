using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;
using Ploeh.Samples.RunningJournalApi;

namespace Ploeh.Samples.RunningJournal.WebHost
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new Bootstrap().Configure(GlobalConfiguration.Configuration);
        }
    }
}