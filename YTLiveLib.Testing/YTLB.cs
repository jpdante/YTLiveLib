using Google.Apis.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using YTLiveLib.Classes.Client;
using YTLiveLib.Logger;

namespace YTLiveLib.Testing {
    public class YTLB {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(YTLB));

        private YTClient client;

        public YTLB(string appname, string clientID, string clientsecret, string refreshtoken) {
            client = new YTClient(appname, new GoogleCredentials() { ClientID = clientID, ClientSecret = clientsecret, RefreshToken = refreshtoken }, debug: true);
            client.RegisterLogger(new Log4NetLogger());
            client.OnConnectEvent += Client_OnConnectEvent;
            client.OnDisconnectEvent += Client_OnDisconnectEvent;
            client.OnReceiveMessageEvent += Client_OnReceiveMessageEvent;
        }

        private void Client_OnReceiveMessageEvent(object sender, Events.OnReceiveMessageArgs e) {
            log.Info($"Received Message: {e.Message.Message}");
        }

        private void Client_OnDisconnectEvent(object sender, Events.OnDisconnectArgs e) {
            
        }

        private void Client_OnConnectEvent(object sender, Events.OnConnectArgs e) {
            client.JoinStreamChannel("#LIVE 14 - PUBG | Voltando a ORIGEM");
        }

        public void Connect() {
            client.Connect();
        }

        public void Disconnect() {
            client.Disconnect();
        }
    }
}
