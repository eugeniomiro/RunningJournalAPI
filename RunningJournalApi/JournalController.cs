using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Configuration;
using Simple.Data;

namespace Ploeh.Samples.RunningJournalApi
{
    public class JournalController : ApiController
    {
        private readonly IJournalEntriesQuery query;
        private readonly IAddJournalEntryCommand addCommand;

        public JournalController(IJournalEntriesQuery query, IAddJournalEntryCommand addCommand)
        {
            this.query = query;
            this.addCommand = addCommand;
        }

        public HttpResponseMessage Get()
        {
            var userName = this.GetUserName(this.Request);

            var entries = this.query.GetJournalEntries(userName);

            return this.Request.CreateResponse(
                HttpStatusCode.OK,
                new JournalModel
                {
                    Entries = entries.ToArray()
                });
        }

        public HttpResponseMessage Post(JournalEntryModel journalEntry)
        {
            var userName = this.GetUserName(this.Request);

            this.addCommand.AddJournalEntry(journalEntry, userName);

            return this.Request.CreateResponse();
        }

        private string GetUserName(HttpRequestMessage request)
        {
            SimpleWebToken swt;
            SimpleWebToken.TryParse(request.Headers.Authorization.Parameter, out swt);
            var userName = swt.Single(c => c.Type == "userName").Value;
            return userName;
        }
    }
}
