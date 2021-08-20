using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    public class BaseController<Entity> : ControllerBase
    {
        IBaseService<Entity> _baseService;
        public BaseController(IBaseService<Entity> baseService)
        {
            _baseService = baseService;
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
                var _serviceResult = _baseService.GetAll();
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

        #region API lấy thông tin chi tiết 1 bản ghi

        /// <summary>
        /// API lấy thông tin chi tiết 1 bản ghi
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpGet("{entityId}")]
        public IActionResult GetById(Guid entityId)
        {
            try
            {
                var _serviceResult = _baseService.GetById(entityId);
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

        #region API phân trang và bộ lọc theo tên, mã nhân viên, số điện thoại

        /// <summary>
        /// Bộ lọc theo Tên, Mã, Số điện thoại
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="code">Mã nhân viên/Mã khách hàng</param>
        /// <param name="phoneNumber"></param>
        /// <returns>
        ///     totalRecord
        ///     {entity}
        /// </returns>
        [HttpGet("Filter")]
        public IActionResult Filter(int pageSize, int pageNumber, String filter)
        {
            try
            {
                var _serviceResult = _baseService.Filter(pageSize, pageNumber, filter);
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

        #region API sinh mã 
        /// <summary>
        /// Sinh mã mới
        /// </summary>
        /// <returns>Sinh mã mới</returns>
        [HttpGet("NewCode")]

        public IActionResult NewCode()
        {
            try
            {
                var _serviceResult = _baseService.NewCode();
                //Trả về cho client
                if (_serviceResult.Data != null)
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

        #region API thêm mới bản ghi

        /// <summary>
        /// Thêm mới 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual IActionResult Add(Entity entity)
        {
            try
            {
                var serviceResult = _baseService.Add(entity);
                if (serviceResult.IsValid == true)
                {
                    if ((int)serviceResult.Data == 400)
                    {
                        var errorObj = new
                        {
                            devMsg = MISA.ApplicationCore.Resources.ResourcesCommon.Duplicate_ErrorMsg.Replace("{}", ""),
                            userMsg = MISA.ApplicationCore.Resources.ResourcesCommon.Duplicate_ErrorMsg.Replace("{}", ""),
                            errorCode = "",
                            moreInfo = "",
                            traceId = ""
                        };
                        return StatusCode(400, errorObj);
                    }
                    return StatusCode(201, serviceResult.Data); ;
                }
                else
                {
                    var errorObj = new
                    {
                        devMsg = serviceResult.Data,
                        userMsg = serviceResult.Data,
                        errorCode = "",
                        moreInfo = "",
                        traceId = ""
                    };
                    return BadRequest(errorObj);
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

        #region API xóa 1 bản ghi

        /// <summary>
        /// API xóa 1 bản ghi
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpDelete("{entityId}")]
        public IActionResult Delete(Guid entityId)
        {
            try
            {
                var _serviceResult = _baseService.Delete(entityId);

                // Trả về cho client
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

        #region API xóa nhiều bản ghi
        [HttpDelete]
        public IActionResult DeleteMultiple(List<Guid> listId)
        {
            try
            {
                var _serviceResult = _baseService.DeleteMultiple(listId);

                // Trả về cho client
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

        #region API sửa thông tin nhân viên

        /// <summary>
        /// Sửa bản ghi nhân viên
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("{entityId}")]
        public IActionResult Edit(Guid entityId, Entity entity)
        {
            try
            {
                var _serviceResult = _baseService.Update(entityId, entity);
                if (_serviceResult.IsValid == true)
                {
                    //4. Trả về
                    var response = StatusCode(200, _serviceResult.Data);
                    return response;
                }
                else
                {
                    var errorObj = new
                    {
                        devMsg = _serviceResult.Data,
                        userMsg = _serviceResult.Data,
                        errorCode = "",
                        moreInfo = "",
                        traceId = ""
                    };
                    return BadRequest(errorObj);
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
    }
}
