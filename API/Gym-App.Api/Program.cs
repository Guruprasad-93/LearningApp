using gym_app.Domain.Entities;
using gym_app.Domain.Entities.AppOption;
using gym_app.IOC;
using Gym_App.Api.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using TokenLibrary.EncryptDecrypt.AesCrypto;

try
{
    

    string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() ?? "development";

    // STEP 2: Build config early
    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();

    var encryptedSecret = config.GetValue<string>("AppOptions:JwtOptions:Secret");


    string key = "T@yek#MAET7Sec";
    string plainText = "xA8!zGp2V^r7@LmNwq5#TfK9SbD$JeRu13HYtZvo";

    

    string encrypted = AesCryptoGraphyCore.Encrypt(plainText, key, isEncrypted: false);

    string decrypted = AesCryptoGraphyCore.Decrypt(encryptedSecret, key, isEncrypted: false);


    // STEP 3: Create builder and inject config
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddConfiguration(config);

    // STEP 4: Register AppOptions into DI
    var appOptions = AppOptions.ReadFromConfiguration(config);
    builder.Services.AddSingleton(appOptions);

    // STEP 5: Register DbContext BEFORE using it anywhere
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

    // STEP 6: Register all other services
    DependencyContainer.RegisterService(builder.Services, config);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "Bearer";
        options.DefaultChallengeScheme = "Bearer";
    }).AddJwtBearer("Bearer", options =>{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(config["JwtSettings:Secret"])
        )
    };
    });

    builder.Services.AddAuthorization();


    // STEP 7: Default DI registrations
    //builder.Services.AddControllers();
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    });
    builder.Services.AddControllers(config =>
    {
        var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

        config.Filters.Add(new AuthorizeFilter(policy));
    });

    builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // <-- disables camelCase
    });


    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularClient", policy =>
        {
            policy.WithOrigins("http://localhost:4200") // your Angular dev server
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });

    builder.Services.Configure<CookiePolicyOptions>(config.GetSection("CookiePolicy"));

    var app = builder.Build();

    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    // Localization
    var supportedCultures = new[] { "en-US", "ar" };
    var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
    localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
    app.UseRequestLocalization(localizationOptions);

    // Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "Gym_App.Api v1"));

    // Middleware pipeline
  //  app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseCors("AllowAngularClient");
    app.UseRouting();

    app.UseCookiePolicy();
    app.UseAuthentication();
    app.UseAuthMiddleware(); // custom
    app.UseAuthorization();

    // Error handling
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseStatusCodePages();
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

    // Optional: Log app cache info
    using (var scope = app.Services.CreateScope())
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogDebug("App started.");
    }

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Fatal error: " + ex.Message);
}















































//using gym_app.Domain.Entities;
//using gym_app.Domain.Entities.AppOption;
//using gym_app.IOC;
//using Gym_App.Api.Common;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using System.Reflection;

//try { 
//var builder = WebApplication.CreateBuilder(args);


//string EnvironmentName = "development";
//if (!string.IsNullOrEmpty(builder.Environment.EnvironmentName))
//{
//    EnvironmentName = builder.Environment.EnvironmentName.ToLower();
//}

//var config = new ConfigurationBuilder()
//.SetBasePath(Directory.GetCurrentDirectory())
//.AddJsonFile($"appsettings.{EnvironmentName}.json", optional: true, reloadOnChange: true)
//.AddEnvironmentVariables()
//.AddCommandLine(args).Build();
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//    var options = AppOptions.ReadFromConfiguration(config);

//    builder.Services.AddDbContext<ApplicationDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//DependencyContainer.RegisterService(builder.Services, builder.Configuration);


////Add localizaton path to service
//builder.Services.AddLocalization(localizeOption => localizeOption.ResourcesPath = "Resources");

////var startup = new Startup(builder.Configuration);
////builder.Services.AddAppOptions(options);
////startup.ConfigureServices(builder.Services); // calling ConfigureServices method

//builder.Services.Configure<CookiePolicyOptions>(builder.Configuration.GetSection("CookiePolicy"));

////XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()), new FileInfo("log4net.config"));

//builder.Services.AddEndpointsApiExplorer();
////builder.Logging.SetMinimumLevel(LogLevel.Debug).AddLog4Net();
//builder.Services.AddControllers();

//var app = builder.Build();

//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

////Add localization languages and default settings
//var supportedCultures = new[] { "en-US", "ar" };
//var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
//    .AddSupportedCultures(supportedCultures)
//    .AddSupportedUICultures(supportedCultures);
//localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
//app.UseRequestLocalization(localizationOptions);

//// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
////    app.UseSwagger();
////    app.UseSwaggerUI();
////}

//app.UseSwagger();
//// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
//// specifying the Swagger JSON endpoint.
//app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "Gym_App.Api v1"));
//app.UseHttpsRedirection();
//app.UseAuthorization();

//app.MapControllers();
//app.UseCookiePolicy();

//builder.Configuration.AddEnvironmentVariables();

//app.UseSwagger();
//// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
//// specifying the Swagger JSON endpoint.
//app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "Saras.eMarking.Api v1"));

//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}
//else
//{
//    app.UseStatusCodePages();
//    app.UseExceptionHandler("/Error");
//    app.UseHsts();
//}
//app.UseDefaultFiles();
//app.UseStaticFiles();

//app.UseRouting();
//////app.UseCors("AllowAny");
//app.UseAuthMiddleware();
//app.UseAuthentication();
//app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

//using (var scope = app.Services.CreateScope())
//{
//    IServiceProvider services = scope.ServiceProvider;
//    ILogger<Program> logger = null;
//    try
//    {
//        logger = services.GetRequiredService<ILogger<Program>>();
//        //Insert all the initial data to AppCache object
//        logger.LogDebug("GenerateAppCache started.");
//       // var appCache = services.GetRequiredService<IAppCache>();
//       // var appCacheService = services.GetRequiredService<IAppCacheService>();
//       // appCache.GenerateAppCache(appCacheService);
//        logger.LogDebug("GenerateAppCache completed successfully.");
//    }
//    catch (Exception ex)
//    {
//        logger.LogError(ex, "An error occurred while binding the App Cache data.");
//    }

//}

//app.Run();
//}
//catch (Exception ex) {

//    Console.WriteLine("Error" + ex);
//}