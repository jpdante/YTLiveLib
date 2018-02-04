using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using YTLiveLib.Classes.Client;
using YTLiveLib.Logger;

namespace YTLiveLib.Testing {
    public class Program {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));

        public static string videoid;
        private static YTLB client;

        public static void Main(string[] args) {
            var logRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            Console.Write("Enter the id of the video: ");
            videoid = Console.ReadLine();
            Console.WriteLine("");

            log.Info("Reading configuration file...");

            string json = File.ReadAllText("testing_config.json");
            var resource = JObject.Parse(json);

            log.Info("Loading YTLB...");
            client = new YTLB(resource["ApplicationName"].Value<string>(), resource["ClientID"].Value<string>(), resource["SecretKey"].Value<string>(), resource["RefreshToken"].Value<string>());

            client.ytClient.OnConnectEvent += YtClient_OnConnectEvent;

            client.Connect();
            Console.ReadLine();
            client.Disconnect();
        }

        private static void YtClient_OnConnectEvent(object sender, Events.ConnectArgs e) {
            string chatid = client.ytClient.GetChatID(videoid).GetAwaiter().GetResult();
            var joinedChannel = client.ytClient.JoinChannel(chatid);
            joinedChannel.SetChatDelay(Classes.Enum.ChatUpdateDelay.Normal);
        }
    }
}
