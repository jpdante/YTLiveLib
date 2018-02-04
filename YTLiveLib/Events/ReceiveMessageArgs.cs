using System;
using System.Collections.Generic;
using System.Text;
using YTLiveLib.Classes.API;

namespace YTLiveLib.Events {
    public class ReceiveMessageArgs : EventArgs {
        public ChatMessage Message;

        public ReceiveMessageArgs(ChatMessage message) {
            Message = message;
        }
    }
}
