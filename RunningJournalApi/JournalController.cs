﻿using System;
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
        private readonly IUserNameProjection userNameProjection;
        private readonly IJournalEntriesQuery query;
        private readonly IAddJournalEntryCommand addCommand;

        public JournalController(IJournalEntriesQuery query, IAddJournalEntryCommand addCommand)
        {
            this.userNameProjection = new SimpleWebTokenUserNameProjection();
            this.query = query;
            this.addCommand = addCommand;
        }

        public HttpResponseMessage Get()
        {
            var userName = this.userNameProjection.GetUserName(this.Request);

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
            var userName = this.userNameProjection.GetUserName(this.Request);

            this.addCommand.AddJournalEntry(journalEntry, userName);

            return this.Request.CreateResponse();
        }
    }
}
