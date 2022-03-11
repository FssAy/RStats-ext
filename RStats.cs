using System;
using Oxide.Core;
using Oxide.Core.Extensions;


namespace Oxide.Ext.RStats
{
    class Printer
    {
        public static void logInfo(string msg)
        {
            Interface.Oxide.LogInfo("[RStats] {0}", msg);
        }
    }

    public class RStats : Extension
    {
        public override string Name => "RStats";

        public override string Author => "FssAy";

        public override VersionNumber Version => new VersionNumber(0, 0, 1);

        public RStats(ExtensionManager manager) : base(manager)
        {
        }

        public override void OnModLoad()
        {
            Printer.logInfo("loaded");
        }

        public override void OnShutdown()
        {
            Printer.logInfo("unloaded");
        }
    }
}
