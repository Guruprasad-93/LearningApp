using gym_app.Domain.Entities;
using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities.Security;
using gym_app.Domain.Interfaces.BusinessInterface;
using gym_app.Domain.Interfaces.RepositoryInterface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Business.Global
{
    public class RoleService : BaseService<RoleService>, IRoleService
    {
        private readonly IRoleRepository roleRepository;
        private readonly JwtOptions JwtOptions;
        public RoleService(IRoleRepository _roleRepository,AppOptions _appOptions, ILogger<RoleService> _logger) : base(_appOptions, _logger)
        {
            roleRepository = _roleRepository;
            JwtOptions = _appOptions.JwtOptions;
        }

        public async Task<List<Role>> GetRoleDetails()
        {
            List<Role> role = new List<Role>();
            try
            {
                logger.LogDebug("RoleService > GetRoleDetails() started");

                role = await roleRepository.GetRoleDetails();

                logger.LogDebug("RoleService > GetRoleDetails() completed");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in RoleService > GetRoleDetails()");
                throw;
            }

            return role;
        }

        public async Task<string> RoleCreationUpdation(Role role)
        {
            string Result = "";
            try
            {
                logger.LogDebug("RoleService > RoleCreationUpdation() started {model}", role);

                Result = await roleRepository.RoleCreationUpdation(role);

                logger.LogDebug("RoleService > RoleCreationUpdation() completed {model}", role);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in RoleService > RoleCreationUpdation() {model}", role);
                throw;
            }

            return Result;
        }
    }
}
