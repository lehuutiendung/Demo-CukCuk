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
    public class DepartmentService:BaseFacilityService<Department>, IDepartmentService
    {
        ServiceResult _serviceResult;
        public DepartmentService(IBaseFacilityRepository<Department> baseFacilityRepository):base(baseFacilityRepository)
        {
            _serviceResult = new ServiceResult();
        }
    }
}
