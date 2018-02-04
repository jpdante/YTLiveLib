using System;
using System.Collections.Generic;
using System.Text;
using YTLiveLib.Classes.API;

namespace YTLiveLib.Internal.Events {
    internal class GetChatEventsArgs : EventArgs {
        public List<ChatMessage> Messages;

        public GetChatEventsArgs(List<ChatMessage> messages) {
            Messages = messages;
        }
    }
}
