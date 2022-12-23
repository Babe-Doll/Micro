using IdentityServer4.AccessTokenValidation;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json", false, true);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot();

#region ids4
var authenticationProviderKey = "UserGatewayKey"; //配置的这个key跟配置文件一致
builder.Services.AddAuthentication("Bearer")
   .AddIdentityServerAuthentication(authenticationProviderKey, options =>
   {
       //指定要接入的授权服务器
       options.Authority = "https://localhost:7000";
       //options.ApiName = "UserApi";
       options.RequireHttpsMetadata = true;
       options.SupportedTokens = SupportedTokens.Both;
   });

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} 

app.UseOcelot(); 

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
