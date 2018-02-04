using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YTLiveLib.Classes.Client;
using YTLiveLib.Classes.Enum;
using YTLiveLib.Events;
using YTLiveLib.Internal;
using YTLiveLib.Logger;

namespace YTLiveLib {
    public class YTClient {

        private static List<ILogger> loggers;
        private YouTubeAPI youTubeAPI;
        private List<ChatEventsListener> messageListeners;

        public string appName;
        public GoogleCredentials googleCredentials;

        public List<JoinedChannel> JoinedChannels;

        public static bool Debug;

        public YTClient(string appname, GoogleCredentials credentials, bool debug = false) {
            appName = appname;
            googleCredentials = credentials;

            loggers = new List<ILogger>();
            youTubeAPI = new YouTubeAPI();
            messageListeners = new List<ChatEventsListener>();

            JoinedChannels = new List<JoinedChannel>();

            Debug = debug;

            this.OnConnectEvent += YTClient_OnConnectEvent;
            this.OnDisconnectEvent += YTClient_OnDisconnectEvent;
            this.OnReceiveMessageEvent += YTClient_OnReceiveMessageEvent;
        }

        #region Events
        public delegate void OnConnect(object sender, ConnectArgs e);
        public event OnConnect OnConnectEvent;

        public delegate void OnDisconnect(object sender, DisconnectArgs e);
        public event OnDisconnect OnDisconnectEvent;

        public delegate void OnReceiveMessage(object sender, ReceiveMessageArgs e);
        public event OnReceiveMessage OnReceiveMessageEvent;

        private void YTClient_OnReceiveMessageEvent(object sender, ReceiveMessageArgs e) {

        }
        private void YTClient_OnConnectEvent(object sender, ConnectArgs e) {
            Log(LogLevel.Info, "Connected to GoogleAPI!");
        }
        private void YTClient_OnDisconnectEvent(object sender, DisconnectArgs e) {
            Log(LogLevel.Info, "Disconnected from GoogleAPI!");
        }
        private void Channel_OnReceiveMessageEvent(object sender, ReceiveMessageArgs e) {
            OnReceiveMessageEvent(sender, e);
        }
        #endregion

        #region APIMethods    
        public JoinedChannel JoinChannel(string chatID) {
            JoinedChannel channel = new JoinedChannel(youTubeAPI, chatID, ChatUpdateDelay.Normal);
            channel.StartListening();
            channel.OnReceiveMessageEvent += Channel_OnReceiveMessageEvent;
            JoinedChannels.Add(channel);
            return channel;
        }

        public async Task<string> GetChatID(string videoID) {
            return await youTubeAPI.getStreamChatID(videoID);
        }
        #endregion

        #region Methods
        public async void Connect() {
            Log(LogLevel.Info, "Connecting to GoogleAPI...");
            youTubeAPI.Init(appName, googleCredentials);
            OnConnectEvent(this, new ConnectArgs());
        }

        public void RegisterLogger(ILogger logger) {
            if (Debug) logger.Debug($"Registering logger: {logger.ToString()}");
            if (Debug) Log(LogLevel.Info, $"Registering logger: {logger.ToString()}");
            loggers.Add(logger);
        }

        public async void Disconnect() {
            if (Debug) Log(LogLevel.Info, "Disconnecting from GoogleAPI...");
            OnDisconnectEvent(this, new DisconnectArgs());
        }
        #endregion

        #region Loggers
        internal static void Log(LogLevel level, string message, Exception ex) {
            if(level == LogLevel.Debug) {
                foreach (ILogger logger in loggers) {
                    logger.Debug(message, ex);
                }
            } else if (level == LogLevel.Error) {
                foreach (ILogger logger in loggers) {
                    logger.Error(message, ex);
                }
            } else if (level == LogLevel.Fatal) {
                foreach (ILogger logger in loggers) {
                    logger.Fatal(message, ex);
                }
            } else if (level == LogLevel.Info) {
                foreach (ILogger logger in loggers) {
                    logger.Info(message, ex);
                }
            } else if (level == LogLevel.Warn) {
                foreach (ILogger logger in loggers) {
                    logger.Warn(message, ex);
                }
            }
        }

        internal static void Log(LogLevel level, string message) {
            if(level == LogLevel.Debug) {
                foreach (ILogger logger in loggers) {
                    logger.Debug(message);
                }
            } else if (level == LogLevel.Error) {
                foreach (ILogger logger in loggers) {
                    logger.Error(message);
                }
            } else if (level == LogLevel.Fatal) {
                foreach (ILogger logger in loggers) {
                    logger.Fatal(message);
                }
            } else if (level == LogLevel.Info) {
                foreach (ILogger logger in loggers) {
                    logger.Info(message);
                }
            } else if (level == LogLevel.Warn) {
                foreach (ILogger logger in loggers) {
                    logger.Warn(message);
                }
            }
        }
        #endregion
    }
}
