using gym_app.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_app.Domain.Interfaces.BusinessInterface
{
    public interface IGymTypeService
    {
        Task<List<GymType>> GetGymTypeDetails();
        Task<string> GymTypeCreation(GymType gymType);
    }
}
