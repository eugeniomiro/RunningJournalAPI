using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Web;
using Ploeh.Samples.RunningJournalApi;

namespace Ploeh.Samples.RunningJournal.WebHost
{
    [RunInstaller(true)]
    public class DatabaseInstaller : Installer
    {
        public override void Install(IDictionary stateSaver)
        {
            var connectionString = this.Context.Parameters["ConnectionString"];
            new Bootstrap().InstallDatabase(connectionString);

            base.Install(stateSaver);
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            var connectionString = this.Context.Parameters["ConnectionString"];
            new Bootstrap().UninstallDatabase(connectionString);
        }
    }
}