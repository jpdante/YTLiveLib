using System;
using System.Collections.Generic;
using System.Text;
using YTLiveLib.Classes.API;

namespace YTLiveLib.Events {
    public class ReceiveSuperChatArgs : EventArgs {
        public ChatMessage Message;

        public ReceiveSuperChatArgs(ChatMessage message) {
            Message = message;
        }
    }
}
