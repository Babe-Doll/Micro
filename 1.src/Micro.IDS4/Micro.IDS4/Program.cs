using IdentityServer4.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region IOC
//https://localhost:7016/.well-known/openid-configuration
builder.Services.AddIdentityServer()//ids4��ô�õ�
.AddDeveloperSigningCredential()//��ʱ���ɵ�֤��--��ʱ���ɵ�
.AddInMemoryClients(ClientInitConfig.GetClients())//InMemory �ڴ�ģʽ
.AddInMemoryApiScopes(ClientInitConfig.GetApiScopes())//ָ��������
.AddInMemoryApiResources(ClientInitConfig.GetApiResources());//�ܷ���ɶ��Դ
#endregion



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#region �м��
app.UseIdentityServer();//ʹ������м������������
#endregion

//app.UseHttpsRedirection(); //�������Ҫ

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
    /// ����ApiResource   
    /// �������Դ��Resources��ָ�ľ��ǹ����API
    /// </summary>
    /// <returns>���ApiResource</returns>
    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new[]
        {
                new ApiResource("UserApi", "�û���ȡAPI")
                {
                    Scopes={ "scope1" }//4.x����д��
                }
            };
    }

    /// <summary>
    /// Api��Χ---4.x������
    /// </summary>
    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new ApiScope[]
          {
                new ApiScope("api1", "My API"),//api����
                new ApiScope("api2"),
          };
    }

    /// <summary>
    /// �������ͨ����֤��Client
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Client> GetClients()
    {
        return new[]
        {
                new Client
                {
                    ClientId = "client1",//�ͻ���Ψһ��ʶ 

                    ClientSecrets = new [] {
                        new Secret("Secret".Sha256())
                    },//�ͻ������룬�����˼���

                    AllowedGrantTypes = GrantTypes.ClientCredentials,//��Ȩ��ʽ���ͻ�����֤��û�н���ʽ�û���ʹ�� clientid/secret ���������֤
                     
                    AllowedScopes = new [] { "api1" },//������ʵ���Դ 
                    
                    
                    Claims=new List<ClientClaim>(){
                        new ClientClaim(IdentityModel.JwtClaimTypes.Role,"Admin"),
                        new ClientClaim(IdentityModel.JwtClaimTypes.NickName,"lala"),
                        new ClientClaim("eMail","1@qq.com") 
                    }
                },
                new Client
                {
                    ClientId = "client2",//�ͻ���Ψһ��ʶ 

                    ClientSecrets = new [] {
                        new Secret("Secret2".Sha256())
                    },//�ͻ������룬�����˼���

                    AllowedGrantTypes = GrantTypes.ClientCredentials,//��Ȩ��ʽ���ͻ�����֤��û�н���ʽ�û���ʹ�� clientid/secret ���������֤
                     
                    AllowedScopes = new [] { "api1","api2" },//������ʵ���Դ 
                    
                     
                }
            };
    }
}