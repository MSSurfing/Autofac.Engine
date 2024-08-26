using Autofac.Engine;
using MSSurfing.NRpcServer.Core30;
using System;

// error
var service = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

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
