using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repository;
using MISA.ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Services
{
    public class PositionService : BaseFacilityService<Position>, IPositionService
    {
        ServiceResult _serviceResult;
        public PositionService(IBaseFacilityRepository<Position> baseFacilityRepository) : base(baseFacilityRepository)
        {
            _serviceResult = new ServiceResult();
        }
    }
}
