using Micro.API.Interface;
using Micro.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Micro.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    #region Identity
    private readonly ILogger<UsersController> _logger;
    private readonly IUserService _iUserService = null;
    private IConfiguration _iConfiguration;

    public UsersController(ILogger<UsersController> logger, IUserService userService, IConfiguration configuration)
    {
        _logger = logger;
        this._iUserService = userService;
        this._iConfiguration = configuration;
    }
    #endregion

    [HttpGet]
    [Route("Get")]
    public User Get(int id)
    {
        Console.WriteLine($"This is UsersController {this._iConfiguration["port"]} Invoke Get");
        var u = this._iUserService.FindUser(id);
        return new User()
        {
            Id = u.Id,
            Account = u.Account + "MA",
            Name = u.Name,
            Role = $"{ this._iConfiguration["ip"] ?? "192.168.3.230"}{ this._iConfiguration["port"] ?? "5726"}",
            Email = u.Email,
            LoginTime = u.LoginTime,
            Password = u.Password + "K8S"
        };
    }

    [HttpGet]
    [Route("All")]
    [Authorize]//需要授权
    //[Authorize(Roles = "Admin")]
    //[Authorize(Policy = "AdminPolicy")]
    public IEnumerable<User> Get()
    {
        Console.WriteLine($"This is UsersController {this._iConfiguration["port"] ?? "5726"} Invoke");

        return this._iUserService.UserAll().Select(u => new User()
        {
            Id = u.Id,
            Account = u.Account + "MA",
            Name = u.Name,
            Role = $"{ this._iConfiguration["ip"] ?? "192.168.3.230"}{ this._iConfiguration["port"] ?? "5726"}",
            Email = u.Email,
            LoginTime = u.LoginTime,
            Password = u.Password + "K8S"
        });
    }

}
