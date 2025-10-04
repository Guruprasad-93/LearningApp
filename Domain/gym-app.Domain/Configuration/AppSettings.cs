using gym_app.Domain.Model.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Configuration
{
    public static class DecryptDomain
    {
       // private static string AesDecryptkey { get; set; } = "K+iCU4H+AtV4uy0+Skmo8w==";
         private static string AesDecryptkey { get; set; } = "T@yek#MAET7Sec";
        public static string DecryptAes(string stringToDecrypt)
        {
            if (stringToDecrypt != null && stringToDecrypt.Length > 0)
            {
                return TokenLibrary.EncryptDecrypt.AesCrypto.EncryptDecryptAesCrypto.Decrypt(stringToDecrypt, AesDecryptkey);
            }
            else
                return string.Empty;
        }
    }

    public class AppSettings
    {

        public bool EncryptConnectionString { get; set; }

        public string GatewayBaseURL { get; set; }

        public string ViewSolutionURL { get; set; }

        /// <summary>
        /// Gets or Sets the encryption algorithm key.
        /// </summary>
        public string EncryptionAlgorithmKey { get; set; }

        /// <summary>
        /// Gets or Sets the image response URL.
        /// </summary>
        public string ImageResponseURL { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether password encrypted in is client.
        /// </summary>
        public bool IsPasswordEncryptedInClient { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether audit log is enabled.
        /// </summary>
        public bool IsAuditLogEnabled { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether csrf validation is enabled.
        /// </summary>
        public bool IsCsrfValidationEnabled { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether xss validation is enabled.
        /// </summary>
        public bool IsXssValidationEnabled { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether build number display is enabled.
        /// </summary>
        public bool IsBuildNumberDisplayEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the build number.
        /// </summary>
        public string BuildNumber { get; set; }

        /// <summary>
        /// Gets or Sets the report sync page size.
        /// </summary>
        public string ReportSyncPageSize { get; set; }

        /// <summary>
        /// Gets or Sets the branding.
        /// </summary>
        public Branding Branding { get; set; }

        /// <summary>
        /// Gets or Sets the calendar.
        /// </summary>
        public CalendarSetting Calendar { get; set; }

        /// <summary>
        /// Gets or Sets the change passwords.
        /// </summary>
        public ChangePassword ChangePasswords { get; set; }

        /// <summary>
        /// Gets or Sets the forgot passwords.
        /// </summary>
        public ForgotPassword ForgotPasswords { get; set; }

        /// <summary>
        /// Gets or Sets the media service config.
        /// </summary>
        public MediaServiceConfig MediaServiceConfig { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether captcha is enabled.
        /// </summary>
        public bool IsCaptchaEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the outbound api url.
        /// </summary>
        public string InboundApiUrl { get; set; }

        /// <summary>
        /// Gets or Sets the excel cell max length.
        /// </summary>
        public long ExcelCellMaxLength { get; set; }

        /// <summary>
        /// Gets or Sets the quartz api url.
        /// </summary>
        public string QuartzApiUrl { get; set; }
        public string EnvironmentName { get; set; }

        /// <summary>
        /// Gets or Sets the language.
        /// </summary>
        public LanguageTheme Language { get; set; }

        /// <summary>
        /// Gets or Sets the color.
        /// </summary>
        public ColorTheme Color { get; set; }
        public bool IsTestPlayerView { get; set; }

        /// <summary>
        /// Gets or Sets the image response URL.
        /// </summary>
        public bool IsImageBase64Required { get; set; }

        public bool IsPreviousMarkerDataRequired { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public AppSettings(IConfiguration config)
        {

            EncryptConnectionString = config.GetValue("AppSettings:EncryptConnectionString", true);
            GatewayBaseURL = config.GetValue("AppSettings:GatewayBaseURL", "https://slsweb.excelindia.com/eMarkingAPI/api/public/v1/");
            ImageResponseURL = config.GetValue("AppSettings:ImageResponseURL", "");
            ViewSolutionURL = config.GetValue("AppSettings:ViewSolutionURL", "https://slsweb.excelindia.com/eMarkingTestPlayer/");
            EncryptionAlgorithmKey = config.GetValue("AppSettings:EncryptionAlgorithmKey", "4512631236589784");
            IsAuditLogEnabled = config.GetValue("AppSettings:IsAuditLogEnabled", true);
            IsCsrfValidationEnabled = config.GetValue("AppSettings:IsCsrfValidationEnabled", false);
            IsXssValidationEnabled = config.GetValue("AppSettings:IsXssValidationEnabled", true);
            IsBuildNumberDisplayEnabled = config.GetValue("AppSettings:IsBuildNumberDisplayEnabled", true);
            BuildNumber = config.GetValue("AppSettings:BuildNumber", "Emark2021.1.250.0");
            ReportSyncPageSize = config.GetValue("AppSettings:ReportSyncPageSize", "100");
            Branding = new Branding(config);
            Calendar = new CalendarSetting(config);
            //ChangePasswords = new ChangePassword(config);
          //  ForgotPasswords = new ForgotPassword(config);
            MediaServiceConfig = new MediaServiceConfig(config);
            IsCaptchaEnabled = config.GetValue("AppSettings:IsCaptchaEnabled", true);
            IsPasswordEncryptedInClient = config.GetValue("AppSettings:IsPasswordEncryptedInClient", false);
            InboundApiUrl = config.GetValue("AppSettings:InboundApiUrl", "");
            ExcelCellMaxLength = config.GetValue("AppSettings:ExcelCellMaxLength", 32000);
            QuartzApiUrl = config.GetValue("AppSettings:QuartzApiURL", string.Empty);
            EnvironmentName = config.GetValue("AppSettings:EnvironmentName", string.Empty);

            Language = config.GetSection("AppSettings:LanguageTheme").Get<LanguageTheme>();
            Color = config.GetSection("AppSettings:ColorTheme").Get<ColorTheme>();
            IsTestPlayerView = config.GetValue("AppSettings:IsTestPlayerView", false);
            IsImageBase64Required = config.GetValue("AppSettings:IsImageBase64Required", false);
            IsPreviousMarkerDataRequired = config.GetValue("AppSettings:IsPreviousMarkerDataRequired", true);
        }
    }

    /// <summary>
    /// The media service config.
    /// </summary>
    public class MediaServiceConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaServiceConfig"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public MediaServiceConfig(IConfiguration config)
        {
         //   RepoType = config.GetValue("AppSettings:MediaServiceConfig:RepoType", FileUploadRepo.LocalRepo);
            LocalRepoPath = config.GetValue("AppSettings:MediaServiceConfig:LocalRepoPath", string.Empty);
            ApplicationTypeName = config.GetValue("AppSettings:MediaServiceConfig:ApplicationTypeName", "eAssessment");
            CloudContainerName = config.GetValue("AppSettings:MediaServiceConfig:CloudContainerName", "eMarkingRepository");
            ApplicationModuleCode = config.GetValue("AppSettings:MediaServiceConfig:ApplicationModuleCode", "itemauthoring");
            ProjectCode = config.GetValue("AppSettings:MediaServiceConfig:ProjectCode", "SEAB");
            URLCode = config.GetValue("AppSettings:MediaServiceConfig:URLCode", "AWSS3");
            ISS3Config = config.GetValue("AppSettings:MediaServiceConfig:ISS3Config", "0");
        }

        /// <summary>
        /// Gets or Sets the repo type.
        /// </summary>
      //  public FileUploadRepo RepoType { get; set; }

        /// <summary>
        /// Gets or Sets the local repo path.
        /// </summary>
        public string LocalRepoPath { get; set; }

        /// <summary>
        /// Gets or Sets the application type name.
        /// </summary>
        public string ApplicationTypeName { get; set; }

        /// <summary>
        /// Gets or Sets the cloud container name.
        /// </summary>
        public string CloudContainerName { get; set; }

        /// <summary>
        /// Gets or Sets the application module code.
        /// </summary>
        public string ApplicationModuleCode { get; set; }

        /// <summary>
        /// Gets or Sets the project code.
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// Gets or Sets the URL code.
        /// </summary>
        public string URLCode { get; set; }

        /// <summary>
        /// Gets or sets the i s s3 config.
        /// </summary>
        public string ISS3Config { get; set; }
    }

    /// <summary>
    /// The change password.
    /// </summary>
    public class ChangePassword
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePassword"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public ChangePassword(IConfiguration config)
        {
            NoOfAttemps = config.GetValue("AppSettings:ChangePasswords:NoOfAttemps", 5);
            EncryptionKey = DecryptDomain.DecryptAes(config.GetValue("AppSettings:ChangePasswords:EncryptionKey", "EkI4dvsE1p1REtQ/lkcFKNeGR+YB57QEFKmsCv3J6MMO6j14Xo4s6yHThhSdScl/"));
            DefaultPwd = DecryptDomain.DecryptAes(config.GetValue("AppSettings:ChangePasswords:DefaultPwd", "mrYQ+aiL37ysFSddJl5YxBJTaEVCvcmHAFVkNaHCLFGsGsM/NVzzZ3zSiPznLIS4"));
        }

        /// <summary>
        /// Gets or Sets the no of attemps.
        /// </summary>
        public int NoOfAttemps { get; set; }

        /// <summary>
        /// Gets or Sets the encryption key.
        /// </summary>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Gets or Sets the default pwd.
        /// </summary>
        public string DefaultPwd { get; set; }
    }

    /// <summary>
    /// The forgot password.
    /// </summary>
    public class ForgotPassword
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPassword"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public ForgotPassword(IConfiguration config)
        {
            ForgotPasswordNoofAttemps = Convert.ToByte(config.GetValue("AppSettings:ForgotPasswords:ForgotPasswordNoofAttemps", "10"));
        }

        /// <summary>
        /// Gets or Sets the forgot password noof attemps.
        /// </summary>
        public byte ForgotPasswordNoofAttemps { get; set; }
    }

    /// <summary>
    /// The calendar setting.
    /// </summary>
    public class CalendarSetting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarSetting"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public CalendarSetting(IConfiguration config)
        {
            string zone = config.GetValue("AppSettings:Calendar:TimeZoneFrom", "2");
            _ = Enum.TryParse(zone, out EnumTimeZoneFrom timeZoneFrom);
            TimeZoneFrom = timeZoneFrom;
           // DefaultTimeZone = new UserTimeZone(config);
        }

        /// <summary>
        /// Gets or Sets the time zone from.
        /// </summary>
        public EnumTimeZoneFrom TimeZoneFrom { get; set; }

        /// <summary>
        /// Gets or Sets the default time zone.
        /// </summary>
        public UserTimeZone DefaultTimeZone { get; set; }
    }

    /// <summary>
    /// The enum time zone from.
    /// </summary>
    public enum EnumTimeZoneFrom
    {
        None = 0,
        UserProfile = 1,
        UserBrowser = 2
    }

    /// <summary>
    /// The branding.
    /// </summary>
    public class Branding
    {
        public Branding()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Branding"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public Branding(IConfiguration config)
        {
            LogoPath = config.GetValue("AppSettings:Branding:LogoPath", "assets/images/saras-logo.png");
            Copyright = config.GetValue("AppSettings:Branding:Copyright", "Singapore Examinations and Assessment Board");
            Year = config.GetValue("AppSettings:Branding:Year", "2023");
            DefaultUserImage = config.GetValue("AppSettings:Branding:DefaultUserImage", "assets/images/userImg.jpg");
        }

        /// <summary>
        /// Gets or Sets the logo path.
        /// </summary>
        public string LogoPath { get; set; }

        /// <summary>
        /// Gets or Sets the copyright.
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// Gets or Sets the year.
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Gets or Sets the default user image.
        /// </summary>
        public string DefaultUserImage { get; set; }
    }

    /// <summary>
    /// The language theme.
    /// </summary>
    public class LanguageTheme
    {
        /// <summary>
        /// Gets or Sets a value indicating whether multi language enabled.
        /// </summary>
        public bool MultiLanguageEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the languages.
        /// </summary>
        public List<Language> Languages { get; set; }
    }

    /// <summary>
    /// The color theme.
    /// </summary>
    public class ColorTheme
    {
        /// <summary>
        /// Gets or Sets a value indicating whether multi color enabled.
        /// </summary>
        public bool MultiColorEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the colors.
        /// </summary>
        public List<Color> Colors { get; set; }
    }

    /// <summary>
    /// The language.
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Gets or Sets the language name.
        /// </summary>
        public string LanguageName { get; set; }

        /// <summary>
        /// Gets or Sets the language code.
        /// </summary>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether is default.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or Sets the language class.
        /// </summary>
        public string LanguageClass { get; set; }
    }

    /// <summary>
    /// The color.
    /// </summary>
    public class Color
    {
        /// <summary>
        /// Gets or Sets the color name.
        /// </summary>
        public string ColorName { get; set; }

        /// <summary>
        /// Gets or Sets the color code.
        /// </summary>
        public string ColorCode { get; set; }

        /// <summary>
        /// Gets or Sets a value indicating whether is default.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
