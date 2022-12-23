using Micro.IDS4;
using Micro.IDS4.DbContext;
using Micro.IDS4.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region IOC
//https://localhost:7016/.well-known/openid-configuration 接口地址
//配置登录的凭证

//内存模式
//builder.Services.AddIdentityServer()//ids4怎么用的
//.AddDeveloperSigningCredential()//临时生成的证书--即时生成的
//.AddInMemoryClients(ClientInitConfig.GetClients())//InMemory 内存模式
//.AddInMemoryApiScopes(ClientInitConfig.GetApiScopes())//指定作用域
//.AddInMemoryApiResources(ClientInitConfig.GetApiResources())//能访问啥资源
//.AddTestUsers(ClientInitConfig.GetUsers());

//数据库  
var migrationsAssembly = Assembly.GetEntryAssembly().GetName().Name;
var connectionString = builder.Configuration.GetConnectionString("mysql");

builder.Services.AddDbContext<Ids4DbContext>(options => options.UseMySql(connectionString, new MySqlServerVersion(new Version())));
builder.Services.AddIdentity<SysUser, SysRole>().AddEntityFrameworkStores<Ids4DbContext>().AddDefaultTokenProviders();
builder.Services.AddIdentityServer()
    .AddTestUsers(ClientInitConfig.GetUsers())
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b => b.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29)),
            sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b => b.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29)),
            sql => sql.MigrationsAssembly(migrationsAssembly));
    });
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

app.UseHttpsRedirection();

app.Run();
