using gym_app.Domain.Configuration;
using gym_app.Domain.Entities.Security;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace gym_app.Domain.Entities.AppOption
{
    public class AppOptions
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public AppMode AppMode { get; set; }
        public string Scope { get; set; }
        public AppSettings AppSettings { get; set; }
        public JwtOptions JwtOptions { get; set; }

        public SsoIntegrationOptions SsoIntegrationOptions { get; set; }

        public static AppOptions ReadFromConfiguration(IConfiguration config)
        {
            var options = new AppOptions();

            var apm = config.GetValue("AppOptions:AppMode", "Development");
            _ = Enum.TryParse(apm, out AppMode apmode);
            options.AppMode = apmode;

            options.Scope = config.GetValue("AppOptions:Scope", "dev");

            options.AppSettings = new AppSettings(config);

            options.JwtOptions = new JwtOptions(config);

            options.SsoIntegrationOptions = config.GetSection("SsoIntegration").Get<SsoIntegrationOptions>();

            return options;
        }
    }

    public enum AppMode
    {
        Development,
        Testing,
        Staging,
        Production
    }

    public static class AppConfigurationExtensions
    {
        public static string ToScope(this AppMode mode)
        {
            return mode switch
            {
                AppMode.Staging => "stage",
                AppMode.Production => "prod",
                AppMode.Testing => "testing",
                _ => "dev",
            };
        }
    }
}
