


using Micro.API.Interface;
using Micro.API.Service;

var builder = WebApplication.CreateBuilder(args); 
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "测试", Version = "v1" });
});
#region IOC
builder.Services.AddTransient<IUserService, UserService>();
#endregion

#region ids4
builder.Services.AddAuthentication("Bearer")//scheme--表示通过Bearer方式来解析用户信息
     .AddIdentityServerAuthentication(options =>
     {
         options.Authority = "http://localhost:7200";//ids4的地址--专门获取公钥
         options.ApiName = "UserApi";
         options.RequireHttpsMetadata = false;
     });//配置ids4
#endregion


var app = builder.Build(); 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} 
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
 