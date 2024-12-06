using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlX.XDevAPI;
using SocketProtocol;
using Server.Servers;
using System.Threading;
namespace Server.Controller
{

    internal class PlayerController :BaseController
    {
        static public List<PlayerInfo> players = new List<PlayerInfo>();
        private int playerNum = 0;
        private bool flag = false;
        public PlayerController() {
            request = SocketProtocol.Request.Game;
        }
        public MainPack Double(Servers.Client client,MainPack pack)
        {
            if (flag)
            {
                if (pack.Player.Joined == 0)
                {
                    Console.WriteLine("有对局正在进行");
                    pack.Return = Return.Fail;
                }
                if(pack.Player.Joined == 1)
                    pack.Return = Return.Succeed;
                return pack;
            }
            else
            {
                if (playerNum < 2&&pack.Player.Joined==0)
                {
                    pack.Player.Id = playerNum;
                    Position p = new Position();
                    if (pack.Player.Id == 0)//初始化玩家位置
                    {

                        p.X = 22.5f;
                        p.Y = -24.5f;
                        p.Z = 10;

                    }
                    else if (pack.Player.Id == 1) {
                        p.X = 22.5f;
                        p.Y = 24.5f;
                        p.Z = 10;
                    }
                    PlayerInfo info = new PlayerInfo();
                    info.Id = pack.Player.Id;
                    info.Pos = p;
                    players.Add(info);
                    playerNum++;
                    pack.Player.Joined = 1;
                }
                if (playerNum == 2)
                {
                    Console.WriteLine("匹配成功");
                    flag = true;
                    pack.Return = Return.Succeed;
                    return pack;
                }
                else {
                    pack.Return = Return.Fail;
                    return pack;
                }
            }
        }
        public MainPack Multi(Servers.Client client, MainPack pack)
        {
            if (flag)
            {
                if (pack.Player.Joined == 0)
                {
                    Console.WriteLine("有对局正在进行");
                    pack.Return = Return.Fail;
                }
                if (pack.Player.Joined == 1)
                    pack.Return = Return.Succeed;
                return pack;
            }
            else
            {
                if (playerNum < 4 && pack.Player.Joined == 0)
                {
                    pack.Player.Id = playerNum;
                    Position p = new Position();
                    if (pack.Player.Id == 0)
                    {

                        p.X = 47.5f;
                        p.Y = -49.5f;
                        p.Z = 10;

                    }
                    else if (pack.Player.Id == 1)
                    {
                        p.X = 47.5f;
                        p.Y = 49.5f;
                        p.Z = 10;
                    }
                    else if (pack.Player.Id == 2)
                    {
                        p.X = 47.5f;
                        p.Y = -24.5f;
                        p.Z = 10;
                    }
                    else if (pack.Player.Id == 3)
                    {
                        p.X = 47.5f;
                        p.Y = 24.5f;
                        p.Z = 10;
                    }
                    PlayerInfo info = new PlayerInfo();
                    info.Id = pack.Player.Id;
                    info.Pos = p;
                    players.Add(info);
                    playerNum++;
                    pack.Player.Joined = 1;
                }
                if (playerNum >= 3)
                {
                    Console.WriteLine("匹配成功");
                    pack.Return = Return.Succeed;
                    return pack;
                }
                else
                {
                    pack.Return = Return.Fail;
                    return pack;
                }
            }
        }
        public MainPack Start(Servers.Client client, MainPack pack) {
            Console.WriteLine("Starting");
            flag = true;
            foreach (PlayerInfo info in players)
            {
                pack.Playerlist.Add(info);
            }
            client.AllSend(pack);
            return null;
        }
        public void UpdatePlayer(Servers.Client client,MainPack pack)
        {
            client.Broadcast(pack);
        }
        public void Delete(Servers.Client client, MainPack pack) {
            client.Broadcast(pack);
        }
        public void Reset(Servers.Client client, MainPack pack) {
            if (flag)
            {
                flag = false;
                players.Clear();
                playerNum = 0;
                flag = false;
                pack.Return = Return.Succeed;
                Console.WriteLine("Reset");
                client.AllSend(pack);
                
            }
        }
        public void Score(Servers.Client client, MainPack pack) {
            client.Broadcast(pack);
        }
    }
}
