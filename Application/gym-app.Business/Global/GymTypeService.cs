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
    public class GymTypeService : BaseService<GymTypeService>, IGymTypeService
    {
        private readonly IGymTypeRepository gymTypeRepository;
        private readonly JwtOptions JwtOptions;
        public GymTypeService(IGymTypeRepository _gymTypeRepository,AppOptions _appOptions, ILogger<GymTypeService> _logger) : base(_appOptions, _logger)
        {
            gymTypeRepository = _gymTypeRepository;
            JwtOptions = _appOptions.JwtOptions;
        }

        public async Task<List<GymType>> GetGymTypeDetails()
        {
            List<GymType> gymType = new List<GymType>();
            try
            {
                logger.LogDebug("GymTypeService > GetGymTypeDetails() started");

                gymType = await gymTypeRepository.GetGymTypeDetails();

                logger.LogDebug("GymTypeService > GetGymTypeDetails() completed");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GymTypeService > GetGymTypeDetails()");
                throw;
            }

            return gymType;
        }

        public async Task<string> GymTypeCreation(GymType gymType)
        {
            string Result = "";
            try
            {
                logger.LogDebug("GymTypeService > GymTypeCreation() started {model}", gymType);

                Result = await gymTypeRepository.GymTypeCreation(gymType);

                logger.LogDebug("GymTypeService > GymTypeCreation() completed {model}", gymType);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GymTypeService > GymTypeCreation() {model}", gymType);
                throw;
            }

            return Result;
        }
    }
}
