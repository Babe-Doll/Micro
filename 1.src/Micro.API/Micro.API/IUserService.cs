using Micro.API.Model;

namespace Micro.API.Interface
{
    public interface IUserService
    {
        User FindUser(int id);

        IEnumerable<User> UserAll();

    }
}
