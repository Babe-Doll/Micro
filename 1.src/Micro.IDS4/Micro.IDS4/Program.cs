using IdentityServer4.Models;
using Micro.IDS4;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region IOC
//https://localhost:7016/.well-known/openid-configuration 接口地址
//配置登录的凭证
builder.Services.AddIdentityServer()//ids4怎么用的
.AddDeveloperSigningCredential()//临时生成的证书--即时生成的
.AddInMemoryClients(ClientInitConfig.GetClients())//InMemory 内存模式
.AddInMemoryApiScopes(ClientInitConfig.GetApiScopes())//指定作用域
.AddInMemoryApiResources(ClientInitConfig.GetApiResources())//能访问啥资源
.AddTestUsers(ClientInitConfig.GetUsers());
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
 