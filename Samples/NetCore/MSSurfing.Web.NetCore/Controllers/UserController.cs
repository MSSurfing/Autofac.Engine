using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MSSurfing.Web.NetCore.Services;

namespace MSSurfing.Web.NetCore.Controllers
{
    public class UserController : Controller
    {
        #region Fields

        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        public IActionResult Index()
        {
            var count = _userService.Count();
            TempData["count"] = count;
            return View();
        }
    }
}