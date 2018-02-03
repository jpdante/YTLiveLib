using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using YTLiveLib.Classes.Client;
using YTLiveLib.Logger;

namespace YTLiveLib.Testing {
    public class Program {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));

        private YTClient client;

        public static void Main(string[] args) {
            var logRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            log.Info("Reading configuration file...");

            string json = File.ReadAllText("testing_config.json");
            var resource = JObject.Parse(json);

            log.Info("Loading YTLB...");
            YTLB ytlb = new YTLB(resource["ApplicationName"].Value<string>(), resource["ClientID"].Value<string>(), resource["SecretKey"].Value<string>(), resource["RefreshToken"].Value<string>());
            ytlb.Connect();
            Console.ReadLine();
            ytlb.Disconnect();
        }
    }
}
