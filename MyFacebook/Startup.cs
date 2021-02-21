using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFacebook
{
    public class Startup
    {
        public void Configuration(IAppBuilder App)
        {
            App.MapSignalR();
        }
    }
}