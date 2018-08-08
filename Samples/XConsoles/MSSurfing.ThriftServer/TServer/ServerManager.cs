//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace MSSurfing.ThriftServer.TServer
//{
//    public partial class ServerManager
//    {
//        #region Fields
//        //private ServerConfig _config;

//        private TServerSocket serverTransport;
//        private Thrift.Server.TServer server;
//        #endregion

//        #region Constructor
//        public ServerManager(int port) : this(new ServerConfig(port: port)) { }

//        /// <param name="lcCommerceConnection"></param>
//        /// <param name="port"></param>
//        public ServerManager(ServerConfig config)
//        {
//            _config = config;

//            try
//            {
//                //initialize engine context
//                EngineContext.Initialize(false);

//                serverTransport = new TServerSocket(_config.Port, _config.ClientTimeout, false);
//                server = new TThreadPoolServer(ProcessorRegistrar.Register(), serverTransport, (str) =>
//                {
//                    Logger.ErrorLog("TServer 线程执行中错误:{0}", str);
//                });
//            }
//            catch (Exception ex)
//            {
//                Logger.ErrorLog("TServer 初始化失败:{0}", ex.Message);
//                throw ex;
//            }
//        }

//        #endregion

//        #region Methods
//        public void Start()
//        {
//            bool databaseInstalled = EntityFramework.Engine.Infrastructure.DataSettingsHelper.DatabaseIsInstalled();
//            if (!databaseInstalled)
//                throw new ConfigurationErrorsException("数据库连接错误！");

//            server.Serve();
//        }

//        /// <summary>
//        /// 暂停TServer服务
//        /// </summary>
//        public void Stop()
//        {
//            server.Stop();
//        }

//        /// <summary>
//        /// 关闭TServer服务
//        /// </summary>
//        public void Close()
//        {
//            if (serverTransport != null)
//            {
//                serverTransport.Close();
//                serverTransport = null;
//            }
//            if (server != null)
//            {
//                server.Stop();
//                server = null;
//            }
//        }
//        #endregion
//    }
//}
