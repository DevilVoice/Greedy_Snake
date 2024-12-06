using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Server.InterAction;
using SocketProtocol;
using Mysqlx.Connection;
using Org.BouncyCastle.Tls;
using System.Threading;
using Server.Controller;

namespace Server.Servers
{
    internal class Client
    {
        private Socket socket;
        private Message message;
        private UserData userdata;
        private Server server;
        public static List<string> user=new List<string>();
        private Thread send;
        private List<Client> clients;
        private string CurrentUser="";
        public string setUser { set { CurrentUser = value; } }
        public UserData GetUserData { get => userdata; }
        public Client(Socket socket, Server server )
        {
            userdata = new UserData();
            this.socket = socket;
            message = new Message();
            this.server = server;
            clients = server.getClient;
            //send = new Thread(new ThreadStart(autoSend));
            //send.Start();
            StartReceive();
            
        }
        void StartReceive() {
            socket.BeginReceive(message.Buffer, message.StartIndex, message.restSize, SocketFlags.None, ReceiveCallback, null);
        }
        void ReceiveCallback(IAsyncResult ira)
        {
            try
            {
                if (socket == null || socket.Connected == false) return;
                int length = socket.EndReceive(ira);
                if (length == 0)
                {
                    Close();
                    return;
                }
                message.ReadBuffer(length,HandleRequest);
                StartReceive();
            }//将信息序列化
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Exceptional Quit!");
                Close();//若未接收到信息，则客户端退出
            }
        }
        public void Send(MainPack pack)
        {
            socket.Send(Message.PackData(pack));
        }

        public void Broadcast(MainPack pack)
        {
            foreach (Client e in clients)
            {
                if (e == this)
                    continue;
                else
                    e.Send(pack);
            }
        }
        public void AllSend(MainPack pack)
        {
            foreach (Client e in clients)
                e.Send(pack);
        }
        void HandleRequest(MainPack pack)
        {
            server.HandleRequest(pack, this);
        }

        public bool Sign(MainPack pack) {
            return GetUserData.Sign(pack,this);
        }

        public bool Login(MainPack pack) { 
            return GetUserData.Login(pack,this);
        }

        public void Close() {
            Console.WriteLine("a client quit");
            server.Remove(this);
            if (user.Exists(t => t == CurrentUser))
                user.Remove(CurrentUser);//移除登陆状态
            socket.Close();
            userdata.GetSqlConnection.Close();
        }
        //void autoSend() {
        //    while (true)
        //    {
        //        for (int i = 0; i < PlayerController.players.Count; i++)
        //        {
        //            MainPack pack = new MainPack();
        //            pack.Action = SocketProtocol.Action.UpdatePlayer;
        //            pack.Player = PlayerController.players[i];
        //            Send(pack);
        //        }
        //        Thread.Sleep(50);
        //    }
        //}
    }
}
