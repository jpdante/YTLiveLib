using System;
using System.Collections.Generic;
using System.Text;
using YTLiveLib.Classes.API;

namespace YTLiveLib.Internal {
    public class OnGetMessagesArgs : EventArgs {

        public List<ChatMessage> Messages;

        public OnGetMessagesArgs(List<ChatMessage> messages) {
            Messages = messages;
        }
    }
}
