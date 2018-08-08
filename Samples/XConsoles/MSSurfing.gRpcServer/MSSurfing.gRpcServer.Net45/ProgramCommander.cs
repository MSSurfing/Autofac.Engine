using Autofac.Engine;
using MSSurfing.gRpcServer.Net45.GServices;
using MSSurfing.gRpcServer.Net45.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSurfing.gRpcServer.Net45
{
    public static class ProgramCommander
    {
        #region Utilities
        public static void LoopCmd()
        {
            bool isContinue = true;
            do
            {
                Console.WriteLine("please entry cmd: add / search / loadallplugins");
                var cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "add":
                        AddUser();
                        break;
                    case "search":
                        SearchUser();
                        break;
                    case "loadallplugins":
                        LoadAllPlugins();
                        break;


                    case "exit":
                        isContinue = false;
                        break;
                    default:
                        break;
                }
            } while (isContinue);
        }
        #endregion

        #region Methods
        static void SearchUser()
        {
            var result = EngineContext.Resolve<GUserService>().Search();
            Console.WriteLine(result);
        }

        static void AddUser()
        {
            var result = EngineContext.Resolve<GUserService>().Add();
            Console.WriteLine(result);
        }

        static void LoadAllPlugins()
        {
            var result = EngineContext.Resolve<GPluginService>().LoadAllPlugins();
            Console.WriteLine(result);
        }
        #endregion
    }
}
