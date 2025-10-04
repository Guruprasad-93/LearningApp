using gym_app.Domain.Model.CommonModel;
using gym_app.Domain.Model.MembershipModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Interfaces.BusinessInterface
{
    public interface IMembershipService
    {
        Task<string> MemberShipCreation(MembershipModel membershipModel);
        Task<List<MemberShipDetails>> MemberShipDetails(CommonModelClass commonModelClass);
    }
}
