using Autofac.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSurfing.gRpcServer.Net45
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineContext.Initialize();

            //or
            //EngineContext.Initialize("MStag"); 
            //EngineContext.Initialize(null, ScopeTag.Http);

            ProgramCommander.LoopCmd();
            
            //Thrift Server Start
            Console.ReadLine();
        }
    }
}
