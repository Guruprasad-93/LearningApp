using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Model.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;
using TokenLibrary.EncryptDecrypt.AES;
using TokenLibrary.JwtToken;

namespace Gym_App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    public abstract class BaseApiController<T> : Controller where T : class
    {
        public const string API_PREFIX = "api/v2";

        public readonly ILogger logger;

        public AppOptions appOptions;

        protected BaseApiController(AppOptions _appOptions, ILogger<T> _logger)
        {
            appOptions = _appOptions;
            logger = _logger;
        }

        private UserContext context = null;
        protected UserContext CurrentUserContext
        {
            get 
            { 
                if (context != null) 
                    return context;

                try
                {
                    context = GetContextData(context);
                }
                catch
                {

                }

                return context;
            }
        }

        private UserContext GetContextData(UserContext context)
        {
            var tokenContext = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
            if (string.IsNullOrEmpty(tokenContext))
                return context;

          //  EncryptDecryptAes.StrEncryptionKey = AppOptions.AppSettings.EncryptionAlgorithmKey;
            context = JsonSerializer.Deserialize<UserContext>(EncryptDecryptAes.DecryptStringAES(tokenContext));

            //if (context != null && context.TimeZone == null)
            //{
            //    if (AppOptions.AppSettings.Calendar.TimeZoneFrom == EnumTimeZoneFrom.UserProfile)
            //    {
            //        context.TimeZone = AppOptions.AppSettings.Calendar.DefaultTimeZone;
            //    }
            //    else if (AppOptions.AppSettings.Calendar.TimeZoneFrom == EnumTimeZoneFrom.UserBrowser)
            //    {
            //        string zone = Convert.ToString(HttpContext.Request.Headers["Local-Time-Zone"]);
            //        string offst = Convert.ToString(HttpContext.Request.Headers["Local-Time-Offset"]);

            //        if (!string.IsNullOrEmpty(zone) && !string.IsNullOrEmpty(offst))
            //        {
            //            string zoneName = Convert.ToString(DateTimeUtils.OlsonTimeZoneToTimeZoneInfo(zone));
            //            context.TimeZone = new UserTimeZone
            //            {
            //                TimeZoneCode = zoneName,
            //                TimeZoneName = zoneName,
            //                BaseUTCOffsetInMin = Convert.ToInt32(offst)
            //            };
            //        }
            //    }
            //}

            return context;
        }

        protected static string SafeText(string textdata)
        {
            return System.Net.WebUtility.HtmlEncode(textdata);
        }
        protected long GetCurrentUserId()
        {
            if (CurrentUserContext != null)
            {
                return CurrentUserContext.UserId;
            }
            else
            {
                return 0;
            }
        }

        protected long GetCurrentCompanyId()
        {
            if (CurrentUserContext != null)
            {
                return CurrentUserContext.CompanyId;
            }
            else
            {
                return 0;
            }
        }

        protected string GetCurrentSessionKey()
        {
            if (CurrentUserContext != null && CurrentUserContext.SessionId != null)
            {
                return CurrentUserContext.SessionId;
            }
            else
            {
                return null;
            }
        }

        protected long GetCurrentUserRoleID()
        {
            if (CurrentUserContext != null && CurrentUserContext.CurrentRole != null)
            {
                return CurrentUserContext.CurrentRole.RoleId;
            }
            else
            {
                return 0;
            }
        }
        protected string GetCurrentUserRoleCode()
        {
            string CurRole = null;
            if (CurrentUserContext != null && CurrentUserContext.CurrentRole != null)
            {
                CurRole = CurrentUserContext.CurrentRole.RoleCode;
            }
            return CurRole;
        }

        //protected bool InsertAuditLogs(AuditTrailData auditTrailData)
        //{
        //    if (auditTrailData == null || AuditService == null || !AppOptions.AppSettings.IsAuditLogEnabled)
        //        return false;

        //    try
        //    {
        //        long userId = GetCurrentUserId();
        //        if (userId > 0)
        //        {
        //            auditTrailData.UserId = userId;
        //            auditTrailData.SessionId = GetCurrentSessionKey();
        //        }

        //        long userRoleId = GetCurrentProjectUserRoleID();
        //        if (userRoleId > 0)
        //        {
        //            auditTrailData.ProjectUserRoleID = userRoleId;
        //            auditTrailData.SessionId = GetCurrentSessionKey();
        //        }

        //        if (string.IsNullOrEmpty(auditTrailData.SessionId))
        //        {
        //            auditTrailData.SessionId = GetCurrentSessionKey();
        //            auditTrailData.ResponseStatus = AuditTrailResponseStatus.Error;
        //        }

        //        AuditService.InsertAuditLogs(auditTrailData);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, "Error in InsertAuditLogs");
        //    }

        //    return true;
        //}

        protected UserTimeZone GetCurrentContextTimeZone()
        {
            UserTimeZone userTimeZone = null;

            if (CurrentUserContext != null && CurrentUserContext.userTimeZone != null && CurrentUserContext.userTimeZone.TimeZoneCode != null)
            {
                userTimeZone = CurrentUserContext.userTimeZone;
            }

            //if (userTimeZone != null || AppOptions.AppSettings.Calendar.TimeZoneFrom != EnumTimeZoneFrom.UserBrowser)
            //    return userTimeZone;
            string zone = Convert.ToString(HttpContext.Request.Headers["Local-Time-Zone"]);
            string offst = Convert.ToString(HttpContext.Request.Headers["Local-Time-Offset"]);
            if (!string.IsNullOrEmpty(zone) && !string.IsNullOrEmpty(offst))
            {
                string zonename = "";//Convert.ToString(DateTimeUtils.OlsonTimeZoneToTimeZoneInfo(zone));
                userTimeZone = new UserTimeZone
                {
                    TimeZoneCode = zonename,
                    TimeZoneName = zonename,
                    BaseUTCOffsetInMin = Convert.ToInt32(offst)
                };
            }
            return userTimeZone;
        }
    }
}
