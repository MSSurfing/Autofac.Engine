using MSSurfing.Services;
using MSSurfing.Services.Users;
using MSSurfing.WebApi.Controllers.Base;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace MSSurfing.WebApi.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : BaseApiController
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

        [HttpGet]
        public async Task<HttpResponseMessage> Search()
        {
            var list = _userService.Search();
            return SuccessMessage(Data: new { list, list.TotalCount });
        }

        [HttpGet, Route("adduser")]
        public async Task<HttpResponseMessage> Add()
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
            return SuccessMessage(Data: true);
        }
    }
}