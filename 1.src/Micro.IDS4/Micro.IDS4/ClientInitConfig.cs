using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.EntityFrameworkCore;

namespace Micro.IDS4
{
    public class ClientInitConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                                                                        new IdentityResource[]
                                                                        {
                                                                                new IdentityResources.OpenId(),
                                                                                new IdentityResources.Profile(),
                                                                        };

        /// <summary>
        /// 定义ApiResource 指的就是管理的api
        /// 可以管理api和api对应能访问的scope
        /// 这个api可以在客户端实例AddIdentityServerAuthentication的ApiName设置
        /// 注意：只要客户端实例有一个scope能在客户端被识别就可以
        /// </summary>
        /// <returns>多个ApiResource</returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {  
            return new[]  
            {
                new ApiResource("Test1", "用户获取API")
                {
                    Scopes={ "scope1" }//4.x必须写的
                },
                new ApiResource("Test2", "用户获取API")
                {
                    Scopes={ "scope2","scope3" } //定义api可以访问的scope
                }
            };
        }

        /// <summary>
        /// Api范围---4.x新增的  定义所有域
        /// </summary>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new ApiScope[]
              {
                new ApiScope("api1", "My API"),//api名字
                new ApiScope("api2"),
                new ApiScope("scope1"),
                new ApiScope("scope2"),
                new ApiScope("scope3"),
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
                     
                    AllowedScopes = new [] { "api1" },//允许访问的scope
                    
                    
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
                     
                    AllowedScopes = new [] { "api1","api2", "scope1" },//允许访问的scope
                    
                     
                },
                new Client
                {
                    ClientId = "client3",//客户端唯一标识 

                    ClientSecrets = new [] {
                        new Secret("Secret3".Sha256())
                    },//客户端密码，进行了加密

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,//授权方式，客户端认证，没有交互式用户，使用 clientid/secret 进行身份验证
                     
                    AllowedScopes = new [] { "api1","api2", "scope1","scope2","scope3" },//允许访问的scope
                    
                     
                }
            };
        }

        /// <summary> 
        /// 使用用户名密码模式
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser()
                { 
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser()
                {
                    SubjectId = "2",
                    Username = "wuwu",
                    Password = "password"
                }
            };
        }

        public static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in ClientInitConfig.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in ClientInitConfig.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                if (!context.ApiResources.Any())
                {
                    foreach (var resource in ClientInitConfig.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in ClientInitConfig.GetApiScopes())
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    } 
}
