using IdentityServer4.Models;
using Micro.IDS4;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region IOC
//https://localhost:7016/.well-known/openid-configuration �ӿڵ�ַ
//���õ�¼��ƾ֤
builder.Services.AddIdentityServer()//ids4��ô�õ�
.AddDeveloperSigningCredential()//��ʱ���ɵ�֤��--��ʱ���ɵ�
.AddInMemoryClients(ClientInitConfig.GetClients())//InMemory �ڴ�ģʽ
.AddInMemoryApiScopes(ClientInitConfig.GetApiScopes())//ָ��������
.AddInMemoryApiResources(ClientInitConfig.GetApiResources())//�ܷ���ɶ��Դ
.AddTestUsers(ClientInitConfig.GetUsers());
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
 