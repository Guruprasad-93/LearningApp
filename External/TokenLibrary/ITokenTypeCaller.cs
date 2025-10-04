using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenLibrary
{
    internal interface ITokenTypeCaller
    {
        TokenCallerType TokenCallerType { get; }
        TokenResponse Generate(TokenUserContext tokenUserContext);
    }
}
