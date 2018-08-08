using Newtonsoft.Json;
using System.Collections;

namespace MSSurfing.ThriftServer.Processors
{
    public abstract class BaseProcessor
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
