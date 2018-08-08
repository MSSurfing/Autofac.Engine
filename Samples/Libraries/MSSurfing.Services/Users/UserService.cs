using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSurfing.Services.Users
{
    public partial class UserService : IUserService
    {
        #region Constants
        //
        #endregion

        #region Fields
        private readonly IRepository<User> _userRepository;
        #endregion

        #region Ctor
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region Utilities
        #endregion

        #region Search & Get

        public IPagedList<User> Search(string username = null, string mobilephone = null, bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _userRepository.Table;

            query = query.Where(e => !e.Deleted);
            if (!string.IsNullOrEmpty(username))
                query = query.Where(e => e.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            var users = new PagedList<User>(query, pageIndex, pageSize);
            return users;
        }

        public IPagedList<User> GetOnlineUsers(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public User GetUserById(Guid Id)
        {
            if (Guid.Empty == Id)
                return null;

            return _userRepository.Table.FirstOrDefault(e => e.Id == Id);
        }

        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            return _userRepository.Table.FirstOrDefault(e => e.Username == username);
        }
        #endregion

        #region Insert / Update / Delete
        public void DeleteUser(User User)
        {
            if (User == null)
                throw new ArgumentNullException("User");

            User.Deleted = true;
            _userRepository.Update(User);
        }

        public void InsertUser(User User)
        {
            if (User == null)
                throw new ArgumentNullException("User");

            _userRepository.Insert(User);
        }

        public void UpdateUser(User User)
        {
            if (User == null)
                throw new ArgumentNullException("User");

            _userRepository.Update(User);
        }
        #endregion
    }
}
