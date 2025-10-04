using gym_app.Domain.Entities;
using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Interfaces.RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace gym_app.Infrastructure.Repository.GlobalRepository
{
    public class GymTypeRepository : BaseRepository<GymTypeRepository>, IGymTypeRepository
    {
        private readonly ApplicationDbContext context;
        public AppOptions appOptions { get; set; }
        public GymTypeRepository(ApplicationDbContext _context, ILogger<GymTypeRepository> _logger, AppOptions _appOptions) : base(_logger)
        {
            context = _context;
            appOptions = _appOptions;
        }

        public async Task<List<GymType>> GetGymTypeDetails()
        {
            List<GymType> GymType = new List<GymType>();
            try
            {
                logger.LogDebug("GymTypeRepository > GetGymTypeDetails() started");

                GymType = await context.GymTypes.ToListAsync();

                logger.LogDebug("GymTypeRepository > GetGymTypeDetails() completed");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GymTypeRepository > GetGymTypeDetails()");
                throw;
            }

            return GymType;
        }

        public async Task<string> GymTypeCreation(GymType gymType)
        {
            string Result = "E001";
            try
            {
                logger.LogDebug("GymTypeRepository > GymTypeCreation() started {model}", gymType);

                if (context.GymTypes.Any(g => g.TypeId == gymType.TypeId))
                {
                    var type = context.GymTypes.Where(g => g.TypeId == gymType.TypeId).FirstOrDefault();
                    if (type != null)
                    {
                        type.TypeName = gymType.TypeName;
                        type.ModifiedBy = gymType.ModifiedBy;
                        type.ModifiedDate = DateTime.Now;

                        context.Update(type);
                        await context.SaveChangesAsync();
                        Result = "U001";
                    }

                }
                if (gymType.TypeId == 0)
                {
                    gymType.CreatedDate = DateTime.Now;
                    gymType.CreatedBy = gymType.CreatedBy;
                    context.GymTypes.Add(gymType);
                    await context.SaveChangesAsync();
                    Result = "S001";
                }

                logger.LogDebug("GymTypeRepository > GymTypeCreation() completed {model}", gymType);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GymTypeRepository > GymTypeCreation() {model}", gymType);
                throw;
            }

            return Result;
        }
    }
}
