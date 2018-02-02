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
            Console.WriteLine("Type the ClientID:");
            string clientid = Console.ReadLine();
            Console.WriteLine("Type the Client Secret:");
            string clientsecret = Console.ReadLine();
            Console.WriteLine("Type the Refresh Token:");
            string refreshtoken = Console.ReadLine();
            Console.WriteLine("Type the App Name:");
            string appname = Console.ReadLine();


            var logRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            log.Info("Loading libraries...");
            Program program = new Program(appname, clientid, clientsecret, refreshtoken);
            program.Start();
            Console.ReadLine();
            program.Stop();
            Console.ReadLine();
        }

        public Program(string appname, string clientID, string clientsecret, string refreshtoken) {
            GoogleCredentials googleCredentials = new GoogleCredentials() {
                ClientID = clientID,
                ClientSecret = clientsecret,
                RefreshToken = refreshtoken
            };
            client = new YTClient(appname, googleCredentials);
            log.Info("Adding logger...");
            client.RegisterLogger(new Log4NetLogger());
            log.Info("Adding events...");
            client.OnConnectEvent += Client_OnConnectEvent;
            client.OnDisconnectEvent += Client_OnDisconnectEvent;
        }

        private void Client_OnDisconnectEvent(object sender, Events.OnDisconnectArgs e) {
            log.Info("YTLiveLib Disconnected!");
        }

        private void Client_OnConnectEvent(object sender, Events.OnConnectArgs e) {
            log.Info("YTLiveLib Connected!");
            client.JoinStreamChannel("#LIVE 34 - Rules of Survival | Dicas ao Vivo");
        }

        public void Start() {
            log.Info("Starting...");
            client.Connect();
        }

        public void Stop() {
            log.Info("Stopping...");
            client.Disconnect();
        }
    }
}
