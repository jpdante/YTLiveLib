using System;
using System.Collections.Generic;
using System.Text;
using YTLiveLib.Events;
using YTLiveLib.Internal;

namespace YTLiveLib.Classes.Client {
    public class JoinedChannel {

        public delegate void OnReceiveMessage(object sender, OnReceiveMessageArgs e);
        public event OnReceiveMessage OnReceiveMessageEvent;

        private YTClient client;

        public ChatEventsListener chatEventsListener;
        public ChatRepeatFilter chatRepeatFilter;

        public JoinedChannel(YouTubeAPI youTubeAPI, string chatID) {
            chatEventsListener = new ChatEventsListener(youTubeAPI, chatID);
            chatEventsListener.OnGetMessagesEvent += ChatEventsListener_OnGetMessagesEvent;
            chatRepeatFilter = new ChatRepeatFilter();
            chatRepeatFilter.OnReceiveMessageEvent += ChatRepeatFilter_OnReceiveMessageEvent;
        }

        private void ChatRepeatFilter_OnReceiveMessageEvent(object sender, OnReceiveMessageArgs e) {
            OnReceiveMessageEvent(sender, e);
        }

        private void ChatEventsListener_OnGetMessagesEvent(object sender, OnGetMessagesArgs e) {
            chatRepeatFilter.AddMessages(e.Messages);
        }

        public void StartListening() {
            chatEventsListener.Start();
        }

        public void StopListening() {
            chatEventsListener.Stop();
        }
    }
}
