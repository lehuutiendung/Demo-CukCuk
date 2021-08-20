using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Services;
using MySqlConnector;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]

    public class EmployeesController : BaseController<Employee>
    {

        IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService, IBaseService<Employee> baseService) : base(baseService)
        {
            _employeeService = employeeService;
        }

        #region Ghi đè API thêm mới để thay đổi Resouces Msg

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Add(Employee employee)
        {
            try
            {
                var serviceResult = _employeeService.Add(employee);
                if (serviceResult.IsValid == true)
                {
                    if ((int)serviceResult.Data == 400)
                    {
                        var errorObj = new
                        {
                            devMsg = MISA.ApplicationCore.Resources.ResourcesCommon.Duplicate_ErrorMsg.Replace("{}", "nhân viên"),
                            userMsg = MISA.ApplicationCore.Resources.ResourcesCommon.Duplicate_ErrorMsg.Replace("{}", "nhân viên"),
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
    }
}
