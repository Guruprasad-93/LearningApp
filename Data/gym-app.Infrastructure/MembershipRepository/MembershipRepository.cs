using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities;
using gym_app.Domain.Interfaces.RepositoryInterface;
using gym_app.Infrastructure.Repository.ReviewRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gym_app.Domain.Model.MembershipModel;
using gym_app.Domain.Model.CommonModel;
using Microsoft.EntityFrameworkCore;

namespace gym_app.Infrastructure.MembershipRepository
{
    public class MembershipRepository:BaseRepository<MembershipRepository>, IMembershipRepository
    {
        private readonly ApplicationDbContext context;
        public AppOptions appOptions { get; set; }
        public MembershipRepository(ApplicationDbContext _context, ILogger<MembershipRepository> _logger, AppOptions _appOptions) : base(_logger)
        {
            context = _context;
            appOptions = _appOptions;

        }

        public async Task<string> MemberShipCreation(MembershipModel membershipModel)
        {
            string Status = "E001";

            try
            {
                logger.LogDebug("MembershipRepository > MemberShipCreation() started {model}", membershipModel);

                if(membershipModel != null)
                {
                    Membership membership = new Membership();

                    membership.UserId = membershipModel.UserId;
                    membership.Duration = membershipModel.Duration;
                    membership.StartDate = membershipModel.StartDate;
                    membership.EndDate = membershipModel.EndDate;
                    membership.TrainerId = membershipModel.TrainerId;
                    membership.CompanyId = membershipModel.CompanyId;
                    membership.IsActive = true;
                    membership.IsDeleted = false;
                    membership.Type = membershipModel.Type;
                    membership.CreatedBy = membershipModel.CreatedBy;
                    membership.CreatedDate = membershipModel.CreatedDate;
                    context.Memberships.Add(membership);
                    await context.SaveChangesAsync();
                    Status = "S001";
                }
                else
                {
                    Status = "INVALID";
                }


                    logger.LogDebug("MembershipRepository > MemberShipCreation() completed {model}", membershipModel);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in MembershipRepository > MemberShipCreation() {model}", membershipModel);
                throw;
            }

            return Status;

        }

        public async Task<List<MemberShipDetails>> MemberShipDetails(CommonModelClass commonModelClass)
        {
            List<MemberShipDetails> memberShipDetails = new List<MemberShipDetails>();

            try
            {
                logger.LogDebug("MembershipRepository > MemberShipDetails() started {model}", commonModelClass);


                memberShipDetails = await (from ms in context.Memberships
                                           join u in context.Users on ms.UserId equals u.UserId
                                           join up in context.UserPhotos on u.UserId equals up.UserId
                                           join gt in context.GymTypes on ms.Type equals gt.TypeId
                                           where ms.CompanyId == commonModelClass.CompanyId && ms.IsActive == true && !ms.IsDeleted &&
                                           !u.IsDeleted && !up.IsDeleted && !gt.IsDeleted
                                           select new MemberShipDetails
                                           {
                                               UserName = u.FullName,
                                               Type = gt.TypeName,
                                               PhotoPath = up.PhotoPath,
                                               StartDate = ms.StartDate,
                                               EndDate = ms.EndDate,
                                               Duration = ms.Duration

                                           }).ToListAsync();


                logger.LogDebug("MembershipRepository > MemberShipDetails() completed {model}", commonModelClass);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in MembershipRepository > MemberShipDetails() {model}", commonModelClass);
                throw;
            }

            return memberShipDetails;

        }
    }
}
