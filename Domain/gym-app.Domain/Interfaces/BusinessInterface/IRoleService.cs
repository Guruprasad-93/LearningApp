using gym_app.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Interfaces.BusinessInterface
{
    public interface IRoleService
    {
        Task<List<Role>> GetRoleDetails();
        Task<string> RoleCreationUpdation(Role role);
    }
}
