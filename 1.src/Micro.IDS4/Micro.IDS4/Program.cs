using IdentityServer4.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region IOC
//https://localhost:7016/.well-known/openid-configuration
builder.Services.AddIdentityServer()//ids4怎么用的
.AddDeveloperSigningCredential()//临时生成的证书--即时生成的
.AddInMemoryClients(ClientInitConfig.GetClients())//InMemory 内存模式
.AddInMemoryApiScopes(ClientInitConfig.GetApiScopes())//指定作用域
.AddInMemoryApiResources(ClientInitConfig.GetApiResources());//能访问啥资源
#endregion



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#region 中间件
app.UseIdentityServer();//使用这个中间件来处理请求
#endregion

//app.UseHttpsRedirection(); //这个不能要

app.Run();
public class ClientInitConfig
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    new IdentityResource[]
    {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
    };

    /// <summary>
    /// 定义ApiResource   
    /// 这里的资源（Resources）指的就是管理的API
    /// </summary>
    /// <returns>多个ApiResource</returns>
    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new[]
        {
                new ApiResource("UserApi", "用户获取API")
                {
                    Scopes={ "scope1" }//4.x必须写的
                }
            };
    }

    /// <summary>
    /// Api范围---4.x新增的
    /// </summary>
    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new ApiScope[]
          {
                new ApiScope("api1", "My API"),//api名字
                new ApiScope("api2"),
          };
    }

    /// <summary>
    /// 定义可以通过验证的Client
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Client> GetClients()
    {
        return new[]
        {
                new Client
                {
                    ClientId = "client1",//客户端唯一标识 

                    ClientSecrets = new [] {
                        new Secret("Secret".Sha256())
                    },//客户端密码，进行了加密

                    AllowedGrantTypes = GrantTypes.ClientCredentials,//授权方式，客户端认证，没有交互式用户，使用 clientid/secret 进行身份验证
                     
                    AllowedScopes = new [] { "api1" },//允许访问的资源 
                    
                    
                    Claims=new List<ClientClaim>(){
                        new ClientClaim(IdentityModel.JwtClaimTypes.Role,"Admin"),
                        new ClientClaim(IdentityModel.JwtClaimTypes.NickName,"lala"),
                        new ClientClaim("eMail","1@qq.com") 
                    }
                },
                new Client
                {
                    ClientId = "client2",//客户端唯一标识 

                    ClientSecrets = new [] {
                        new Secret("Secret2".Sha256())
                    },//客户端密码，进行了加密

                    AllowedGrantTypes = GrantTypes.ClientCredentials,//授权方式，客户端认证，没有交互式用户，使用 clientid/secret 进行身份验证
                     
                    AllowedScopes = new [] { "api1","api2" },//允许访问的资源 
                    
                     
                }
            };
    }
}