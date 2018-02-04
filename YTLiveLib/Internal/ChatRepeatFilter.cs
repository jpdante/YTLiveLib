using System;
using System.Collections.Generic;
using System.Text;
using YTLiveLib.Classes.API;
using YTLiveLib.Events;

namespace YTLiveLib.Internal {
    public class ChatRepeatFilter {

        public delegate void OnReceiveMessage(object sender, ReceiveMessageArgs e);
        public event OnReceiveMessage OnReceiveMessageEvent;

        public bool FirstInit;
        public List<string> AllMessages;

        public ChatRepeatFilter() {
            FirstInit = true;
        }

        public void FixMessages(List<ChatMessage> messages) {
            if(AllMessages == null) {
                FirstInit = true;
                AllMessages = new List<string>();
            }
            if(FirstInit) {
                FirstInit = false;
                AllMessages.Clear();
                foreach (ChatMessage message in messages) {
                    AllMessages.Add($"{message.PublishedAtRaw};{message.ChannelUser.ChannelID};{message.Message}");
                }
                return;
            }
            foreach(ChatMessage message in messages) {
                if(!AllMessages.Contains($"{message.PublishedAtRaw};{message.ChannelUser.ChannelID};{message.Message}")) {
                    OnReceiveMessageEvent(this, new ReceiveMessageArgs(message));
                }
            }
            AllMessages.Clear();
            foreach (ChatMessage message in messages) {
                AllMessages.Add($"{message.PublishedAtRaw};{message.ChannelUser.ChannelID};{message.Message}");
            }
        }
    }
}
