using Login.Data;
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
      private readonly ILogin login;
      public ChatHub(ILogin _login){
        login  = _login;
     }
        public void Send(string name, string message)
        {
            // Call the "OnMessage" method to update clients.
            Clients.All.SendCoreAsync("OnMessage", new object[] { name, message });
        }
        
        public void SendPrivate( string toUser,string sender, string message)
        {
         
            Clients.User(toUser).SendCoreAsync("OnMessage", new object[] {toUser, sender, message });
       
         
        }



        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendCoreAsync("OnMessage", new object[]{ "  ","   ",$"{Context.UserIdentifier} katıldı."});
            await base.OnConnectedAsync();
            Person msg = new Person();
            msg.Ad  = Context.UserIdentifier;
            msg.Soyad = "Katıldı -DENEME";
            await login.Save(msg);
        }
       public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendCoreAsync("OnMessage",new object[]{ "  ","   ",$"{Context.UserIdentifier} ayrıldı."});
            await base.OnDisconnectedAsync(exception);
               Person msg = new Person();
            msg.Ad  = Context.UserIdentifier;
            msg.Soyad = "Ayrıldı -DENEME";
            await login.Save(msg);
        }
    }
}
