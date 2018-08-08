using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MSSurfing.WebApi.Controllers
{
    //[Stopwatch]
    public abstract class BasePublicController : BaseController
    {
        protected void LogException(Exception exception)
        {
            //todo
        }

        //protected virtual JsonResult EmptyMessage(string msg = null, object data = null)
        //{
        //    return null;
        //}

        /*
            HttpContext.Response.Clear();
            HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{filename}\"");
            HttpContext.Response.ContentType = "application/octet-stream";
            await HttpContext.Response.SendFileAsync(filename);

            */
    }
}
