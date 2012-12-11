using System;

namespace Ploeh.Samples.RunningJournalApi
{
    public interface IAddJournalEntryCommand
    {
        void AddJournalEntry(JournalEntryModel journalEntry, string userName);
    }
}
