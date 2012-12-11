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
        private readonly JournalEntriesQuery query;
        private readonly AddJournalEntryCommand addCommand;

        public JournalController()
        {
            this.db = CreateDb();
            this.query = new JournalEntriesQuery(this.db);
            this.addCommand = new AddJournalEntryCommand(this.db);
        }

        public HttpResponseMessage Get()
        {
            var userName = this.GetUserName();

            var entries = this.query.GetJournalEntries(userName);

            return this.Request.CreateResponse(
                HttpStatusCode.OK,
                new JournalModel
                {
                    Entries = entries.ToArray()
                });
        }

        private class JournalEntriesQuery
        {
            private readonly dynamic db;

            public JournalEntriesQuery(dynamic db)
            {
                this.db = db;
            }

            public IEnumerable<JournalEntryModel> GetJournalEntries(string userName)
            {
                var entries = this.db.JournalEntry
                    .FindAll(this.db.JournalEntry.User.UserName == userName)
                    .ToArray<JournalEntryModel>();
                return entries;
            }
        }

        public HttpResponseMessage Post(JournalEntryModel journalEntry)
        {
            var userName = this.GetUserName();

            this.addCommand.AddJournalEntry(journalEntry, userName);

            return this.Request.CreateResponse();
        }

        private class AddJournalEntryCommand
        {
            private readonly dynamic db;

            public AddJournalEntryCommand(dynamic db)
            {
                this.db = db;
            }

            public void AddJournalEntry(JournalEntryModel journalEntry, string userName)
            {
                var userId = this.db.User
                    .FindAllByUserName(userName)
                    .Select(this.db.User.UserId)
                    .ToScalarOrDefault<int>();
                if (userId == 0)
                    userId = this.db.User.Insert(UserName: userName).UserId;

                this.db.JournalEntry.Insert(
                    UserId: userId,
                    Time: journalEntry.Time,
                    Distance: journalEntry.Distance,
                    Duration: journalEntry.Duration);
            }
        }

        private string GetUserName()
        {
            SimpleWebToken swt;
            SimpleWebToken.TryParse(this.Request.Headers.Authorization.Parameter, out swt);
            var userName = swt.Single(c => c.Type == "userName").Value;
            return userName;
        }

        private static dynamic CreateDb()
        {
            var connStr =
                ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            return Database.OpenConnection(connStr);
        }
    }
}
