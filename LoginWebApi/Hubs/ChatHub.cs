using Login.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginWebApi.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
      private readonly IMessages _Message;
      public ChatHub(IMessages messages){
        _Message  = messages;
      }
        public void Send(string name, string message)
        {
            // Call the "OnMessage" method to update clients.
            Clients.All.SendCoreAsync("OnMessage", new object[] { name, message });
        }
        
        public void SendPrivate(string toUser, string sender, string message)
        {
            Clients.User(toUser).SendCoreAsync("OnMessage", new object[] { toUser,sender, message });
            ChatMessage msg = new ChatMessage();
            msg.ToUser = toUser;
            msg.Sender  = sender;
            msg.Message = message;
            msg.Time = DateTime.Now;
            _Message.Save(msg);
        }



        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendCoreAsync("OnMessage", new object[]{" "," ",$"{Context.UserIdentifier} joined."});
            await base.OnConnectedAsync();
        }
       /* public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }*/
    }
}
