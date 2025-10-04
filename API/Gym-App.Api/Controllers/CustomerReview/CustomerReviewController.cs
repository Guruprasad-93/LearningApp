using Gym_App.Api.Controllers.User;
using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities.Security;
using gym_app.Domain.Interfaces.BusinessInterface;
using Microsoft.AspNetCore.Mvc;
using gym_app.Domain.Model.User;
using gym_app.Domain.Model.CustomerReview;
using gym_app.Domain.Model.CommonModel;
using gym_app.Domain.Interfaces.RepositoryInterface;


namespace Gym_App.Api.Controllers.Review
{
    public class CustomerReviewController : BaseApiController<CustomerReviewController>
    {
        private readonly JwtOptions JwtOptions;
        private readonly IReviewService reviewService;
        public CustomerReviewController(IReviewService _reviewService, ILogger<CustomerReviewController> _logger, AppOptions appOptions) : base(appOptions, _logger)
        {
            reviewService = _reviewService;
            JwtOptions = appOptions.JwtOptions;
        }

        [HttpPost]
        [Route("reviewcreation")]
        public async Task<IActionResult> CustomerReviewCreationUpdation([FromBody] ReviewModel reviewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cid = GetCurrentCompanyId();

            string Result = reviewService.CustomerReviewCreationUpdation(reviewModel);

            return Ok("Customer saved securely.");
        }

        [HttpGet]
        [Route("reviewdetails")]
        public async Task<List<ReviewDetailsModel>> GetCustomerReview()
        {
            List<ReviewDetailsModel> ltReviewDetails = new List<ReviewDetailsModel>();
            ReviewModelClass reviewModelClass = new ReviewModelClass();

            try
            {
                logger.LogDebug("CustomerReviewController > GetCustomerReview() started {model}", reviewModelClass);

                long cid = GetCurrentCompanyId();

                ltReviewDetails = await reviewService.GetCustomerReview(reviewModelClass);

                logger.LogDebug("CustomerReviewController > GetCustomerReview() completed {model}", reviewModelClass);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in CustomerReviewController > GetCustomerReview() {model}", reviewModelClass);
                throw;
            }

            return ltReviewDetails;
        }
    }
}
