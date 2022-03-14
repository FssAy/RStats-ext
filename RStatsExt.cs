using System;
using Oxide.Core;
using Oxide.Core.Extensions;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

namespace Oxide.Ext.RStats
{
    public class Argument
    {
        public string data { get; }

        public Argument(string key, string value)
        {
            data = String.Format("{0}:{1}", key, value);
        }
    }

    public class Packet
    {
        public byte[] bytes { get; }

        public enum Target
        {
            InitInfo,
            CurrentPlayers,
        }

        public Packet(Target target, params Argument[] data)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add((byte) target);

            foreach (Argument entry in data)
            {
                bytes.AddRange(Encoding.UTF8.GetBytes(entry.data));
                bytes.Add(0x00);
            }

            this.bytes = bytes.ToArray();
        }

        public void Send(UdpClient udpClient, string ip, UInt16 port)
        {
            udpClient.Send(bytes, bytes.Length, ip, port);
        }
    }

    public class StatsClient
    {
        private static bool isRunning = false;
        private UdpClient udpClient = new UdpClient();
        public string ip { get; }
        public UInt16 port { get; }

        public StatsClient()
        {
            if (isRunning)
            {
                Printer.logErr("StatsClient is already running");
                return;
            }

            isRunning = true;

            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] buff = wc.DownloadData("https://raw.githubusercontent.com/DmitrijVC/RStats-ext/master/oth/master");
            string masterServer = Encoding.ASCII.GetString(buff);

            string[] split = masterServer.Split(':');
            ip = split[0];
            try
            {
                port = UInt16.Parse(split[1]);
            }
            catch (Exception)
            {
                port = 28014;
            }

            if (port == 0 || ip == "0.0.0.0")
            {
                Printer.logErr("master server isn't operating, try again later");
                return;
            }

            TcpClient tmpTCP = new TcpClient(ip, port);
            NetworkStream stream = tmpTCP.GetStream();

            var packet = new Packet(
                Packet.Target.InitInfo,
                new Argument("ip", ConVar.Server.ip),
                new Argument("port", ConVar.Server.port.ToString()),
                new Argument("hostname", ConVar.Server.hostname),
                new Argument("level", ConVar.Server.level),
                new Argument("seed", ConVar.Server.seed.ToString()),
                new Argument("world_size", ConVar.Server.worldsize.ToString()),
                new Argument("max_players", ConVar.Server.maxplayers.ToString()),
                new Argument("tags", ConVar.Server.tags),
                new Argument("url", ConVar.Server.url)
            );

            stream.Write(packet.bytes, 0, packet.bytes.Length);
            stream.Close();
        }

        ~StatsClient()
        {
            isRunning = false;
        }

        public void SendPacket(Packet packet)
        {
            packet.Send(udpClient, ip, port);
        }
    }

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
