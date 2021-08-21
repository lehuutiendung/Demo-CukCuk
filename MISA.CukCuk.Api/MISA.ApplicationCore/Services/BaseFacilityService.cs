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
    public class BaseFacilityService<FacilityEntity> : IBaseFacilityService<FacilityEntity>
    {
        IBaseFacilityRepository<FacilityEntity> _baseFacilityRepository;
        ServiceResult _serviceResult;
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="entityRepository"></param>
        public BaseFacilityService(IBaseFacilityRepository<FacilityEntity> baseFacilityRepository)
        {
            _baseFacilityRepository = baseFacilityRepository;
            _serviceResult = new ServiceResult();
        }

        public ServiceResult GetAll()
        {
            //Xử lý kết quả trả về sau khi tương tác Database
            _serviceResult.Data = _baseFacilityRepository.GetAll();
            return _serviceResult;
        }

        public ServiceResult GetById(Guid facilityEntityId)
        {
            //Xử lý kết quả trả về sau khi tương tác Database
            _serviceResult.Data = _baseFacilityRepository.GetById(facilityEntityId);
            return _serviceResult;
        }
    }
}
