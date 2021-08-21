using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Services
{
    public interface IBaseFacilityService<FacilityEntity>
    {
        /// <summary>
        /// Lấy tất cả (theo phòng ban hoặc vị trí)
        /// </summary>
        /// <returns></returns>
        ServiceResult GetAll();

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="facilityEntityId"></param>
        /// <returns></returns>
        ServiceResult GetById(Guid facilityEntityId);
    }
}
