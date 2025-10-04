using gym_app.Domain.Model.CommonModel;
using gym_app.Domain.Model.CustomerReview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Interfaces.BusinessInterface
{
    public interface IReviewService
    {
        string CustomerReviewCreationUpdation(ReviewModel reviewModel);
        Task<List<ReviewDetailsModel>> GetCustomerReview(ReviewModelClass reviewModelClass);
    }
}
