using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using YTLiveLib.Classes.API;
using YTLiveLib.Classes.Client;

namespace YTLiveLib.Internal {
    public class ChatEventsListener {

        public delegate void OnGetMessages(object sender, OnGetMessagesArgs e);
        public event OnGetMessages OnGetMessagesEvent;

        private Timer timer;
        private YouTubeAPI youTubeAPI;
        private string chatID;

        private int errors = 0;

        public ChatEventsListener(YouTubeAPI youtubeapi, string chatid) {
            youTubeAPI = youtubeapi;
            chatID = chatid;
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            OnGetMessagesEvent += ChatEventsListener_OnGetMessagesEvent;
        }

        private void ChatEventsListener_OnGetMessagesEvent(object sender, OnGetMessagesArgs e) { }

        public ChatEventsListener Start() {
            errors = 0;
            timer.Start();
            return this;
        }

        public ChatEventsListener Stop() {
            timer.Stop();
            return this;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
            try {
                List<ChatMessage> messages = youTubeAPI.getChatMessages(chatID).GetAwaiter().GetResult();
                OnGetMessagesEvent(this, new OnGetMessagesArgs(messages));
            } catch(Exception ex) {
                YTClient.Log(Logger.LogLevel.Error, ex.Message, ex);
                errors++;
                if(errors >= 3) {
                    YTClient.Log(Logger.LogLevel.Error, "Limit of errors reached, disconnecting from chat!");
                    Stop();
                }
            }
        }
    }
}
