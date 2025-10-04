using gym_app.Domain.Entities;
using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Interfaces.RepositoryInterface;
using gym_app.Domain.Model.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Infrastructure.Repository
{
    public class AuthRepository : BaseRepository<AuthRepository>, IAuthRepository
    {
        private readonly ApplicationDbContext context;
        public AppOptions appOptions { get; set; }
        public AuthRepository(ApplicationDbContext _context, ILogger<AuthRepository> _logger, AppOptions _appOptions) : base(_logger)
        {
            context = _context;
            appOptions = _appOptions;
        }

        public UserContext Authenticate(AuthenticateRequestModel loginModel, bool isSso)
        {
            UserContext userContext = new UserContext();
            string status = string.Empty;
            try
            {
                logger.LogDebug("AuthRepository > Authenticate() started {model}", loginModel);

                var user = context.Users.FirstOrDefault(a => a.UserName == loginModel.LoginName && a.Password == loginModel.Password && !a.IsDeleted);

                if (user != null)
                {
                    userContext.UserId = user.UserId;
                    userContext.LoginId = user.UserName;
                    userContext.CompanyId = user.CompanyId;
                    userContext.EmailId = user.Email;
                    userContext.UserId = user.UserId;
                    userContext.Status = "S001";
                    var role = context.Roles.FirstOrDefault(a => a.RoleId == user.RoleId);
                    if (role != null)
                    {
                        UserRole userRole = new UserRole();
                        userRole.RoleId = role.RoleId;
                        userRole.RoleCode = role?.RoleCode;
                        userRole.RoleName = role?.RoleName;

                        userContext.CurrentRole.RoleCode = role?.RoleCode;
                        userContext.CurrentRole.RoleId = (long)(role?.RoleId);
                        userContext.CurrentRole.RoleName = role?.RoleName;
                    }
                    
                }
                else
                {
                    userContext.Status = "N001";
                    userContext.LoginId = loginModel.LoginName;
                    
                }

                    logger.LogDebug("AuthRepository > Authenticate() Completed {model}", loginModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AuthRepository > Authenticate() {model}", loginModel);
                throw;
            }

            return userContext;

        }

        public bool UpdateRefreshToken(UserLoginTokenModel userLoginTokenModel1, string ipAddress, bool IsLogout)
        {
            try
            {
                logger.LogDebug("AuthRepository > UpdateRefreshToken() started {userLoginTokenModel1}", userLoginTokenModel1);

                if (!IsLogout)
                {
                    DeleteExistingUserToken(userLoginTokenModel1);

                    UserLoginToken userLoginToken = new()
                    {
                        RefreshToken = userLoginTokenModel1.RefreshToken,
                        JwtToken = userLoginTokenModel1.JwtToken,
                        UserId = userLoginTokenModel1.UserId,
                        UserName = userLoginTokenModel1.UserName,
                        Expires = userLoginTokenModel1.Expires,
                        IsExpired = false,
                        IsActive = true,
                        CreatedBy = userLoginTokenModel1.CreatedBy,
                        IsDeleted = false,
                        CreatedDate = DateTime.UtcNow,
                        Revoked = userLoginTokenModel1.Revoked,
                        RevokedBy = userLoginTokenModel1.RevokedBy,
                        CompanyId = userLoginTokenModel1.CompanyId,

                    };

                    context.UserLoginTokens.Add(userLoginToken);
                    context.SaveChanges();
                }
                else
                {
                    var refTokens = context.UserLoginTokens.Where(x => x.UserId == userLoginTokenModel1.UserId && x.UserName == userLoginTokenModel1.UserName).ToList();

                    foreach (var item in refTokens)
                    {
                        AddUserTokenToArchive(item);
                    }

                    context.SaveChanges();
                }

                logger.LogDebug("AuthRepository > UpdateRefreshToken() started {userLoginTokenModel1}", userLoginTokenModel1);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error in AuthRepository > UpdateRefreshToken() {userLoginTokenModel1}", userLoginTokenModel1);
                throw;
            }
            return true;
        }

        private void AddUserTokenToArchive(UserLoginToken userLoginToken)
        {
            throw new NotImplementedException();
        }

        private void DeleteExistingUserToken(UserLoginTokenModel userLoginToken)
        {
            var refTokens = context.UserLoginTokens.Where(x => x.UserId == userLoginToken.UserId && x.UserName == userLoginToken.UserName && !x.IsDeleted).ToList();

            foreach (var item in refTokens)
            {
                item.IsDeleted = true;
                item.IsActive = false;
                item.IsExpired = true;

                context.UserLoginTokens.Update(item);
            }

            context.SaveChanges();
           
        }

        public UserLoginTokenModel GetRefreshToken(string refreshToken, string token)
        {
            try
            {
                logger.LogDebug("AuthRepository > GetRefreshToken() started {refreshToken}", refreshToken);

                UserLoginToken userLoginToken = context.UserLoginTokens.FirstOrDefault(a => a.RefreshToken == refreshToken && !a.IsDeleted);

                if (userLoginToken == null) return null;

                logger.LogDebug("AuthRepository > GetRefreshToken() Completed {refreshToken}", refreshToken);

                return userLoginToken.IsActive == false ? null : new UserLoginTokenModel
                {
                    UserId = userLoginToken.UserId,
                    TokenId = userLoginToken.TokenId,
                    UserName = userLoginToken.UserName,
                    RefreshToken = userLoginToken.RefreshToken,
                    Expires = userLoginToken.Expires,
                    CreatedBy = userLoginToken.CreatedBy,
                    CreatedDate = userLoginToken.CreatedDate,
                    Revoked = userLoginToken.Revoked,
                    RevokedBy = userLoginToken.RevokedBy,
                    IsExpired = userLoginToken.IsExpired,
                    IsActive = userLoginToken.IsActive,
                    JwtToken = userLoginToken.JwtToken,
                    IsDeleted = userLoginToken.IsDeleted,
                    CompanyId = userLoginToken.CompanyId,
                };

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AuthRepository > GetRefreshToken() {refreshToken}", refreshToken);
                throw;
            }
        }

        public UserContext GetUserContext(string? userName)
        {
            UserContext userContext = new UserContext();
            try
            {
                logger.LogDebug("AuthRepository > GetUserContext() started {userName}", userName);

               var  uc = (from u in context.Users
                                join r in context.Roles on u.RoleId equals r.RoleId
                                where u.UserName == userName && !u.IsDeleted && u.IsActive && !r.IsDeleted
                                select new UserContext
                                {
                                    UserId = u.UserId,
                                    LoginId = u.UserName,
                                    EmailId = u.Email,
                                    CurrentRole = new UserRole
                                    {
                                        RoleId = r.RoleId,
                                        RoleCode = r.RoleCode,
                                        RoleName = r.RoleName,
                                    }
                                }).FirstOrDefault();
               
                if(uc != null && uc.CurrentRole != null)
                {
                    userContext = uc;
                }
                logger.LogDebug("AuthRepository > GetUserContext() started {userName}", userName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AuthRepository > GetUserContext() {userName}", userName);
                throw;
            }

            return userContext;
        }

        public UserLoginToken? IsTokenValid(object jwttoken, object refToken, long userId)
        {
            UserLoginToken userLoginToken = null;
            try
            {
                logger.LogDebug("AuthRepository > IsTokenValid() method started {jwttoken}", jwttoken);

                var userLoginTokens = context.UserLoginTokens.Where(x => x.UserId == userId && x.IsExpired == false && DateTime.UtcNow <= x.Expires).ToList();

                if (userLoginTokens.Count > 0 && userLoginTokens.Exists(a => a.RefreshToken == refToken && a.JwtToken == jwttoken))
                {
                    userLoginToken = userLoginTokens.Find(a => a.RefreshToken == refToken && a.JwtToken == jwttoken);
                }

                logger.LogDebug("AuthRepository > IsTokenValid() method completed {jwttoken}", jwttoken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AuthRepository > IsTokenValid() method token = {jwttoken}", jwttoken);
            }
            return userLoginToken;
        }
    }
}
