using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Model.CommonModel
{
    public class CommonModelClass
    {
        public long UserId { get; set; }
        public long MembershipId { get; set; }
        public long CompanyId { get; set; }
        
    }

    public class ReviewModelClass
    {
        public long? UserId { get; set; }
        public long? CompanyId { get; set; }

    }

}
