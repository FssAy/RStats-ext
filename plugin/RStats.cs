using System;
using System.Runtime.InteropServices;
using Oxide.Core.Plugins;
using Oxide.Ext.RStats;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("RStats", "FssAy", "0.0.1")]

    class RStats : RustPlugin
    {
        private StatsClient client;

        void OnServerInitialized(bool initial)
        {
            client = new StatsClient();
        }
    }
}
