using Microsoft.AspNetCore.Mvc;
using MSSurfing.Services.Users;
using MSSurfing.Services;
using System.Collections;
using s = MSSurfing.Services.Logging;
using IoCServiceProvider;
using MSSurfing.Services.Logging;

namespace MS.WebApi.Net6.Controllers
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
            var typeFinder = Singleton<ITypeFinder>.Instance;

            var log = typeFinder.FindClassesOfType(typeof(Log));

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

            s.ILogger logger = (s.ILogger)IoC.ServiceProvider.GetService<s.ILogger>();
            DebugLogger.Debug(logger, $"inserted user:id{user.Id}");
            return Json(true);
        }
    }
}
