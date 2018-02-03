using System;
using System.Collections.Generic;
using System.Text;
using YTLiveLib.Classes.API;

namespace YTLiveLib.Events {
    public class OnReceiveMessageArgs {
        public ChatMessage Message;

        public OnReceiveMessageArgs(ChatMessage message) {
            Message = message;
        }
    }
}
