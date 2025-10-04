using gym_app.Domain.Entities;
using gym_app.Domain.Model.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Interfaces.RepositoryInterface
{
    public interface IAuthRepository
    {
        UserContext Authenticate(AuthenticateRequestModel loginModel, bool isSso);
        UserLoginTokenModel GetRefreshToken(string refreshToken, string token);
        UserContext GetUserContext(string? userName);
        UserLoginToken? IsTokenValid(object jwttoken, object refToken, long v);
        bool UpdateRefreshToken(UserLoginTokenModel userLoginTokenModel1, string ipAddress, bool v);
    }
}
