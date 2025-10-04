using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Interfaces.RepositoryInterface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Business
{
    public class BaseService<T> where T : class
    {
        public readonly ILogger logger;

        public AppOptions appOptions { get; set; }

        public BaseService(AppOptions _appOptions, ILogger<T> _logger)
        {
            logger = _logger;
            appOptions = _appOptions;
        }

        protected string HtmlDecode(string value)
        {
            return WebUtility.HtmlDecode(value);
        }
        protected string HtmlEncode(string value)
        {
            return WebUtility.HtmlEncode(value);
        }
    }
}
