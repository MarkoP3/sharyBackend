using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SharyApi.Hubs
{
    public class TestHub : Hub
    {
        public Task SendMessage(string user, string message)
        {
            Console.WriteLine("tu sam");
            return Clients.All.SendAsync("serverMessage", user, message);

        }
    }
}
