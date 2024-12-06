using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketProtocol;
using Google.Protobuf;

namespace Server.InterAction
{
    internal class Message
    {
        private byte[]buffer=new byte[2048];
        private int startindex=0;

        public byte[] Buffer { get => buffer; }
        public int StartIndex { get => startindex; }

        public int restSize { get => buffer.Length - startindex; }

        public void ReadBuffer(int length,Action<MainPack> HandleRequest) {
            startindex += length;
            

            while (true)
            {
                if (startindex <= 4) return;//未接收到包体
                int len = BitConverter.ToInt32(buffer, 0);//获得包体长度
                if (startindex >= len + 4)
                {
                    MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer,4,len);
                    HandleRequest(pack);//处理包体信息
                    Array.Copy(buffer, len + 4, buffer, 0, startindex - 4 - len);
                    startindex -= (4 + len);
                }
                else break ;
            }
        }//序列化解包
        public static byte[] PackData(MainPack pack)
        {
            byte[] data = pack.ToByteArray();//包体
            byte[] head=BitConverter.GetBytes(data.Length);//包头
            return head.Concat(data).ToArray();
        }//反序列化信息
    }
}
