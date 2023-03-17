using Autofac.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using NLog.Web;
using OneSeedApi.Conf;
using OneSeedApi.Filters;
using OneSeedApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CatchExceptionFilterAttribute>();
    options.Filters.Add<ToApiResultFilterAttribute>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddSignalR();
#region 配置Swagger

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "请输入Token，格式为Bearer XXXXXXXX",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                }
            }, Array.Empty<string>()
        }
    });
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OneSeed平台接口",
        Version = "0.0.1",
        Description = "OneSeed平台后台接口文档",
        Contact = new OpenApiContact
        {
            Name = "Imwhuan",
            Email = "imwhuan@qq.com",
            Url = new Uri("https://github.com/imwhuan/LiveWeb")
        }
    });
    string xmlPath = Path.Combine(AppContext.BaseDirectory, "OneSeedApi.xml");
    option.IncludeXmlComments(xmlPath);
});

#endregion

//配置IOC服务
builder.AddIocService();
builder.AddJwtService();
#region 配置跨域

builder.Services.AddCors(options =>
{
    //options.AddPolicy("All", PolicyBuilder =>
    //{
    //    PolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    //});
    //使用signalr连接时需要配置AllowCredentials，该项与AllowAnyOrigin冲突，所以要改成下述写法
    options.AddPolicy("All", policy =>
    {
        policy.AllowAnyMethod().SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowCredentials();
    });
});

#endregion


#region 配置AutoMapper
builder.initAutoMapper();
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("All");

app.UseAuthorization();

app.MapGet("/api", () =>
{
    return "欢迎使用one seed服务";
});
app.MapHub<ChatRoomHub>("hubs/chatroom");

app.MapControllers();

app.Run();
