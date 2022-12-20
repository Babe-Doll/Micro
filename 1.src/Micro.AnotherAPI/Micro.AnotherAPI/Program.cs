


 
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "测试", Version = "v1" });
}); 



#region ids4
// 1.第一种写法
//builder.Services.AddAuthentication("Bearer")
//           .AddJwtBearer("Bearer", options =>
//           {
//               options.Authority = "https://localhost:7016";

//               options.TokenValidationParameters = new TokenValidationParameters
//               {
//                   ValidateAudience = false
//               };
//           });

// 2.第二种写法
builder.Services.AddAuthentication("Bearer")//scheme--表示通过Bearer方式来解析用户信息
     .AddIdentityServerAuthentication(options =>
     {
         options.Authority = "https://localhost:7016";//ids4的地址--专门获取公钥
         options.ApiName = "Test2";//quick没这个 就是用scope的 定义这个或者定义这个+scope 或者单个scope都可以
         options.RequireHttpsMetadata = false;// 不需要 https
     });//配置ids4


//scope校验
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        //policy.RequireClaim("scope", "api1"); //配置的需要是ids4里配置有的
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

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    //.RequireAuthorization("ApiScope"); //如果上面配置了scope 这边要加
});
app.Run();
