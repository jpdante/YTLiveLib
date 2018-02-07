using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YTLiveLib.Classes.Enum;
using YTLiveLib.Events;
using YTLiveLib.Internal;
using YTLiveLib.Internal.Events;

namespace YTLiveLib.Classes.Client {
    public class JoinedChannel {

        public delegate void OnReceiveMessage(object sender, ReceiveMessageArgs e);
        public event OnReceiveMessage OnReceiveMessageEvent;

        public delegate void OnReceiveSuperChat(object sender, ReceiveMessageArgs e);
        public event OnReceiveSuperChat OnReceiveSuperChatEvent;

        private YouTubeAPI youTubeAPI;
        private string chatID;
        private ChatUpdateDelay chatDelay;

        public ChatEventsListener chatEventsListener;
        public ChatRepeatFilter chatRepeatFilter;

        public JoinedChannel(YouTubeAPI youTubeapi, string chatid, ChatUpdateDelay delay) {
            chatDelay = delay;
            chatID = chatid;
            youTubeAPI = youTubeapi;
            chatEventsListener = new ChatEventsListener(youTubeAPI, chatID, delay);
            chatEventsListener.OnGetChatEvents_Event += ChatEventsListener_OnGetMessagesEvent;
            chatRepeatFilter = new ChatRepeatFilter();
            chatRepeatFilter.OnReceiveMessageEvent += ChatRepeatFilter_OnReceiveMessageEvent;
        }

        public async Task<bool> SendMessage(string message) {
            return await youTubeAPI.sendChatMessage(message, chatID);
        }

        public void SetChatDelay(ChatUpdateDelay delay) {
            chatEventsListener.setDelay(delay);
        }

        private void ChatRepeatFilter_OnReceiveMessageEvent(object sender, ReceiveMessageArgs e) {
            if(e.ChatMessage.Type.Equals("textMessageEvent", StringComparison.CurrentCultureIgnoreCase)) OnReceiveMessageEvent(sender, e);
            else if (e.ChatMessage.Type.Equals("superChatEvent", StringComparison.CurrentCultureIgnoreCase)) OnReceiveSuperChatEvent(sender, e);
        }

        private void ChatEventsListener_OnGetMessagesEvent(object sender, GetChatEventsArgs e) {
            chatRepeatFilter.FixMessages(e.Messages);
        }

        public void StartListening() {
            chatEventsListener.Start();
        }

        public void StopListening() {
            chatEventsListener.Stop();
        }
    }
}
