using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities.Security;
using gym_app.Domain.Interfaces.BusinessInterface;
using gym_app.Domain.Interfaces.RepositoryInterface;
using gym_app.Domain.Model.CommonModel;
using gym_app.Domain.Model.MembershipModel;
using gym_app.Domain.Model.User;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Business.MembershipService
{
    public class MembershipService: BaseService<MembershipService>, IMembershipService
    {
        private readonly IMembershipRepository memberShipRepository;
        private readonly JwtOptions JwtOptions;

        public MembershipService(IMembershipRepository _memberShipRepository, ILogger<MembershipService> _logger, AppOptions appOptions) : base(appOptions, _logger)
        {
            memberShipRepository = _memberShipRepository;
            JwtOptions = appOptions.JwtOptions;
        }

        public async Task<string> MemberShipCreation(MembershipModel membershipModel)
        {
            string Result = "";
            try
            {
                logger.LogDebug("MembershipService > MemberShipCreation() started {model}", membershipModel);

                Result = await memberShipRepository.MemberShipCreation(membershipModel);

                logger.LogDebug("MembershipService > MemberShipCreation() completed {model}", membershipModel);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in MembershipService > MemberShipCreation() {model}", membershipModel);
                throw;
            }

            return Result;
        }

        public async Task<List<MemberShipDetails>> MemberShipDetails(CommonModelClass commonModelClass)
        {
            List<MemberShipDetails> memberShipDetails = new List<MemberShipDetails>();
            try
            {
                logger.LogDebug("MembershipService > MemberShipDetails() started {model}", commonModelClass);

                memberShipDetails = await memberShipRepository.MemberShipDetails(commonModelClass);

                logger.LogDebug("MembershipService > MemberShipDetails() completed {model}", commonModelClass);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in MembershipService > MemberShipDetails() {model}", commonModelClass);
                throw;
            }

            return memberShipDetails;
        }
    }
}
