using System;
using System.Collections.Generic;

namespace Ploeh.Samples.RunningJournalApi
{
    public interface IJournalEntriesQuery
    {
        IEnumerable<JournalEntryModel> GetJournalEntries(string userName);
    }
}
