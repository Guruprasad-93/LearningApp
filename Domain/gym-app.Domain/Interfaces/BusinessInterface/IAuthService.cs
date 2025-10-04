using gym_app.Domain.Entities;
using gym_app.Domain.Model.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Interfaces.BusinessInterface
{
    public interface IAuthService
    {
        AuthenticateResponseModel Authenticate(AuthenticateRequestModel loginModel, string ipAddress, bool isSso);
        UserLoginToken IsTokenValid(string accessToken, string refreshToken, string refAccessToken);
        AuthenticateResponseModel RefreshToken(string refreshToken, string token, string IpAddress, string? csrftoken);
        bool RevokeToken(string refreshToken, string accessToken, string v1, bool v2);
    }
}
