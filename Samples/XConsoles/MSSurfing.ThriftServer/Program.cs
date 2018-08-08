using Autofac.Engine;
using MSSurfing.ThriftServer.Processors;
using System;

namespace MSSurfing.ThriftServer
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
