using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketProtocol;
using Server.Servers;
using System.Diagnostics.Eventing.Reader;

namespace Server.Controller
{
    internal class UserController : BaseController
    {
        public UserController()
        {
            request = Request.User;
        }

        public MainPack Sign(Client client, MainPack pack)
        {
            if (client.Sign(pack))
            {
                pack.Return = Return.Succeed;
            }
            else
            {
                pack.Return = Return.Fail;
            }
            return pack;
        }

        public MainPack Login(Client client, MainPack pack)
        {
            if (client.Login(pack))
            {
                pack.Return = Return.Succeed;
            }
            else
            {
                pack.Return = Return.Fail;
            }
            return pack;
        }

        public Object Logout(Client client,MainPack pack) {
            Console.WriteLine("logout");
            Client.user.Remove(pack.Login.UserName);
            return null;
        }
    }
}
