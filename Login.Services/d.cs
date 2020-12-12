/*using Login.Data;
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
      public ChatHub(ILogin _login){
        login  = _login;
     }



        public void _Send(string toUser,string sender, string message)
        {
        using (var db = new LoginDbContext())
            {
                var user = db.Girisler.Find(toUser);
                if (user == null)
                {
                    //Clients.Caller.showErrorMessage("Could not find that user.");
                }
                else
                {
                    db.Entry(user)
                        .Collection(u => u.Connections)
                        .Query()
                        .Where(c => c.Connected == true)
                        .Load();

                    if (user.Connections == null)
                    {
                        //Clients.Caller.showErrorMessage("The user is no longer connected.");
                    }
                    else
                    {
                        //
                    }
                }
            
            
            }
        }

            2132

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
*/