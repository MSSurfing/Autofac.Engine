using MSSurfing.gRpcServer.Net45.Services.Base;
using MSSurfing.Services;
using MSSurfing.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSurfing.gRpcServer.Net45.Services
{
    public class GUserService : GBaseService
    {
        #region Fields
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public GUserService(IUserService userService)
        {
            _userService = userService;
        }
        #endregion
        public string Add()
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

        public string Search()
        {
            var list = _userService.Search();
            return Json(list, list.TotalCount);
        }
    }
}
