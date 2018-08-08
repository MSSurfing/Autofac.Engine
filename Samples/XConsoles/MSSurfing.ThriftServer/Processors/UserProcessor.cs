using MSSurfing.Services;
using MSSurfing.Services.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSSurfing.ThriftServer.Processors
{
    public class UserProcessor : BaseProcessor //, TUserProcessor.Iface
    {
        #region Fields
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public UserProcessor(IUserService userService)
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
