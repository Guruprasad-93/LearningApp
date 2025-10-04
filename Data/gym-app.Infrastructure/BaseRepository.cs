using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Infrastructure
{
    public abstract class BaseRepository<T> where T : class
    {
        public readonly ILogger logger;

        protected BaseRepository(ILogger<T> _logger)
        {
            logger = _logger;
        }
    }
}
