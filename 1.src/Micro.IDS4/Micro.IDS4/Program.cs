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
//https://localhost:7016/.well-known/openid-configuration �ӿڵ�ַ
//���õ�¼��ƾ֤

//�ڴ�ģʽ
//builder.Services.AddIdentityServer()//ids4��ô�õ�
//.AddDeveloperSigningCredential()//��ʱ���ɵ�֤��--��ʱ���ɵ�
//.AddInMemoryClients(ClientInitConfig.GetClients())//InMemory �ڴ�ģʽ
//.AddInMemoryApiScopes(ClientInitConfig.GetApiScopes())//ָ��������
//.AddInMemoryApiResources(ClientInitConfig.GetApiResources())//�ܷ���ɶ��Դ
//.AddTestUsers(ClientInitConfig.GetUsers());

//���ݿ�  
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
#region �м��
app.UseIdentityServer();//ʹ������м������������
#endregion

app.UseHttpsRedirection();

app.Run();
