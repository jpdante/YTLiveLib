using System;
using System.Collections.Generic;
using System.Text;

namespace YTLiveLib.Classes.API {
    public class ChatMessage {
        public ChannelUser ChannelUser;
        public ChannelSuperChat SuperChatInfo;
        public string DisplayMessage;
        public string Message;
        public string Type;
        public string AuthorChannelID;
        public string LiveChatID;
        public string PublishedAtRaw;
        public DateTime PublishedAt;
    }
}
