using Autofac.Engine;
using MSSurfing.NRpcServer.Core30.Processors;
using System;

namespace MSSurfing.NRpcServer.Core30
{
    public static class ProgramCommander
    {
        #region Utilities
        public static void LoopCmd()
        {
            bool isContinue = true;
            do
            {
                Console.WriteLine("please entry cmd: add / search / loadallplugins / getversion / setversion");
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
                    case "getversion":
                        GetVersion();
                        break;
                    case "setversion":
                        SetVersion();
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
            var result = EngineContext.Resolve<UserProcessor>().Search();
            Console.WriteLine(result);
        }

        static void AddUser()
        {
            var result = EngineContext.Resolve<UserProcessor>().Add();
            Console.WriteLine(result);
        }

        static void LoadAllPlugins()
        {
            var result = EngineContext.Resolve<PluginProcessor>().LoadAllPlugins();
            Console.WriteLine(result);
        }

        static void GetVersion()
        {
            var result = EngineContext.Resolve<PluginProcessor>().GetVersion();
            Console.WriteLine(result);
        }

        static void SetVersion()
        {
            var version = Guid.NewGuid().ToString();
            Console.WriteLine("version:" + version);

            var result = EngineContext.Resolve<PluginProcessor>().SetVersion(version);
            Console.WriteLine(result);
        }
        #endregion
    }
}
