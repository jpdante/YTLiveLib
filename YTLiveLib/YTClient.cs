using System;
using System.Collections.Generic;
using System.Text;
using YTLiveLib.Classes.Client;
using YTLiveLib.Events;
using YTLiveLib.Internal;
using YTLiveLib.Logger;

namespace YTLiveLib {
    public class YTClient {

        private static List<ILogger> loggers;
        private YouTubeAPI youTubeAPI;
        private List<EventsListener> messageListeners;

        public string appName;
        public GoogleCredentials googleCredentials;

        public YTClient(string appname, GoogleCredentials credentials) {
            appName = appname;
            googleCredentials = credentials;

            loggers = new List<ILogger>();
            youTubeAPI = new YouTubeAPI();
            messageListeners = new List<EventsListener>();

            this.OnConnectEvent += YTClient_OnConnectEvent;
            this.OnDisconnectEvent += YTClient_OnDisconnectEvent;
        }

        #region Events
        public delegate void OnConnect(object sender, OnConnectArgs e);
        public event OnConnect OnConnectEvent;

        public delegate void OnDisconnect(object sender, OnDisconnectArgs e);
        public event OnDisconnect OnDisconnectEvent;
        #endregion

        public async void Connect() {
            Log(LogLevel.Info, "Connecting to GoogleAPI...");
            youTubeAPI.Init(appName, googleCredentials);
            OnConnectEvent(this, new OnConnectArgs());
        }

        public async void JoinStreamChannel(string search) {
            Log(LogLevel.Info, $"Searching for \"{search}\"");
            string videoid = await youTubeAPI.getVideo(search);
            Log(LogLevel.Info, $"Video ID: \"{videoid}\"");
            Log(LogLevel.Info, $"Getting StreamChat...");
            string chatid = await youTubeAPI.getStreamChatID(videoid);
            Log(LogLevel.Info, $"Chat ID: \"{chatid}\"");
            Log(LogLevel.Info, $"Adding to MessageListener...");
            messageListeners.Add(new EventsListener(youTubeAPI, chatid).Start());
        }

        public void RegisterLogger(ILogger logger) {
            Log(LogLevel.Info, $"Registering logger: {logger.ToString()}");
            loggers.Add(logger);
        }

        public async void Disconnect() {
            Log(LogLevel.Info, "Disconnecting from GoogleAPI...");
            OnDisconnectEvent(this, new OnDisconnectArgs());
        }

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

        #region Events
        private void YTClient_OnConnectEvent(object sender, OnConnectArgs e) {
            Log(LogLevel.Info, "Connected to GoogleAPI!");
        }
        private void YTClient_OnDisconnectEvent(object sender, OnDisconnectArgs e) {
            Log(LogLevel.Info, "Disconnected from GoogleAPI!");
        }
        #endregion
    }
}
