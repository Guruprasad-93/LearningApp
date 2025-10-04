using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities;
using gym_app.Domain.Interfaces.RepositoryInterface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gym_app.Domain.Model.CustomerReview;
using gym_app.Domain.Model.User;
using gym_app.Domain.Model.CommonModel;
using Microsoft.EntityFrameworkCore;

namespace gym_app.Infrastructure.Repository.ReviewRepository
{
    public class CustomerReviewRepository: BaseRepository<CustomerReviewRepository>, IReviewRepository
    {
        private readonly ApplicationDbContext context;
        public AppOptions appOptions { get; set; }
        public CustomerReviewRepository(ApplicationDbContext _context, ILogger<CustomerReviewRepository> _logger, AppOptions _appOptions) : base(_logger)
        {
            context = _context;
            appOptions = _appOptions;

        }

        public string CustomerReviewCreationUpdation(ReviewModel reviewModel)
        {
            string Result = "";

            try
            {
                logger.LogDebug("CustomerReviewRepository > CustomerReviewCreationUpdation() started {model}", reviewModel);

                if (context.Ratings.Any(r => r.UserId == reviewModel.UserId && r.CompanyId == reviewModel.CompanyId))
                {
                    var rating = context.Ratings.FirstOrDefault(r => r.UserId == reviewModel.UserId && r.CompanyId == reviewModel.CompanyId);

                    if(rating != null)
                    {
                        rating.UserId = reviewModel.UserId;
                        rating.Stars = reviewModel.Stars;
                        rating.Review = reviewModel.Review;
                        rating.ModifiedBy = reviewModel.UserId;
                        rating.ModifiedDate = DateTime.Now;
                        rating.IsDeleted = false;
                        rating.CompanyId = reviewModel.CompanyId;
                        context.Ratings.Update(rating);
                        context.SaveChanges();
                    } 
                }
                else
                {
                    Rating rating = new Rating();
                    rating.UserId = reviewModel.UserId;
                    rating.Stars = reviewModel.Stars;
                    rating.Review = reviewModel.Review;
                    rating.CreatedDate = DateTime.Now;
                    rating.CreatedBy = reviewModel.UserId;
                    rating.ModifiedBy = reviewModel.UserId;
                    rating.ModifiedDate = DateTime.Now;
                    rating.IsDeleted = false;
                    rating.CompanyId = reviewModel.CompanyId;
                    context.Ratings.Add(rating);
                    context.SaveChanges();

                }
                Result = "S001";

                logger.LogDebug("CustomerReviewRepository > CustomerReviewCreationUpdation() Completed {model}", reviewModel);

            }
            catch (Exception ex)
            {
                Result = "E001";
                logger.LogError(ex, "Error in CustomerReviewRepository > CustomerReviewCreationUpdation() {model}", reviewModel);
                throw;
            }

            return Result;
        }

        public async Task<List<ReviewDetailsModel>> GetCustomerReview(ReviewModelClass reviewModelClass)
        {
            List<ReviewDetailsModel> ltReviewDetails = new List<ReviewDetailsModel>();

            try
            {
                logger.LogDebug("CustomerReviewRepository > GetCustomerReview() started {model}", reviewModelClass);

                ltReviewDetails = await (from r in context.Ratings
                                   join u in context.Users on r.UserId equals u.UserId
                                   join p in context.UserPhotos on u.UserId equals p.UserId
                                   where r.CompanyId == reviewModelClass.CompanyId && !r.IsDeleted && !u.IsDeleted && !p.IsDeleted && u.IsActive
                                   select (new ReviewDetailsModel
                                   {
                                       UserName = u.FullName,
                                       Stars = r.Stars,
                                       Review = r.Review,
                                       CreatedDate = r.ModifiedDate,
                                       PhotoPath = p.PhotoPath

                                   })).ToListAsync();

                

                logger.LogDebug("CustomerReviewRepository > GetCustomerReview() Completed {model}", reviewModelClass);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CustomerReviewRepository > GetCustomerReview() {model}", reviewModelClass);
                throw;
            }

            return ltReviewDetails;
        }
    }
}
