using System;
using System.Collections.Generic;
using System.Text;

namespace YTLiveLib.Classes.API {
    public class ChannelBan {
        public string ID;
        public string Kind;
        public ulong BanDurationSeconds;
        public string LiveChatID;
        public ChannelUser ChannelUser;
    }
}
