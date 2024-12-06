using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Server.Servers;
using SocketProtocol;

namespace Server.InterAction
{
    internal class UserData
    {
        private string connect = "Database = user;Server = localhost;User = root;Password = @xiatian2012;Port = 3306";
        private MySqlConnection sqlConnection;
        public MySqlConnection GetSqlConnection { get=>sqlConnection; }

        public UserData()
        {
            try
            {
                sqlConnection = new MySqlConnection(connect);
                sqlConnection.Open();
            }
            catch
            {
                Console.WriteLine("连接失败");
            }
        }
        public bool Sign(MainPack pack,Servers.Client client)
        {
            string userName = pack.Login.UserName;
            string passWord = pack.Login.PassWord;
            string findUser = "SELECT name FROM user.userdata where name ='" + userName + "'";
            try
            {
                string createUser = "INSERT INTO user.userdata (name,password) VALUES('" + userName + "','" + passWord + "')";
                MySqlCommand commd = new MySqlCommand(createUser, sqlConnection);
                commd.ExecuteNonQuery();
                Console.WriteLine("注册成功");
                Servers.Client.user.Add(userName);
                client.setUser = userName;
                return true;
            }
            catch {
                Console.WriteLine("用户名已存在");
                return false;
            }
            
        }

        public bool Login(MainPack pack,Servers.Client client)
        {
                string userName = pack.Login.UserName;
                string passWord = pack.Login.PassWord;
                string findUser = "SELECT password FROM user.userdata where name ='" + userName + "'";
                MySqlCommand commd = new MySqlCommand(findUser, sqlConnection);
                MySqlDataReader reader = commd.ExecuteReader();
                if (!reader.Read() || reader[0].ToString() != passWord)
                {
                    Console.WriteLine("用户名或密码错误");
                    reader.Close();
                    return false;
                }
                else
                {
                if (Servers.Client.user.Exists(t => t == userName))//检测是否重复登录
                {
                    Console.WriteLine("用户已登录");
                    reader.Close();
                    return false;
                }
                else
                {
                    Console.WriteLine("登陆成功");
                    Servers.Client.user.Add(userName);
                    client.setUser=userName;
                    reader.Close();
                    return true;
                }
                }
           
            
            
            
        }
    }
}
