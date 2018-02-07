using Google.Apis.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using YTLiveLib.Classes.Client;
using YTLiveLib.Logger;

namespace YTLiveLib.Testing {
    public class YTLB {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(YTLB));

        internal YTClient ytClient;
        private JoinedChannel joinedChannel;

        public YTLB(string appname, string clientID, string clientsecret, string refreshtoken) {
            ytClient = new YTClient(appname, new GoogleCredentials() { ClientID = clientID, ClientSecret = clientsecret, RefreshToken = refreshtoken }, debug: true);
            ytClient.RegisterLogger(new Log4NetLogger());
            ytClient.OnConnectEvent += Client_OnConnectEvent;
            ytClient.OnDisconnectEvent += Client_OnDisconnectEvent;
            ytClient.OnReceiveMessageEvent += Client_OnReceiveMessageEvent;
        }

        private void Client_OnReceiveMessageEvent(object sender, Events.ReceiveMessageArgs e) {
            log.Info($"{e.ChatMessage.ChannelUser.DisplayName}: {e.ChatMessage.Message}");
            if(e.ChatMessage.Message.Equals("say something", StringComparison.CurrentCultureIgnoreCase)) {
                joinedChannel.SendMessage("hey, now i can write in chat :D").GetAwaiter().GetResult();
            }
        }

        private void Client_OnDisconnectEvent(object sender, Events.DisconnectArgs e) {
            
        }

        private void Client_OnConnectEvent(object sender, Events.ConnectArgs e) {

        }

        public void Connect() {
            ytClient.Connect();
        }

        public void Disconnect() {
            ytClient.Disconnect();
        }
    }
}
