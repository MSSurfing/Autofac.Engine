using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using MSSurfing.Services;
using MSSurfing.Services.Users;

namespace MSSurfing.WebApi.Core30.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
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

        #region Utilities
        protected JsonResult Json(IEnumerable Data, int Total)
        {
            return Json(new { data = Data, total = Total });
        }
        #endregion

        [HttpGet, Route("search")]
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