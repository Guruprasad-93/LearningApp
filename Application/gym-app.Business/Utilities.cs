using gym_app.Domain.Model.Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Business
{
    public class Utilities
    {

        public static void InsertStringCookie(HttpContext httpContext, string cookieName, string cookieObject, double CookieExpiryInMinute = 0)
        {
            if(httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            if (string.IsNullOrEmpty(cookieName))
            {
                throw new ArgumentException($"'{nameof(cookieName)}' cannot be null or empty.", nameof(cookieName));
            }
            if (string.IsNullOrEmpty(cookieObject))
            {
                throw new ArgumentException($"'{nameof(cookieObject)}' cannot be null or empty.", nameof(cookieObject));
            }

            if(httpContext.Response != null && httpContext.Request != null)
            {
                httpContext.Response.Cookies.Append(cookieName, cookieObject, new CookieOptions()
                {
                    Expires = CookieExpiryInMinute <= 0 ? null : DateTime.UtcNow.AddMinutes(CookieExpiryInMinute)
                });
            }
        }
    }
}
