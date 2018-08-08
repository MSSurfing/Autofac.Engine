//using Thrift.Protocol;

//namespace MSSurfing.ThriftServer.TServer
//{
//    public static class ProcessorRegistrar
//    {
//        public static TMultiplexedProcessor Register(TMultiplexedProcessor processors = null)
//        {
//            if (processors == null) processors = new TMultiplexedProcessor();

//            processors.RegisterProcessor(TServiceNames.T_USER_SERVICE, new TUserProcessor.Processor(new UserProcessor()));

//            return processors;
//        }
//    }
//}
