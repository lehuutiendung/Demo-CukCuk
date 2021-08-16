using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Services;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]

    public class EmployeesController : ControllerBase
    {

        IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        #region API lấy tất cả nhân viên

        /// <summary>
        /// Lấy tất cả nhân viên
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetEmployees()
        {
            try
            {
                var _serviceResult = _employeeService.GetAll();
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

        #region API lấy thông tin của 1 nhân viên

        /// <summary>
        /// Lấy thông tin của 1 nhân viên
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("{employeeId}")]
        public IActionResult GetEmployeeById(Guid employeeId)
        {
            try
            {
                var _serviceResult = _employeeService.GetById(employeeId);
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
        /// Bộ lọc theo Tên, Mã nhân viên, Số điện thoại
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="employeeCode"></param>
        /// <param name="phoneNumber"></param>
        /// <returns>
        ///     totalRecord
        ///     {employee}
        /// </returns>
        [HttpGet("Filter")]
        public IActionResult FilterEmployee(int pageSize, int pageNumber, String fullName, String employeeCode, String phoneNumber)
        {
            try
            {
                var _serviceResult = _employeeService.Filter(pageSize, pageNumber, fullName, employeeCode, phoneNumber);
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

        #region API sinh mã nhân viên
        /// <summary>
        /// Sinh mã nhân viên mới
        /// </summary>
        /// <returns>Sinh mã nhân viên mới</returns>
        [HttpGet("NewEmployeeCode")]

        public IActionResult NewEmployeeCode()
        {
            try
            {
                var _serviceResult = _employeeService.NewCode();
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

        #region API thêm mới nhân viên

        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            try
            {
                var serviceResult = _employeeService.Add(employee);
                #region Code cũ
                // Validate các trường bắt buộc: EmployeeCode, FullName, Email, PhoneNumber
                /* if (employee.EmployeeCode == "" || employee.EmployeeCode == null)
                 {
                     var errorObj = new
                     {
                         devMsg = Properties.ResourcesEmployee.EmployeeCode_ErrorMsg,
                         userMsg = Properties.ResourcesEmployee.EmployeeCode_ErrorMsg,
                         errorCode = "",
                     };
                     return BadRequest(errorObj);
                 }else if(employee.FullName == "" || employee.FullName == null){
                     var errorObj = new
                     {
                         devMsg = Properties.ResourcesCommon.FullName_ErrorMsg,
                         userMsg = Properties.ResourcesCommon.FullName_ErrorMsg,
                         errorCode = "",
                     };
                     return BadRequest(errorObj);
                 }
                 else if(employee.Email == "" || employee.Email == null){
                     var errorObj = new
                     {
                         devMsg = Properties.ResourcesCommon.Email_ErrorMsg,
                         userMsg = Properties.ResourcesCommon.Email_ErrorMsg,
                         errorCode = "",
                     };
                     return BadRequest(errorObj);
                 }
                 else if(employee.PhoneNumber == "" || employee.PhoneNumber == null)
                 {
                     var errorObj = new
                     {
                         devMsg = Properties.ResourcesCommon.PhoneNumber_ErrorMsg,
                         userMsg = Properties.ResourcesCommon.PhoneNumber_ErrorMsg,
                         errorCode = "",
                     };
                     return BadRequest(errorObj);
                 }

                 // Email phải đúng định dạng
                 var emailValidate = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
                 var isMatch = Regex.IsMatch(employee.Email, emailValidate, RegexOptions.IgnoreCase);
                 if(isMatch == false)
                 {
                     var errorObj = new
                     {
                         devMsg = Properties.ResourcesCommon.ValidateEmail_ErrorMsg,
                         userMsg = Properties.ResourcesCommon.ValidateEmail_ErrorMsg,
                         errorCode = "",
                         moreInfo = "",
                         traceId = ""
                     };
                     return BadRequest(errorObj);
                 }

                 // Khai báo thông tin kết nối database
                 var connectionString = "Host = 47.241.69.179;" +
                     "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
                     "User Id = dev;" +
                     "Password = 12345678";

                 // Khởi tạo đối tượng kết nối với database
                 IDbConnection dbConnection = new MySqlConnection(connectionString);

                 // Mã nhân viên không được trùng
                 var sqlCode = $"SELECT * FROM Employee WHERE EmployeeCode = @EmployeeCode";
                 DynamicParameters dynamicParameters = new DynamicParameters();
                 dynamicParameters.Add("@EmployeeCode", employee.EmployeeCode);
                 //Truy vấn mã nhân viên trong database (kiểm tra trùng lặp)
                 var check = dbConnection.Query<int>(sqlCode, dynamicParameters);
                 if(check.Count() > 0)
                 {
                     var errorObj = new
                     {
                         devMsg = Properties.ResourcesEmployee.EmployeeCode_Duplicate_ErrorMsg,
                         userMsg = Properties.ResourcesEmployee.EmployeeCode_Duplicate_ErrorMsg,
                         errorCode = "",
                         moreInfo = "",
                         traceId = ""
                     };
                     return BadRequest(errorObj);
                 }

                 // Sinh mã Guid cho EmployeeId
                 employee.EmployeeId = Guid.NewGuid();

                 // Thêm dữ liệu
                 var columnName = string.Empty;
                 var columnValue = string.Empty;

                 //Khai bao dynamic parameters
                 var dynamicParam = new DynamicParameters();

                 //Đọc từng property của object
                 var properties = employee.GetType().GetProperties();

                 //Duyệt từng property
                 foreach (var item in properties)
                 {
                     //Lấy tên của prop
                     var itemName = item.Name;

                     //Lấy value của prop
                     var itemValue = item.GetValue(employee);

                     //Thêm param tương ứng với mỗi property
                     dynamicParam.Add($"@{itemName}", itemValue);

                     columnName += $"{itemName},";
                     columnValue += $"@{itemName},";
                 }

                 //Xóa dấu phẩy cuối cùng trong các dãy string columnName, columnValue
                 columnName = columnName.Remove(columnName.Length - 1, 1);
                 columnValue = columnValue.Remove(columnValue.Length - 1, 1);

                 var sqlCommand = $"INSERT INTO Employee({columnName}) VALUES({columnValue})";
                 var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);*/
                #endregion

                if (serviceResult.IsValid == true)
                {
                    return StatusCode(201, serviceResult.Data); ;
                }
                else
                {
                    return BadRequest(serviceResult.Data);
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

        #region API xóa một nhân viên

        /// <summary>
        /// Xóa một nhân viên
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpDelete("{employeeId}")]
        public IActionResult DeleteEmployee(Guid employeeId)
        {
            try
            {
                var _serviceResult = _employeeService.Delete(employeeId);

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
        [HttpPut("{employeeId}")]
        public IActionResult EditEmployee(Guid employeeId, Employee employee)
        {
            try
            {
                var _serviceResult = _employeeService.Update(employeeId, employee);
                if(_serviceResult.IsValid == true)
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
