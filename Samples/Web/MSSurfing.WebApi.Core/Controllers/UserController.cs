using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSSurfing.Services;
using MSSurfing.Services.Users;

namespace MSSurfing.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : BasePublicController
    {
        #region Fields
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public UserController(IUserService userService)
        {
            //_userService = new UserService();
            _userService = userService;
        }
        #endregion

        [HttpGet]
        public JsonResult Search()
        {
            var list = _userService.Search();
            return Json(list, list.TotalCount);
        }

        [HttpGet, Route("adduser")]
        public JsonResult Add()
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = "Ab",
                Email = "Ab@ab.ab",
                Mobilephone = "18000009999",
                Active = true,
                Deleted = false,
            };
            _userService.InsertUser(user);
            DebugLogger.Debug($"inserted user:id{user.Id}");
            return Json(true);
        }
    }
}