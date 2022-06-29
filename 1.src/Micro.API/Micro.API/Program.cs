


using Micro.API.Interface;
using Micro.API.Service;
using Microsoft.IdentityModel.Tokens;

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
         //options.ApiName = "Test";
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

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers()
    .RequireAuthorization("ApiScope"); //�������������scope ���Ҫ��
});
app.Run();
 