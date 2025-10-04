using gym_app.Domain.Entities;
using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities.Security;
using gym_app.Domain.Interfaces.BusinessInterface;
using gym_app.Domain.Interfaces.RepositoryInterface;
using gym_app.Domain.Model.Auth;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenLibrary;
using TokenLibrary.EncryptDecrypt.AES;
using TokenLibrary.JwtToken;

namespace gym_app.Business.AuthService
{
    public class AuthService : BaseService<AuthService>, IAuthService
    {
        private readonly IAuthRepository authRepository;
        private readonly JwtOptions JwtOptions;

        public AuthService(IAuthRepository _authRepository, ILogger<AuthService> _logger, AppOptions appOptions) : base(appOptions, _logger)
        {
            authRepository = _authRepository;
            JwtOptions = appOptions.JwtOptions;
        }
        public AuthenticateResponseModel Authenticate(AuthenticateRequestModel loginModel, string ipAddress, bool isSso)
        {
            AuthenticateResponseModel authenticateResponseModel = new AuthenticateResponseModel();

            try
            {
                logger.LogDebug("AuthService > Authenticate() started {model}", loginModel);

               //var pwd = TokenLibrary.EncryptDecrypt.Hmac.Hashing.GetHash(appOptions.AppSettings.ChangePasswords.EncryptionKey, loginModel.Password);
               // if(pwd != null)
               // {
               //     //loginModel.Password = pwd;
               // }

                UserContext userContext = authRepository.Authenticate(loginModel, isSso);

                if (userContext == null) return null;

                string SessionId = Guid.NewGuid().ToString();

                if (userContext.Status == "N001")
                {
                    authenticateResponseModel = new AuthenticateResponseModel
                    {
                        Status = userContext.Status,
                       // UserId = userContext.UserId,
                        LoginId = userContext?.LoginId
                    };
                }
                else
                {
                    authenticateResponseModel = GenerateAuthToken(userContext, SessionId, ipAddress);
                    authenticateResponseModel.SessionKey = SessionId;
                    authenticateResponseModel.IsFirstTimeLoggedIn = userContext.IsFirstTimeLoggedIn;
                    authenticateResponseModel.LoginId = userContext.LoginId;
                    authenticateResponseModel.Status = userContext.Status;
                    authenticateResponseModel.UserId = userContext.UserId;
                }

                logger.LogDebug("AuthService > Authenticate() completed {model}", loginModel);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error in AuthService > Authenticate() {model}", loginModel);
                throw;
            }

            return authenticateResponseModel;
        }

        public UserLoginToken IsTokenValid(string accessToken, string refreshToken, string refAccessToken)
        {
            UserLoginToken userToken = null;
            try
            {
                logger.LogDebug("AuthService > IsTokenValid() method started token = {jwttoken}", accessToken);

                EncryptDecryptAes.StrEncryptionKey = appOptions.AppSettings.EncryptionAlgorithmKey;
                var userid = EncryptDecryptAes.DecryptStringAES(refAccessToken);

                userToken = authRepository.IsTokenValid(accessToken, refreshToken, Convert.ToInt64(userid));

                logger.LogDebug("AuthService > IsTokenValid() method completed token = {jwttoken}", accessToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, " Error in AuthService > IsTokenValid() method token= {jwttoken}", accessToken);
            }
            return userToken;
        }

        public AuthenticateResponseModel RefreshToken(string refreshToken, string token, string IpAddress, string? csrftoken)
        {
            AuthenticateResponseModel authenticateResponseModel = new AuthenticateResponseModel();
            try
            {
                logger.LogDebug("AuthService > RefreshToken() started {refreshtoken}", refreshToken);

                UserLoginTokenModel userLoginTokenModel = authRepository.GetRefreshToken(refreshToken, token);

                if (userLoginTokenModel == null) return null;

                UserContext userContext = authRepository.GetUserContext(userLoginTokenModel.UserName);

                authenticateResponseModel = GenerateAuthToken(userContext, userLoginTokenModel.sessionId, IpAddress, userLoginTokenModel);

                logger.LogDebug("AuthService > RefreshToken() completed {refreshtoken}", refreshToken);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AuthService > RefreshToken() {refreshtoken}", refreshToken);
                throw;
            }
            return authenticateResponseModel;
        }

        public bool RevokeToken(string token, string jwttoken, string ipAddress, bool IsLogout)
        {
            try
            {
                logger.LogDebug("AuthService > RevokeToken() method started token = {token}", token);

                UserLoginTokenModel refreshToken = authRepository.GetRefreshToken(token, jwttoken);
                if (refreshToken != null)
                {
                    refreshToken.IsExpired = true;
                    refreshToken.Revoked = DateTime.UtcNow;
                    refreshToken.IsExpired = true;
                    refreshToken.IsActive = false;
                    refreshToken.IpAddress = ipAddress;
                    refreshToken.JwtToken = jwttoken;
                    refreshToken.IsDeleted = true;
                    authRepository.UpdateRefreshToken(refreshToken, ipAddress, IsLogout);
                }
                logger.LogDebug("AuthService > RevokeToken() method completed token = {token}", token);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, " Error in AuthService > RevokeToken() method token= {token}", token);
            }
            return true;
        }

        private AuthenticateResponseModel GenerateAuthToken(UserContext userContext, string sessionId, string ipAddress, UserLoginTokenModel userLoginTokenModel = null)
        {
            AuthenticateResponseModel authenticateResponseModel = null;
            try
            {
                #region Generate Jwt Token;

                if (userContext == null) return authenticateResponseModel;

                TokenUserContext tokenUserContext = new()
                {
                    EmailId = userContext.EmailId,
                    LoginId = userContext.LoginId,
                    SessionId = sessionId,
                    UserId = userContext.UserId
                };

                if(userContext.userTimeZone != null)
                {
                    tokenUserContext.userTimeZone = new TokenLibrary.UserTimeZone
                    {
                        TimeZoneCode = userContext.userTimeZone.TimeZoneCode,
                        TimeZoneName = userContext.userTimeZone.TimeZoneName,
                        BaseUTCOffsetInMin = userContext.userTimeZone.BaseUTCOffsetInMin
                    };
                }

                if(userContext.CurrentRole != null)
                {
                    tokenUserContext.CurrentRole = new TokenLibrary.UserRole
                    {
                        RoleCode = userContext.CurrentRole.RoleCode,
                        RoleName = userContext.CurrentRole.RoleName,
                        RoleId = userContext.CurrentRole.RoleId,
                        //RoleType = (TokenLibrary.UserRoleType)userContext.CurrentRole.RoleType,
                    };

                }
                TokenJwtOptions tokenJwtOptions = new()
                {
                    EncryptionAlgorithmkey = appOptions.AppSettings.EncryptionAlgorithmKey,
                    RefreshTokenValidityInMinutes = JwtOptions.RefreshTokenValidityInMinutes,
                    Secret = JwtOptions.Secret,
                    TokenValidityInMinutes = JwtOptions.TokenValidityInMinutes,
                    ValidAudience = JwtOptions.ValidAudience,
                    ValidIssuer = JwtOptions.ValidIssuer
                };

                // Generate Jwt Token

                var JwtTokenResponse = TokenGenerator.GetToken(tokenUserContext, TokenCallerType.JWT, tokenJwtOptions);

                //save jwt token to database table

                if(JwtTokenResponse != null)
                {
                    UserLoginTokenModel userLoginTokenModel1 = new()
                    {
                        sessionId = sessionId,
                        CreatedBy = userContext.UserId,
                        CreatedDate = DateTime.Now,
                        JwtToken = JwtTokenResponse.Token,
                        RefreshToken = JwtTokenResponse.RefreshToken,
                        IsActive = true,
                        IsExpired = false,
                        Expires = DateTime.UtcNow.AddMinutes(JwtOptions.RefreshTokenValidityInMinutes),
                        IpAddress = ipAddress,
                        UserName = userContext.LoginId,
                        IsDeleted = false,
                        UserId = userContext.UserId,
                        CompanyId = userContext.CompanyId,

                    };
                    if(userLoginTokenModel != null)
                    {
                        userLoginTokenModel1.Revoked = DateTime.UtcNow;
                        userLoginTokenModel1.RevokedBy = userLoginTokenModel1.UserId;

                    }

                    authRepository.UpdateRefreshToken(userLoginTokenModel1, ipAddress, false);

                    authenticateResponseModel = new()
                    {
                        Token = JwtTokenResponse.Token,
                        RefreshToken = JwtTokenResponse.RefreshToken,
                        RefreshInterval = JwtTokenResponse.RefreshInterval,
                        Roles = userContext.CurrentRole.RoleCode,
                        SessionKey = userContext.SessionId,
                        RefKey = JwtTokenResponse.RefKey
                    };
                }
                #endregion Generate JWT Token
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AuthService > GenerateAuthToken()");
                throw;
            }

            return authenticateResponseModel;
        }
    }
}
