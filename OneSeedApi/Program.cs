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
#region ����Swagger

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "������Token����ʽΪBearer XXXXXXXX",
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
        Title = "OneSeedƽ̨�ӿ�",
        Version = "0.0.1",
        Description = "OneSeedƽ̨��̨�ӿ��ĵ�",
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

//����IOC����
builder.AddIocService();
builder.AddJwtService();
#region ���ÿ���

builder.Services.AddCors(options =>
{
    //options.AddPolicy("All", PolicyBuilder =>
    //{
    //    PolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    //});
    //ʹ��signalr����ʱ��Ҫ����AllowCredentials��������AllowAnyOrigin��ͻ������Ҫ�ĳ�����д��
    options.AddPolicy("All", policy =>
    {
        policy.AllowAnyMethod().SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowCredentials();
    });
});

#endregion


#region ����AutoMapper
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
    return "��ӭʹ��one seed����";
});
app.MapHub<ChatRoomHub>("hubs/chatroom");

app.MapControllers();

app.Run();
