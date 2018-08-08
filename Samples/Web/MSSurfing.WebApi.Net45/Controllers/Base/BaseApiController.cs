using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MSSurfing.WebApi.Controllers.Base
{
    public class BaseApiController : ApiController
    {
        protected virtual HttpResponseMessage ErrorMessage(string Msg = null, object Data = null)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { Code = -1, Msg, Data });
        }

        protected virtual HttpResponseMessage SuccessMessage(string Msg = null, object Data = null)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new { Code = 1, Msg, Data });
        }
    }
}