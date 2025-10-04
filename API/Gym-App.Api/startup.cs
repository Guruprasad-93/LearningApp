//using gym_app.Domain.Entities.Security;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.ResponseCompression;
//using Microsoft.Extensions.DependencyInjection.Extensions;
//using Microsoft.OpenApi.Models;
//using System.IO.Compression;
//using System.Net;
//using System.Reflection;
//using System.Text;

//namespace Gym_App.Api
//{
//    public class Startup
//    {
//        /// <summary>
//        /// Api Startup Constructor
//        /// </summary>
//        /// <param name="configuration"></param>
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        /// <summary>
//        /// Api Configuration
//        /// </summary>
//        public IConfiguration Configuration { get; }

//        /// <summary>
//        /// This method gets called by the runtime. Use this method to add services to the container.
//        /// </summary>
//        /// <param name="services"></param>
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddHttpClient("").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
//            {
//                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
//            });
//            services.Configure<GzipCompressionProviderOptions>(options =>
//            {
//                options.Level = CompressionLevel.Optimal;
//            });
//            services.AddResponseCompression(options =>
//            {
//                options.EnableForHttps = true;
//                options.Providers.Add<GzipCompressionProvider>();
//            });
//            //services.AddInfrastructure(Configuration);

//            var dbProvider = Configuration.GetValue<string>("DbProvider");
//            if (dbProvider == "POSTGRE_SQL")
//            {
//                SqlDataHelper.Initialize(Configuration);
//                services.AddInfrastructure(Configuration);

//            }
//            else if (dbProvider == "SqlServer")
//            {
//                SqlDataHelper.Initialize(Configuration);
//                services.AddInfrastructureSqlServer(Configuration);
//            }



//            ////services.AddSingleton<ProjectConfiguration>();
//            gym_app.IOC.DependencyContainer.RegisterService(services);
//            services.AddControllers(config =>
//            {
//                config.Filters.Add<ApiExceptionFilter>();
//            }).AddNewtonsoftJson(o =>
//            {
//                o.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
//                o.SerializerSettings.NullValueHandling = NullValueHandling.Include;
//                o.SerializerSettings.Formatting = Formatting.Indented;
//            }).AddDataAnnotationsLocalization();

//            var jwtOptions = new JwtOptions(Configuration);

//            services.TryAddEnumerable(ServiceDescriptor.Singleton<IFilterProvider, AntiforgeryFilterProvider>());
//            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);
//            services.AddAuthentication(x =>
//            {
//                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            })
//            .AddJwtBearer(x =>
//            {
//                x.RequireHttpsMetadata = false;
//                x.SaveToken = true;
//                x.TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateIssuer = true,
//                    ValidateAudience = true,
//                    ValidIssuer = jwtOptions.ValidIssuer,
//                    ValidAudience = jwtOptions.ValidAudience,
//                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
//                    ClockSkew = TimeSpan.FromMinutes(1)
//                };
//            });

//            services.AddAntiforgery(options =>
//            {
//                options.HeaderName = "X-XSRF-TOKEN";
//            });

//            //Create singleton for Appcache class
//            services.AddSingleton<IAppCache, AppCache>();

//            #region Swagger

//            services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Saras.eMarking.Api", Version = "v1" });
//                c.CustomSchemaIds(type => type.ToString());

//                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//                {
//                    Description = "Authorization token. Example: \"Bearer {apikey}\"",
//                    Scheme = "bearer",
//                    Type = SecuritySchemeType.Http
//                });
//                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
//                    {
//                        new OpenApiSecurityScheme {
//                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" }
//                        },
//                        Array.Empty<string>()
//                    },
//                    {
//                        new OpenApiSecurityScheme {
//                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
//                        },
//                        Array.Empty<string>()
//                    },
//                    {
//                        new OpenApiSecurityScheme {
//                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Token" }
//                        },
//                        Array.Empty<string>()
//                    }
//                });
//                //Locate the XML file being generated by ASP.NET...
//                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

//                //... and tell Swagger to use those XML comments.
//                c.IncludeXmlComments(xmlPath);
//            });

//            services.AddApiVersioning(config =>
//            {
//                config.ReportApiVersions = true;
//                config.AssumeDefaultVersionWhenUnspecified = true;
//                config.DefaultApiVersion = new ApiVersion(1, 0);
//            });

//            #endregion Swagger
//            services.AddWkhtmltopdf();
//            //JSON serializer
//            services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore).AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
//        }
//    }
//}
