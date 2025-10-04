using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenLibrary.JwtToken;

namespace TokenLibrary.Factory
{
    internal static class TokenCallerFactory
    {
        public static ITokenTypeCaller? LoadIntgrationCaller(TokenCallerType tokenCallerType , IAppOptions appOptions)
        {
            ITokenTypeCaller? caller = null;

            switch (tokenCallerType)
            {
                case TokenCallerType.JWT:
                    {
                        caller = new JwtTokens(tokenCallerType, (TokenJwtOptions)appOptions);
                    }
                    break;

                default:
                    break;
            }
            return caller;
        }
    }
}
