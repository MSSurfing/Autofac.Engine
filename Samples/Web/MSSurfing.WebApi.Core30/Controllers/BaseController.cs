using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace MSSurfing.WebApi.Core30.Controllers
{
    public abstract class BaseController : Controller
    {
        protected JsonResult Json(IEnumerable Data, int Total)
        {
            return Json(new { data = Data, total = Total });
        }
    }
}
