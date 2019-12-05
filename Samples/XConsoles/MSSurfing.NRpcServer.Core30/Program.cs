using Autofac;
using Autofac.Engine;
using System;

namespace MSSurfing.NRpcServer.Core30
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineContext.Initialize();

            // or EngineContext.Initialize(doBuild: false).Build();
            // or 
            //      var builder = EngineContext.Initialize(false);
            //      builder...
            //      builder.build();

            ProgramCommander.LoopCmd();

            //Thrift Server Start
            Console.ReadLine();
        }
    }
}
