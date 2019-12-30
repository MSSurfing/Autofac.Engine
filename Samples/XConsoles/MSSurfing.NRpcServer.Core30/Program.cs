using Autofac;
using Autofac.Engine;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MSSurfing.NRpcServer.Core30
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new ServiceCollection();

            EngineContext.Initialize(service);

            //EngineContext.Initialize();

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
