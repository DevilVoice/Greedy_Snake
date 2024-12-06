using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketProtocol;

namespace Server.Controller
{
    abstract class BaseController
    {
        protected Request request=Request.None;

        public Request GetRequest { get => request; }
    }
}
