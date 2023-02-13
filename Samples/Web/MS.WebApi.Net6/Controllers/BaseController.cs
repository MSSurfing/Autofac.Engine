using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace MS.WebApi.Net6.Controllers
{
    public abstract class BaseController : Controller
    {
        protected JsonResult Json(IEnumerable Data, int Total)
        {
            return Json(new { data = Data, total = Total });
        }
    }
}
