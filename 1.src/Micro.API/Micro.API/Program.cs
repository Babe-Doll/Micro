


using Micro.API.Interface;
using Micro.API.Service;

var builder = WebApplication.CreateBuilder(args); 
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "����", Version = "v1" });
});
#region IOC
builder.Services.AddTransient<IUserService, UserService>();
#endregion

#region ids4
builder.Services.AddAuthentication("Bearer")//scheme--��ʾͨ��Bearer��ʽ�������û���Ϣ
     .AddIdentityServerAuthentication(options =>
     {
         options.Authority = "http://localhost:7200";//ids4�ĵ�ַ--ר�Ż�ȡ��Կ
         options.ApiName = "UserApi";
         options.RequireHttpsMetadata = false;
     });//����ids4
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
 