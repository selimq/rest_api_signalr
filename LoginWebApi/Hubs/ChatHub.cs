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
        public async Task SendMessage(string user, string msg)
        {
            await Clients.All.SendAsync("OnMessage",  new object[]{user, msg});
        }
    }
}
