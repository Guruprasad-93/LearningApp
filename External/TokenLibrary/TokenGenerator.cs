using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenLibrary.Factory;
using TokenLibrary.JwtToken;

namespace TokenLibrary
{
    public static class TokenGenerator
    {
        public static TokenResponse? GetToken(TokenUserContext tokenUserContext, TokenCallerType tokenCallerType, IAppOptions appOptions)
        {
            TokenResponse? tokenResponse = null;

            var caller = TokenCallerFactory.LoadIntgrationCaller(tokenCallerType, appOptions);

            if(caller != null)
            {
                tokenResponse = caller.Generate(tokenUserContext);
            }
            return tokenResponse;
        }
    }
}
