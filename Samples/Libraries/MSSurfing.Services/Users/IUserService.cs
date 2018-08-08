using System;
using System.Collections.Generic;
using System.Text;

namespace MSSurfing.Services.Users
{
    public interface IUserService
    {
        #region Search & Get
        IPagedList<User> Search(string username = null, string mobilephone = null
            , bool? isActive = null
            , int pageIndex = 0, int pageSize = 2147483647); //or Int32.MaxValue

        IPagedList<User> GetOnlineUsers(int pageIndex = 0, int pageSize = int.MaxValue);

        User GetUserById(Guid Id);

        User GetUserByUsername(string username);
        #endregion

        #region Insert / Update / Delete 
        void InsertUser(User User);

        void UpdateUser(User User);

        void DeleteUser(User User);
        #endregion
    }
}
