using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Model.CustomerReview
{
    public class ReviewModel
    {
        public long? UserId { get; set; }

        public int? Stars { get; set; }

        public string? Review { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public long CompanyId { get; set; }
    }

    public class ReviewDetailsModel
    {
        public string? PhotoPath { get; set; }
        public string? UserName { get; set; }
        public int? Stars { get; set; }
        public string? Review { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
