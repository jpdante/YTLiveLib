using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YTLiveLib.Classes.API;

namespace YTLiveLib.Classes.Client {
    public class YouTubeAPI {
        private YouTubeService youTubeService;

        public void Init(string appname, GoogleCredentials credentials) {
            ClientSecrets secrets = new ClientSecrets() {
                ClientId = credentials.ClientID,
                ClientSecret = credentials.ClientSecret
            };
            var token = new TokenResponse { RefreshToken = credentials.RefreshToken };
            var usercredentials = new UserCredential(new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer { ClientSecrets = secrets }), "user", token);
            youTubeService = new YouTubeService(new BaseClientService.Initializer { HttpClientInitializer = usercredentials, ApplicationName = appname });
        }

        #region LiveChatModerators
        public async Task<bool> removeModerator(string chatID, string moderatorID) {
            var response = await youTubeService.LiveChatModerators.Delete(moderatorID).ExecuteAsync();
            return response != null;
        }

        public async Task<bool> addModerator(string chatID, ChannelUser channelUser) {
            LiveChatModerator liveChatModerator = new LiveChatModerator();
            ChannelProfileDetails channelProfileDetails = new ChannelProfileDetails() {
                ChannelId = channelUser.ChannelID,
                ChannelUrl = channelUser.ChannelURL,
                DisplayName = channelUser.DisplayName,
                ProfileImageUrl = channelUser.ProfileImageURL
            };
            liveChatModerator.Snippet = new LiveChatModeratorSnippet() {
                LiveChatId = chatID,
                ModeratorDetails = channelProfileDetails
            };
            var response = await youTubeService.LiveChatModerators.Insert(liveChatModerator, "snippet").ExecuteAsync();
            return response != null;
        }

        public async Task<List<ChannelUser>> getModerators(string chatID) {
            var response = await youTubeService.LiveChatModerators.List(chatID, "snippet").ExecuteAsync();
            List<ChannelUser> moderators = new List<ChannelUser>();
            foreach (var t in response.Items) {
                ChannelUser channelUser = new ChannelUser() {
                    ChannelID = t.Snippet.ModeratorDetails.ChannelId,
                    ChannelURL = t.Snippet.ModeratorDetails.ChannelUrl,
                    DisplayName = t.Snippet.ModeratorDetails.DisplayName,
                    ProfileImageURL = t.Snippet.ModeratorDetails.ProfileImageUrl,
                    IsChatModerator = true,
                    ModeratorID = t.Id
                };
                moderators.Add(channelUser);
            }
            return moderators;
        }
        #endregion

        #region LiveChatMessages
        public async Task<bool> sendChatMessage(string message, string chatID) {
            LiveChatMessage liveMessage = new LiveChatMessage();
            liveMessage.Snippet = new LiveChatMessageSnippet() {
                LiveChatId = chatID,
                Type = "textMessageEvent",
                TextMessageDetails = new LiveChatTextMessageDetails() { MessageText = message }
            };
            var response = await youTubeService.LiveChatMessages.Insert(liveMessage, "snippet").ExecuteAsync();
            return response != null;
        }

        public async Task<List<ChatMessage>> getChatMessages(string chatID) {
            var response = await youTubeService.LiveChatMessages.List(chatID, "snippet,authorDetails").ExecuteAsync();
            List<ChatMessage> messages = new List<ChatMessage>();
            foreach (var t in response.Items) {
                ChatMessage message = new ChatMessage() {
                    Type = t.Snippet.Type,
                    DisplayMessage = t.Snippet.DisplayMessage,
                    LiveChatID = t.Snippet.LiveChatId,
                    PublishedAt = t.Snippet.PublishedAt ?? DateTime.Now,
                    PublishedAtRaw = t.Snippet.PublishedAtRaw,
                    AuthorChannelID = t.Snippet.AuthorChannelId,
                    ChannelUser = new ChannelUser() {
                        DisplayName = t.AuthorDetails.DisplayName,
                        ChannelID = t.AuthorDetails.ChannelId,
                        ChannelURL = t.AuthorDetails.ChannelUrl,
                        ProfileImageURL = t.AuthorDetails.ProfileImageUrl,
                        IsChatModerator = t.AuthorDetails.IsChatModerator ?? false,
                        IsChatOwner = t.AuthorDetails.IsChatOwner ?? false,
                        IsChatSponsor = t.AuthorDetails.IsChatSponsor ?? false,
                        IsVerified = t.AuthorDetails.IsVerified ?? false
                    }
                };
                if(t.Snippet.Type == "superChatEvent") {
                    message.SuperChatInfo = new ChannelSuperChat() {
                        AmountDisplayString = t.Snippet.SuperChatDetails.AmountDisplayString,
                        AmountMicros = t.Snippet.SuperChatDetails.AmountMicros ?? 0L,
                        Currency = t.Snippet.SuperChatDetails.Currency,
                        Tier = t.Snippet.SuperChatDetails.Tier ?? 0L,
                        UserComment = t.Snippet.SuperChatDetails.UserComment,
                    };
                } else if(t.Snippet.Type == "textMessageEvent") {
                    message.Message = t.Snippet.TextMessageDetails.MessageText;
                }
                messages.Add(message);
            }
            return messages;
        }
        #endregion

        #region SuperChatEvents

        #endregion

        #region Sponsors

        #endregion

        #region Livestreams
        public async Task<string> getStreamChatID(string videoID) {
            var listRequest = youTubeService.Videos.List("liveStreamingDetails");
            listRequest.Id = videoID;
            var result = await listRequest.ExecuteAsync();
            return result.Items[0].LiveStreamingDetails.ActiveLiveChatId;
        }
        #endregion

        #region LiveCuepoints

        #endregion

        #region LiveChatBans
        public async Task<ChannelBan> addChatBan(string chatID, ChannelUser channelUser, bool permanent = false, ulong time = 300) {
            LiveChatBan liveChatBan;
            if (permanent) {
                liveChatBan = new LiveChatBan() {
                    Snippet = new LiveChatBanSnippet() {
                        LiveChatId = chatID,
                        BannedUserDetails = new ChannelProfileDetails() {
                            ChannelId = channelUser.ChannelID,
                            DisplayName = channelUser.DisplayName,
                            ChannelUrl = channelUser.ChannelURL,
                            ProfileImageUrl = channelUser.ProfileImageURL
                        }
                    }
                };
            } else {
                liveChatBan = new LiveChatBan() {
                    Snippet = new LiveChatBanSnippet() {
                        BanDurationSeconds = time,
                        LiveChatId = chatID,
                        BannedUserDetails = new ChannelProfileDetails() {
                            ChannelId = channelUser.ChannelID,
                            DisplayName = channelUser.DisplayName,
                            ChannelUrl = channelUser.ChannelURL,
                            ProfileImageUrl = channelUser.ProfileImageURL
                        }
                    }
                };
            }
            var addban = youTubeService.LiveChatBans.Insert(liveChatBan, "snippet");
            var result = await addban.ExecuteAsync();
            ChannelBan channelBan = new ChannelBan() {
                ID = result.Id,
                Kind = result.Kind,
                BanDurationSeconds = result.Snippet.BanDurationSeconds ?? 0L,
                LiveChatID = result.Snippet.LiveChatId,
                ChannelUser = new ChannelUser() {
                    ChannelID = result.Snippet.BannedUserDetails.ChannelId,
                    ChannelURL = result.Snippet.BannedUserDetails.ChannelUrl,
                    DisplayName = result.Snippet.BannedUserDetails.DisplayName,
                    ProfileImageURL = result.Snippet.BannedUserDetails.ProfileImageUrl,
                }
            };
            return channelBan;
        }

        public async Task<bool> removeChatBan(string id) {
            var addban = youTubeService.LiveChatBans.Delete(id);
            var result = await addban.ExecuteAsync();
            return true;
        }
        #endregion

        #region LiveBroadcasts

        #endregion

        #region Search
        /*public async Task<string> searchFirstVideo(string search) {
            var searchListRequest = youTubeService.Search.List("snippet");
            searchListRequest.Q = search;
            searchListRequest.MaxResults = 1;
            var searchListResponse = await searchListRequest.ExecuteAsync();
            return searchListResponse.Items[0].Id.VideoId;
        }*/

        public async Task<List<ChannelVideo>> searchVideo(string search, int max_results = 30) {
            var searchListRequest = youTubeService.Search.List("id,snippet");
            searchListRequest.Q = search;
            searchListRequest.MaxResults = max_results;
            var searchListResponse = await searchListRequest.ExecuteAsync();
            List<ChannelVideo> videos = new List<ChannelVideo>();
            foreach(var i in searchListResponse.Items) {
                videos.Add(new ChannelVideo() {
                    VideoID = i.Id.VideoId,
                    VideoPlaylist = i.Id.PlaylistId,
                    IDKind = i.Id.Kind,
                    Kind = i.Kind,
                    ChannelID = i.Snippet.ChannelId,
                    Title = i.Snippet.Title,
                    ChannelTitle = i.Snippet.ChannelTitle,
                    Description = i.Snippet.Description,
                    PublishedAt = i.Snippet.PublishedAt ?? DateTime.Now,
                    PublishedAtRaw = i.Snippet.PublishedAtRaw,
                    LiveBroadcastContent = i.Snippet.LiveBroadcastContent
                });
            }
            return videos;
        }
        #endregion

        #region Videos

        #endregion
    }
}
