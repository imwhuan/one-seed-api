using Autofac;
using Autofac.Extensions.DependencyInjection;
using LibFrame.Confs;
using LibFrame.DBModel;
using LibFrame.DTOModel;
using LibFrame.Enums;
using LibFrame.IServices;
using LibFrame.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OneSeedApi.Model;
using OneSeedApi.Services;
using ServiceStack.Redis;
using SqlSugar;
using System.Reflection;
using System.Text;

namespace OneSeedApi.Conf
{
    public static class RegisterIOC
    {
        public static WebApplicationBuilder AddIocService(this WebApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Host.UseServiceProviderFactory<Autofac.ContainerBuilder>(new AutofacServiceProviderFactory());
            //配置AutoFac容器
            applicationBuilder.Host.ConfigureContainer<Autofac.ContainerBuilder>(AddServiceToAutoFac);
            //注入SqlSugar
            applicationBuilder.InitSqlSugarScope();
            //applicationBuilder.InitRedisService();
            applicationBuilder.InitFileService();
            return applicationBuilder;
        }

        private static void AddServiceToAutoFac(ContainerBuilder ContainerBuilderConfig)
        {
            //注册重写后的验证码服务
            //ContainerBuilderConfig.RegisterType<VerifyCodeServiceExt>().SingleInstance();
            ContainerBuilderConfig.RegisterType<VerifyCodeServiceExt>().As<VerifyCodeService>().SingleInstance();
            //注册SMS服务
            ContainerBuilderConfig.RegisterType<PhoneSMSService>().Keyed<ISMSService>(AccountNumberTypeEnum.Phone).SingleInstance();
            ContainerBuilderConfig.RegisterType<SimulateSMSService>().Keyed<ISMSService>(AccountNumberTypeEnum.Visitor).SingleInstance();
            ContainerBuilderConfig.RegisterType<EmailSMSService>().Keyed<ISMSService>(AccountNumberTypeEnum.Email).SingleInstance();
            //注入系统Counter服务
            ContainerBuilderConfig.RegisterType<SysCounterService>().SingleInstance();
            //注入用户服务
            ContainerBuilderConfig.RegisterType<UserService>().SingleInstance();
            //用户注册服务
            ContainerBuilderConfig.RegisterType<RegisterByPhoneCode>().Keyed<IRegisterService>(AccountNumberTypeEnum.Phone).SingleInstance();
            //用户登录服务
            ContainerBuilderConfig.RegisterType<LoginByAccountPwd>().Keyed<ILoginService>(AccountNumberTypeEnum.SID).SingleInstance();
            ContainerBuilderConfig.RegisterType<LoginByPhoneCode>().Keyed<ILoginService>(AccountNumberTypeEnum.Phone).SingleInstance();
            ContainerBuilderConfig.RegisterType<LoginByVisitor>().Keyed<ILoginService>(AccountNumberTypeEnum.Visitor).SingleInstance();
            ContainerBuilderConfig.RegisterType<LoginRecordManager>().SingleInstance();
            //注册jwt的Token生成服务
            ContainerBuilderConfig.RegisterType<GenerateJwtToken>().As<IGenerateJwtToken>().SingleInstance();
            //注入留言板服务
            ContainerBuilderConfig.RegisterType<MessageBoardService>().SingleInstance();
            //注入HttpClientHelper
            ContainerBuilderConfig.RegisterType<HttpClientHelper>().SingleInstance();
            //笔记服务
            ContainerBuilderConfig.RegisterType<NoteService>().SingleInstance();
            //共用服务
            ContainerBuilderConfig.RegisterType<CommonService>().SingleInstance();
            //Http请求提供对当前 HttpContext的访问权限（如果有）
            ContainerBuilderConfig.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
        }
        private static WebApplicationBuilder InitRedisService(this WebApplicationBuilder applicationBuilder)
        {
            //RedisConfigModel config=  applicationBuilder.Configuration.GetValue<RedisConfigModel>("RedisConfig");
            RedisConfigModel config = new RedisConfigModel();
            applicationBuilder.Configuration.GetSection("RedisConfig").Bind(config);
            GetRedisClient getRedisClient = new GetRedisClient(config);
            //注册RedisClient
            applicationBuilder.Services.AddSingleton<GetRedisClient>(getRedisClient);
            applicationBuilder.Services.AddSingleton<IRedisClient>(getRedisClient.GetClient());
            return applicationBuilder;

        }
        private static WebApplicationBuilder InitFileService(this WebApplicationBuilder builder)
        {
            //FileConfigModel fileConfig = new ();
            //builder.Configuration.GetSection("FileConfig").Bind(fileConfig);
            FileConfigModel fileConfig= builder.Configuration.GetSection("FileConfig").Get<FileConfigModel>();
            FileService fileService = new(fileConfig);
            builder.Services.AddSingleton(fileService);
            return builder;
        }
        /// <summary>
        /// 初始化并注入SqlSugar
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        private static WebApplicationBuilder InitSqlSugarScope(this WebApplicationBuilder applicationBuilder)
        {
            SqlSugarScope scope= new SqlSugarScope(new ConnectionConfig()
            {
                ConnectionString = applicationBuilder.Configuration.GetConnectionString("TencentCloudMySql"),//连接符字串
                DbType = DbType.MySql,//数据库类型
                IsAutoCloseConnection = true, //不设成true要手动close
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    EntityService = (prop, col) =>
                    {
                        // int?  decimal? 这种 isnullable = true
                        if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            col.IsNullable = true;
                        }
                        else if (prop.PropertyType == typeof(string) && prop.GetCustomAttribute<System.ComponentModel.DataAnnotations.RequiredAttribute>() == null)
                        {
                            col.IsNullable = true;
                        }
                    }
                }
            },
                db =>
                {
                    //(A)全局生效配置点，一般AOP等配置写这儿
                    //调试SQL事件，可以删掉
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        //Console.WriteLine(sql);//输出sql,查看执行sql 性能无影响

                        //5.0.8.2 获取无参数化 SQL  对性能有影响，特别大的SQL参数多的，调试使用
                        //string allsql= UtilMethods.GetSqlString(DbType.MySql, sql, pars);
                        //Console.WriteLine(allsql);
                    };

                    //多个配置就写下面
                    //db.Ado.IsDisableMasterSlaveSeparation=true;
                });
            applicationBuilder.Services.AddSingleton<SqlSugarScope>(scope);
            return applicationBuilder;
        }

        public static WebApplicationBuilder AddJwtService(this WebApplicationBuilder builder)
        {
            JwtTokenConfigModel configModel = new JwtTokenConfigModel();
            builder.Configuration.GetSection("JwtTokenConfig").Bind(configModel);
            builder.Services.AddSingleton<JwtTokenConfigModel>(configModel);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, "my_jwt", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configModel.Issuer,
                    ValidateAudience = true,
                    ValidAudience = configModel.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configModel.SecurityKey)),
                    AudienceValidator = (audiences, securityToken, validationParameters) =>
                    {
                        return true;
                    },
                    LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                    {
                        return true;
                    }
                };
                //重写Jwt的OnChallenge事件，可以自定义验证不通过时的返回信息
                options.Events = new JwtBearerEvents
                {
                    OnForbidden = context =>
                    {
                        //权限验证未通过时调用（授权阶段失败）
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.WriteAsJsonAsync<ApiResult>(new ApiResult()
                        {
                            StatusCode = StatusCodes.Status403Forbidden,
                            Message = "不好意思你没有该功能的权限哦~~🤗"
                        });
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        //token验证未通过时调用（鉴权/认证阶段失败）
                        context.HandleResponse();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.WriteAsJsonAsync<ApiResult>(new ApiResult()
                        {
                            StatusCode = StatusCodes.Status401Unauthorized,
                            Message = "不好意思你还没有登录哦~~🤗"
                        });
                        return Task.CompletedTask;
                    },
                    //OnMessageReceived = context =>
                    //{
                    //    string accessToken = context.Request.Query["token"];
                    //    if (!string.IsNullOrEmpty(accessToken) && context.Request.Path.StartsWithSegments("/hubs"))
                    //    {
                    //        context.Token = accessToken;
                    //    }
                    //    return Task.CompletedTask;
                    //}
                };
            });
            return builder;
        }

        public static WebApplicationBuilder initAutoMapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(config =>
            {
                config.CreateMap<TblUser,UserAccountInfo>();
            });
            return builder;
        }
    }
}
