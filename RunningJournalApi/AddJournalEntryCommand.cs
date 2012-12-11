using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploeh.Samples.RunningJournalApi
{
    public class AddJournalEntryCommand : IAddJournalEntryCommand
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
}
