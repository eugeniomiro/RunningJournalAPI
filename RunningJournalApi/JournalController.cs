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
        private readonly dynamic db;

        public JournalController()
        {
            this.db = CreateDb();
        }

        public HttpResponseMessage Get()
        {
            SimpleWebToken swt;
            SimpleWebToken.TryParse(this.Request.Headers.Authorization.Parameter, out swt);
            var userName = swt.Single(c => c.Type == "userName").Value;

            var entries = this.db.JournalEntry
                .FindAll(this.db.JournalEntry.User.UserName == userName)
                .ToArray<JournalEntryModel>();

            return this.Request.CreateResponse(
                HttpStatusCode.OK,
                new JournalModel
                {
                    Entries = entries
                });
        }

        public HttpResponseMessage Post(JournalEntryModel journalEntry)
        {
            SimpleWebToken swt;
            SimpleWebToken.TryParse(this.Request.Headers.Authorization.Parameter, out swt);
            var userName = swt.Single(c => c.Type == "userName").Value;

            var userId = this.db.User.Insert(UserName: userName).UserId;

            this.db.JournalEntry.Insert(
                UserId: userId,
                Time: journalEntry.Time,
                Distance: journalEntry.Distance,
                Duration: journalEntry.Duration);

            return this.Request.CreateResponse();
        }

        private static dynamic CreateDb()
        {
            var connStr =
                ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            return Database.OpenConnection(connStr);
        }
    }
}
