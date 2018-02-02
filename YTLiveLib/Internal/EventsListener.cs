using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using YTLiveLib.Classes.API;
using YTLiveLib.Classes.Client;

namespace YTLiveLib.Internal {
    public class EventsListener {

        private Timer timer;
        private YouTubeAPI youTubeAPI;
        private string chatID;
        private DateTime currentTime;

        public EventsListener(YouTubeAPI youtubeapi, string chatid) {
            youTubeAPI = youtubeapi;
            chatID = chatid;
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
        }

        public EventsListener Start() {
            timer.Start();
            return this;
        }

        public EventsListener Stop() {
            timer.Stop();
            return this;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
            try {
                currentTime = DateTime.Now;
                YTClient.Log(Logger.LogLevel.Info, $"Getting messages... [{currentTime.ToString()}]");
                List<ChatMessage> messages = youTubeAPI.getChatMessages(chatID).GetAwaiter().GetResult();
                foreach(ChatMessage message in messages) {
                    if (message.Type != "textMessageEvent") continue;
                    //if (message.PublishedAt < currentTime) continue;
                    //Console.Clear();
                    YTClient.Log(Logger.LogLevel.Info, $"[{message.PublishedAt.ToString()}] {message.ChannelUser.DisplayName}: {message.Message}");
                }
            }
            catch {
                timer.Stop();
            }
        }
    }
}
