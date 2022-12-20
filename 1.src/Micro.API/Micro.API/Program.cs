using Micro.API.DBContext;
using Micro.API.Interface;
using Micro.API.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "����", Version = "v1" });
});
#region IOC
builder.Services.AddTransient<IUserService, UserService>();
#endregion 
#region Db 
var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
var connectionString = config.GetSection("DbConfig:Mysql:ConnectionString").Value;
var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));
builder.Services.AddDbContext<MicroDBContext>(
        options => options.UseMySql(connectionString, serverVersion)
                // The following three options help with debugging, but should
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
         );
#endregion

#region ids4
// 1.��һ��д��
//builder.Services.AddAuthentication("Bearer")
//           .AddJwtBearer("Bearer", options =>
//           {
//               options.Authority = "https://localhost:7016";

//               options.TokenValidationParameters = new TokenValidationParameters
//               {
//                   ValidateAudience = false
//               };
//           });

// 2.�ڶ���д��
builder.Services.AddAuthentication("Bearer")//scheme--��ʾͨ��Bearer��ʽ�������û���Ϣ
     .AddIdentityServerAuthentication(options =>
     {
         options.Authority = "https://localhost:7016";//ids4�ĵ�ַ--ר�Ż�ȡ��Կ
         options.ApiName = "Test1";  //�����ǽ���scope��
         options.RequireHttpsMetadata = false;// ����Ҫ https
     });//����ids4


//scopeУ��
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1"); //���õ���Ҫ��ids4�������е�
    });
});
#endregion


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseHttpsRedirection();
app.UseRouting();

//��Ȩ��Ȩ
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers()
    .RequireAuthorization("ApiScope"); //�������������scope ���Ҫ��
});
app.Run();
