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
        public void Send(string name, string message)
    {
      // Call the "OnMessage" method to update clients.
      Clients.All.SendCoreAsync("OnMessage", new object[]{name, message});
    }

    }
}
