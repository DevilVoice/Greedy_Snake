using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Server.InterAction;
using Server.Controller;
using SocketProtocol;
using System.Threading;

namespace Server.Servers
{
    internal class Server
    {
        private Socket socket;
        private List<Client> ClientList;
        private ControllerManage controllerManage;
        public List<Client> getClient { get => ClientList; } 
        public Server(int port) {
   
            this.controllerManage = new ControllerManage(this);
            ClientList = new List<Client>();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            Thread accept = new Thread(new ThreadStart(StartAccept));
            accept.Start();
        }

        void StartAccept()
        {
            socket.Listen(0);
            while (true)
            {
                Socket client = socket.Accept();
                ClientList.Add(new Client(client, this));//添加连接的客户
            }
        }
        



        public void HandleRequest(MainPack pack, Client client)
        {
            controllerManage.HandleRequest(pack, client);
        }

        public void Remove(Client client) {
            ClientList.Remove(client);
        }
    }
}
