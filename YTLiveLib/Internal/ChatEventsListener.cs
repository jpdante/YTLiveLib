using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using YTLiveLib.Classes.API;
using YTLiveLib.Classes.Client;
using YTLiveLib.Classes.Enum;
using YTLiveLib.Internal.Events;

namespace YTLiveLib.Internal {
    public class ChatEventsListener {

        internal delegate void OnReceiveChatEvents(object sender, GetChatEventsArgs e);
        internal event OnReceiveChatEvents OnGetChatEvents_Event;

        private Guid guid;
        private Timer timer;
        private ChatStatus chatStatus;
        private YouTubeAPI youTubeAPI;
        private string chatID;

        private int errors = 0;

        public ChatEventsListener(YouTubeAPI youtubeapi, string chatid, ChatUpdateDelay delay) {
            guid = Guid.NewGuid();
            chatStatus = ChatStatus.Disabled;
            youTubeAPI = youtubeapi;
            chatID = chatid;
            timer = new Timer();
            timer.Interval = 5000;
            timer.Elapsed += Timer_Elapsed;
            setDelay(delay);
            OnGetChatEvents_Event += ChatEventsListener_OnGetChatEvents_Event; ;
        }

        private void ChatEventsListener_OnGetChatEvents_Event(object sender, GetChatEventsArgs e) {}

        public void setDelay(ChatUpdateDelay delay) {
            switch(delay) {
                case ChatUpdateDelay.UltraFast:
                    timer.Interval = 500;
                    break;
                case ChatUpdateDelay.VeryFast:
                    timer.Interval = 750;
                    break;
                case ChatUpdateDelay.Fast:
                    timer.Interval = 1000;
                    break;
                case ChatUpdateDelay.Normal:
                    timer.Interval = 2000;
                    break;
                case ChatUpdateDelay.Slow:
                    timer.Interval = 3000;
                    break;
                case ChatUpdateDelay.VerySlow:
                    timer.Interval = 4000;
                    break;
                case ChatUpdateDelay.UltraSlow:
                    timer.Interval = 8000;
                    break;
            }
            YTClient.Log(Logger.LogLevel.Debug, $"Delay from ChatEvenListener[{guid.ToString()}] set to {timer.Interval}ms !");
        }

        public ChatEventsListener Start() {
            chatStatus = ChatStatus.Listening;
            errors = 0;
            timer.Start();
            return this;
        }

        public ChatEventsListener Stop() {
            chatStatus = ChatStatus.Disabled;
            timer.Stop();
            return this;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
            try {
                List<ChatMessage> messages = youTubeAPI.getChatMessages(chatID).GetAwaiter().GetResult();
                OnGetChatEvents_Event(this, new GetChatEventsArgs(messages));
            } catch(Exception ex) {
                YTClient.Log(Logger.LogLevel.Error, ex.Message, ex);
                errors++;
                if(errors >= 3) {
                    chatStatus = ChatStatus.Error;
                    YTClient.Log(Logger.LogLevel.Error, "Limit of errors reached, disconnecting from chat!");
                    Stop();
                }
            }
        }
    }
}
