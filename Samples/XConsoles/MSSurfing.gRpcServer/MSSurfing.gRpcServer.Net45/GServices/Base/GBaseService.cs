using Newtonsoft.Json;
using System.Collections;

namespace MSSurfing.gRpcServer.Net45.Services.Base
{
    public abstract class GBaseService
    {
        public string Json(IEnumerable Data, int Total)
        {
            return Json(new { data = Data, total = Total });
        }

        public string Json(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
