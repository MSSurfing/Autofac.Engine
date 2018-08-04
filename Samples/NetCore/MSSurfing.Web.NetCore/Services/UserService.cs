using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSSurfing.Web.NetCore.Services
{
    public class UserService : IUserService
    {
        public int Count()
        {
            return 10;
        }
    }
}
