using gym_app.Domain.Model.CommonModel;
using gym_app.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace gym_app.Domain.Interfaces.BusinessInterface
{
    public interface IUserService
    {
        Task<List<UserDetails>> GetUserDetails(CommonModelClass commonModelClass);
        Task<UserProfile> GetUserProfile(CommonModelClass commonModelClass);
        Task<string> UserCreationUpdation(UserDetailsModel userDetailsModel, string webRootPath);
    }
}
