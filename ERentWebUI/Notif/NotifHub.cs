using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERentWebUI.Notif
{
    public class NotifHub : Hub
    {
        public static void Send()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotifHub>();
            context.Clients.All.displayStatus();
        }
    }
}