using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities.Security;
using gym_app.Domain.Interfaces.BusinessInterface;
using gym_app.Domain.Interfaces.RepositoryInterface;
using gym_app.Domain.Model.CommonModel;
using gym_app.Domain.Model.User;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Business.UserService
{
    public class UserService : BaseService<UserService>, IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly JwtOptions JwtOptions;

        public UserService(IUserRepository _userRepository, ILogger<UserService> _logger, AppOptions appOptions) : base(appOptions, _logger)
        {
            userRepository = _userRepository;
            JwtOptions = appOptions.JwtOptions;
        }

        public async Task<List<UserDetails>> GetUserDetails(CommonModelClass commonModelClass)
        {
            List<UserDetails> ltUserDetails = new List<UserDetails>();
            try
            {
                logger.LogDebug("UserService > GetUserDetails() started {model}", commonModelClass);

                ltUserDetails = await userRepository.GetUserDetails(commonModelClass);

                logger.LogDebug("UserService > GetUserDetails() completed {model}", commonModelClass);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in UserService > GetUserDetails() {model}", commonModelClass);
                throw;
            }

            return ltUserDetails;
        }

        public async Task<UserProfile> GetUserProfile(CommonModelClass commonModelClass)
        {
            UserProfile userProfile = new UserProfile();
            try
            {
                logger.LogDebug("UserService > GetUserProfile() started {model}", commonModelClass);

                userProfile = await userRepository.GetUserProfile(commonModelClass);

                logger.LogDebug("UserService > GetUserProfile() completed {model}", commonModelClass);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in UserService > GetUserProfile() {model}", commonModelClass);
                throw;
            }

            return userProfile;
        }

        public async Task<string> UserCreationUpdation(UserDetailsModel userDetailsModel, string webRootPath)
        {
            string Result = "";
            try
            {
                logger.LogDebug("UserService > UserCreationUpdation() started {model}", userDetailsModel);

                 Result = await userRepository.UserCreationUpdation(userDetailsModel, webRootPath);

                logger.LogDebug("UserService > UserCreation() completed {model}", userDetailsModel);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in UserService > UserCreationUpdation() {model}", userDetailsModel);
                throw;
            }

            return Result;
        }
    }
}
