using Micro.API.Interface;
using Micro.API.Model;

namespace Micro.API.Service
{
    public class UserService : IUserService
    {
        #region DataInit
        private List<User> _UserList = new List<User>()
        {
            new User()
            {
                Id=1,
                Account="Administrator",
                Email="1@qq.com",
                Name="elodia",
                Password="1234567890",
                LoginTime=DateTime.Now,
                Role="Admin"
            },
             new User()
            {
                Id=1,
                Account="Apple",
                Email="1@qq.com",
                Name="Apple",
                Password="1234567890",
                LoginTime=DateTime.Now,
                Role="Admin"
            },
              new User()
            {
                Id=1,
                Account="Cole",
                Email="1@qq.com",
                Name="Cole",
                Password="1234567890",
                LoginTime=DateTime.Now,
                Role="Admin"
            },
        };
        #endregion

        public User FindUser(int id)
        {
            return this._UserList.Find(u => u.Id == id);
        }

        public IEnumerable<User> UserAll()
        {
            return this._UserList;
        }
    }
}
