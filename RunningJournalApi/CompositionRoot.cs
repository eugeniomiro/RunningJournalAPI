using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Simple.Data;

namespace Ploeh.Samples.RunningJournalApi
{
    public class CompositionRoot : IHttpControllerActivator
    {
        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            var db = CreateDb();
            return new JournalController(
                new SimpleWebTokenUserNameProjection(),
                new JournalEntriesQuery(db),
                new AddJournalEntryCommand(db));
        }

        private static dynamic CreateDb()
        {
            var connStr =
                ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            return Database.OpenConnection(connStr);
        }
    }
}
