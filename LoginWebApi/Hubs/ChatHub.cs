using Login.Data;
using Login.Repo;
using Login.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginWebApi.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ILogin login;
        private readonly IConnection conn;

        private readonly ICache cache;
        public ChatHub(ILogin _login, IConnection _conn, ICache _cache)
        {
            login = _login;
            conn = _conn;
            cache = _cache;
        }



        public async Task SendPrivate(string toUser, string sender, string message)
        {
            Connection User = await conn._GetConnection(toUser);
            if (User.Connected == '1')
            {
                await Clients.User(toUser).SendCoreAsync("OnMessage", new object[] { toUser, sender, message });
            }
            else
            {
                CacheMessage msg = new CacheMessage();
                msg.Message = message;
                msg.ToUser = toUser;
                msg.Sender = sender;
                await cache.Save(msg);
            }
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendCoreAsync("OnMessage", new object[] { "  ", "   ", $"{Context.UserIdentifier} katıldı." });
            await base.OnConnectedAsync();
            Connection user = await conn._GetConnection(Context.UserIdentifier);
            user.Connected = '1';
            user.UserName = Context.UserIdentifier;
            await conn._Save(user);
            await SendPendingMessages(Context.UserIdentifier);

        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {

            await Clients.All.SendCoreAsync("OnMessage", new object[] { "  ", "   ", $"{Context.UserIdentifier} ayrıldı." });
            await base.OnDisconnectedAsync(exception);
            Connection connection = await conn._GetConnection(Context.UserIdentifier);
            connection.Connected = '0';
            await conn._Save(connection);

        }


        public async Task SendPendingMessages(string user)
        {
            List<CacheMessage> messages = await cache.GetMessages(user);
            if (messages == null)
            {

                await Clients.All.SendCoreAsync("OnMessage", new object[] { "  ", "   ", $"{Context.UserIdentifier}." });

            }
            else
            {
                foreach (CacheMessage message in messages)
                {
                    await Clients.User(message.ToUser).SendCoreAsync("OnMessage", new object[] { message.ToUser, message.Sender, message.Message });
                     cache.Delete(message);
                }
            }


        }
    }
}
