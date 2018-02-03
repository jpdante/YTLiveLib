﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
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

        public async Task<string> getVideo(string search) {
            var searchListRequest = youTubeService.Search.List("snippet");
            searchListRequest.Q = search;
            searchListRequest.MaxResults = 1;
            var searchListResponse = await searchListRequest.ExecuteAsync();
            return searchListResponse.Items[0].Id.VideoId;
        }

        public async Task<string> getStreamChatID(string videoID) {
            var listRequest = youTubeService.Videos.List("snippet,contentDetails,statistics,liveStreamingDetails");
            listRequest.Id = videoID;
            var result = await listRequest.ExecuteAsync();
            return result.Items[0].LiveStreamingDetails.ActiveLiveChatId;           
        }

        public async Task<List<ChatMessage>> getChatMessages(string chatID) {
            var result = await youTubeService.LiveChatMessages.List(chatID, "snippet,authorDetails").ExecuteAsync();
            List<ChatMessage> messages = new List<ChatMessage>();
            foreach (var t in result.Items) {
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
    }
}