using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities.Security;
using gym_app.Domain.Interfaces.BusinessInterface;
using gym_app.Domain.Interfaces.RepositoryInterface;
using gym_app.Domain.Model.CommonModel;
using gym_app.Domain.Model.CustomerReview;
using gym_app.Domain.Model.User;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Business.ReviewService
{
    public class CustomerReviewService:BaseService<CustomerReviewService>, IReviewService
    {
        private readonly IReviewRepository reviewRepository;
        private readonly JwtOptions JwtOptions;

        public CustomerReviewService(IReviewRepository _reviewRepository, ILogger<CustomerReviewService> _logger, AppOptions appOptions) : base(appOptions, _logger)
        {
            reviewRepository = _reviewRepository;
            JwtOptions = appOptions.JwtOptions;
        }

        public string CustomerReviewCreationUpdation(ReviewModel reviewModel)
        {
            string Result = "";
            try
            {
                logger.LogDebug("CustomerReviewService > CustomerReviewCreationUpdation() started {model}", reviewModel);

                Result = reviewRepository.CustomerReviewCreationUpdation(reviewModel);

                logger.LogDebug("CustomerReviewService > CustomerReviewCreationUpdation() completed {model}", reviewModel);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CustomerReviewService > CustomerReviewCreationUpdation() {model}", reviewModel);
                throw;
            }

            return Result;
        }

        public async Task<List<ReviewDetailsModel>> GetCustomerReview(ReviewModelClass reviewModelClass)
        {
            List<ReviewDetailsModel> ltReviewDetails = new List<ReviewDetailsModel>();
            try
            {
                logger.LogDebug("CustomerReviewService > GetCustomerReview() started {model}", reviewModelClass);

                ltReviewDetails = await reviewRepository.GetCustomerReview(reviewModelClass);

                logger.LogDebug("CustomerReviewService > GetCustomerReview() completed {model}", reviewModelClass);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CustomerReviewService > GetCustomerReview() {model}", reviewModelClass);
                throw;
            }

            return ltReviewDetails;
        }
    }
}
