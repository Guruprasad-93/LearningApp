using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Model.MembershipModel
{
    public class MembershipModel
    {
        public long MembershipId { get; set; }

        public long? UserId { get; set; }

        public int Duration { get; set; }

        public int Type { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public long CompanyId { get; set; }

        public long? TrainerId { get; set; }
    }

    public class MemberShipDetails
    {
        public long MembershipId { get; set; }
        public int Duration { get; set; }
        public string? UserName { get; set; }
        public string? PhotoPath { get; set; }
        public string? Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
