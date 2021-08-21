using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    public class BaseFacilityController<FacilityEntity> : ControllerBase
    {
        IBaseFacilityService<FacilityEntity> _baseFacilityService;
        public BaseFacilityController(IBaseFacilityService<FacilityEntity> baseFacilityService)
        {
            _baseFacilityService = baseFacilityService;
        }

        #region API lấy tất cả bản ghi

        /// <summary>
        /// API lấy tất cả bản ghi
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var _serviceResult = _baseFacilityService.GetAll();
                //Trả về cho client
                if (_serviceResult.IsValid)
                {
                    var response = StatusCode(200, _serviceResult.Data);
                    return response;
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = MISA.ApplicationCore.Resources.ResourcesCommon.Exception_ErrorMsg,
                    errorCode = "",
                    moreInfo = "",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
        }
        #endregion

        #region API lấy thông tin chi tiết theo id

        /// <summary>
        /// API lấy thông tin chi tiết 
        /// </summary>
        /// <param name="facilityEntityId"></param>
        /// <returns></returns>
        [HttpGet("{facilityEntityId}")]
        public IActionResult GetById(Guid facilityEntityId)
        {
            try
            {
                var _serviceResult = _baseFacilityService.GetById(facilityEntityId);
                //Trả về cho client
                var response = StatusCode(200, _serviceResult.Data);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = MISA.ApplicationCore.Resources.ResourcesCommon.Exception_ErrorMsg,
                    errorCode = "",
                    moreInfo = "",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
        }
        #endregion
    }
}
