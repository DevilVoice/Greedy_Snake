using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketProtocol;
using System.Reflection;
using Server.Servers;
namespace Server.Controller
{
    internal class ControllerManage
    {
        private Dictionary<Request,BaseController> ControllerDic = new Dictionary<Request,BaseController>();

        private Servers.Server server;

        public ControllerManage(Servers.Server server) {
            this.server = server;
            UserController userController = new UserController();
            PlayerController playerController = new PlayerController();
            ControllerDic.Add(userController.GetRequest, userController);
            ControllerDic.Add(playerController.GetRequest, playerController);
        }

        public void HandleRequest(MainPack pack,Client client) {
            if (ControllerDic.TryGetValue(pack.Request, out BaseController controller)) {
                string actionName = pack.Action.ToString();
                MethodInfo method = controller.GetType().GetMethod(actionName);//查找是否有对应名称方法
                if (method != null)
                {
                    object[] obj= new object[] { client, pack };
                    object ret= method.Invoke(controller, obj);
                    if(ret!=null)
                    {
                        client.Send(ret as MainPack);
                    }
                }
            }
                
        }
    }
}
