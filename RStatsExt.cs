using System;
using Oxide.Core;
using Oxide.Core.Extensions;


namespace Oxide.Ext.RStats
{
    public class Printer
    {
        private static string tag = "[RStats]";

        public static void logInfo(string format, params object[] args)
        {
            Interface.Oxide.LogInfo("{0} {1}", tag, String.Format(format, args));
        }

        public static void logWarn(string format, params object[] args)
        {
            Interface.Oxide.LogWarning("{0} {1}", tag, String.Format(format, args));
        }

        public static void logErr(string format, params object[] args)
        {
            Interface.Oxide.LogError("{0} {1}", tag, String.Format(format, args));
        }
    }

    public class RStatsExt : Extension
    {
        public override string Name => "RStats";

        public override string Author => "FssAy";

        public override VersionNumber Version => new VersionNumber(0, 0, 1);

        public RStatsExt(ExtensionManager manager) : base(manager)
        {
        }

        public override void OnModLoad()
        {
            Printer.logInfo("loaded");
        }

        public override void Unload()
        {
            Printer.logInfo("unloaded");
        }
    }
}
