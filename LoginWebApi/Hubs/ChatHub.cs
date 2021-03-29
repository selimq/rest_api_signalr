using Login.Data;
using Login.Repo;
using Login.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json.Converters;

namespace LoginWebApi.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ILogin login;
        private readonly IConnection conn;
        private readonly ICache cache;

        protected HttpClient ClientFireBase;
        public ChatHub(ILogin _login, IConnection _conn, ICache _cache)
        {
            login = _login;
            conn = _conn;
            cache = _cache;
            ClientFireBase = StartFireBase();
        }
        private HttpClient StartFireBase()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://fcm.googleapis.com");
            return client;
        }


        public async Task SendPrivate(String json)
        {
            ChatMessage chatMessage = JsonConvert.DeserializeObject<ChatMessage>(json,
             new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff", }
            );


            Connection User = await conn._GetConnection(chatMessage.ToUser);

            Person Sender = await login.GetLogin(int.Parse(chatMessage.Sender));



            string Json = JsonConvert.SerializeObject(chatMessage, Formatting.None);
            if (User.Connected == '1')
            {
                await Clients.User(chatMessage.ToUser).SendCoreAsync("OnMessage", new object[] { Json });
            }
            else
            {

                List<CacheMessage> messages = await cache.GetMessages(chatMessage.ToUser);
                String text = "";
                foreach (CacheMessage _message in messages)
                {
                    text += _message.Message + _message.Time + "\n";
                }
                text += chatMessage.Message;

                var body = new
                {
                    notification = new
                    {
                        body = text,
                        title = Sender.Ad,
                        tag = User.UserName

                    },
                    priority = "high",
                    sound = "1",
                    data = new
                    {
                        clickaction = "FLUTTERNOTIFICATIONCLICK",
                        id = "1",
                        status = "done",

                    },
                    to = "/topics/all"
                };

                ClientFireBase.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("key", "=AAAAtoc_DlU:APA91bGXD8NuagtJ25dsZBZrALo3KzX3wbX5R9ur9IOURUSgvG5eOdjQ1omW8ZA02JF82npXqPT69safWOVQPw09mpyzDGBznFNJa3RW0EwU5tlan1rhJ5jcWkINV5MrMT4HlrcaVqos");
                var response = await ClientFireBase.PostAsync("fcm/send", new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8, "application/json"));
                var retFireBase = await response.Content.ReadAsStringAsync();


                CacheMessage msg = new CacheMessage();
                msg.Message = chatMessage.Message;
                msg.ToUser = chatMessage.ToUser;
                msg.Sender = chatMessage.Sender;
                msg.Time = chatMessage.Time;
                msg.TypeId = chatMessage.TypeId;
                await cache.Save(msg);
            }
        }
        public override async Task OnConnectedAsync()
        {
            // await Clients.All.SendCoreAsync("S", new object[] { "  ", "   ", $"{Context.UserIdentifier} katıldı." });
            await base.OnConnectedAsync();
            Connection user = await conn._GetConnection(Context.UserIdentifier);
            user.Connected = '1';
            user.UserName = Context.UserIdentifier;
            await conn._Save(user);
            await SendPendingMessages(Context.UserIdentifier);

        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {

            //await Clients.All.SendCoreAsync("OnMessage", new object[] { "  ", "   ", $"{Context.UserIdentifier} ayrıldı." });
            await base.OnDisconnectedAsync(exception);
            Connection connection = await conn._GetConnection(Context.UserIdentifier);
            connection.Connected = '0';
            connection.LastSeen = DateTime.Now;

            await conn._Save(connection);

        }


        public async Task SendPendingMessages(string user)
        {
            List<CacheMessage> messages = await cache.GetMessages(user);
            /*  if (messages == null)
              {

                  await Clients.All.SendCoreAsync("OnMessage", new object[] { "  ", "   ", $"{Context.UserIdentifier}." });

              }
              else
              {*/
            foreach (CacheMessage message in messages)
            {
                string json = JsonConvert.SerializeObject(message, Formatting.Indented);
                await Clients.User(message.ToUser).SendCoreAsync("OnMessage", new object[] { json });
                cache.Delete(message);
            }
            //s   }


        }

        ///WEBRTC
        public async Task StartCall(String webRtcMessage)
        {
            WebRTCMessage webRTCObject = JsonConvert.DeserializeObject<WebRTCMessage>(webRtcMessage);
            await Clients.User(webRTCObject.To).SendCoreAsync("WebRTC", new object[] { webRtcMessage });
        }

        public async Task AnswerCall(String webRtcMessage)
        {
            WebRTCMessage webRTCObject = JsonConvert.DeserializeObject<WebRTCMessage>(webRtcMessage);
            await Clients.User(webRTCObject.To).SendCoreAsync("WebRTC", new object[] { webRtcMessage });
        }
    }
}
