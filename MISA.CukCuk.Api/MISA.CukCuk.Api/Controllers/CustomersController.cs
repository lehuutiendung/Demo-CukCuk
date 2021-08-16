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
    public class CustomersController : ControllerBase
    {
        ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        #region API lấy tất cả khách hàng

        /// <summary>
        /// Lấy tất cả khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCustomers()
        {
            try
            {
                var _serviceResult = _customerService.GetAll();
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

        #region API lấy thông tin của 1 khách hàng

        /// <summary>
        /// Lấy thông tin của 1 khách hàng
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("{customerId}")]
        public IActionResult GetCustomerById(Guid customerId)
        {
            try
            {
                var _serviceResult = _customerService.GetById(customerId);
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

        #region API phân trang và bộ lọc theo tên, mã khách hàng, số điện thoại

        /// <summary>
        /// Bộ lọc theo Tên, Mã khách hàng, Số điện thoại
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="customerCode"></param>
        /// <param name="phoneNumber"></param>
        /// <returns>
        ///     totalRecord
        ///     {customer}
        /// </returns>
        [HttpGet("Filter")]
        public IActionResult FilterCustomer(int pageSize, int pageNumber, String fullName, String customerCode, String phoneNumber)
        {
            try
            {
                var _serviceResult = _customerService.Filter(pageSize, pageNumber, fullName, customerCode, phoneNumber);
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

        #region API sinh mã khách hàng
        /// <summary>
        /// Sinh mã khách hàng mới
        /// </summary>
        /// <returns>Sinh mã khách hàng mới</returns>
        [HttpGet("NewCustomerCode")]
        
        public IActionResult NewCustomerCode()
        {
            try
            {
                var _serviceResult = _customerService.NewCode();
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

        #region API thêm mới khách hàng

        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddCustomer(Customer customer)
        {
            try
            {
                var serviceResult = _customerService.Add(customer);
                #region Code cũ
                // Validate các trường bắt buộc: CustomerCode, FullName, Email, PhoneNumber
                /* if (customer.CustomerCode == "" || customer.CustomerCode == null)
                 {
                     var errorObj = new
                     {
                         devMsg = Properties.ResourcesCustomer.CustomerCode_ErrorMsg,
                         userMsg = Properties.ResourcesCustomer.CustomerCode_ErrorMsg,
                         errorCode = "",
                     };
                     return BadRequest(errorObj);
                 }else if(customer.FullName == "" || customer.FullName == null){
                     var errorObj = new
                     {
                         devMsg = Properties.ResourcesCommon.FullName_ErrorMsg,
                         userMsg = Properties.ResourcesCommon.FullName_ErrorMsg,
                         errorCode = "",
                     };
                     return BadRequest(errorObj);
                 }
                 else if(customer.Email == "" || customer.Email == null){
                     var errorObj = new
                     {
                         devMsg = Properties.ResourcesCommon.Email_ErrorMsg,
                         userMsg = Properties.ResourcesCommon.Email_ErrorMsg,
                         errorCode = "",
                     };
                     return BadRequest(errorObj);
                 }
                 else if(customer.PhoneNumber == "" || customer.PhoneNumber == null)
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
                 var isMatch = Regex.IsMatch(customer.Email, emailValidate, RegexOptions.IgnoreCase);
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

                 // Mã khách hàng không được trùng
                 var sqlCode = $"SELECT * FROM Customer WHERE CustomerCode = @CustomerCode";
                 DynamicParameters dynamicParameters = new DynamicParameters();
                 dynamicParameters.Add("@CustomerCode", customer.CustomerCode);
                 //Truy vấn mã khách hàng trong database (kiểm tra trùng lặp)
                 var check = dbConnection.Query<int>(sqlCode, dynamicParameters);
                 if(check.Count() > 0)
                 {
                     var errorObj = new
                     {
                         devMsg = Properties.ResourcesCustomer.CustomerCode_Duplicate_ErrorMsg,
                         userMsg = Properties.ResourcesCustomer.CustomerCode_Duplicate_ErrorMsg,
                         errorCode = "",
                         moreInfo = "",
                         traceId = ""
                     };
                     return BadRequest(errorObj);
                 }

                 // Sinh mã Guid cho EmployeeId
                 customer.CustomerId = Guid.NewGuid();

                 // Thêm dữ liệu
                 var columnName = string.Empty;
                 var columnValue = string.Empty;

                 //Khai bao dynamic parameters
                 var dynamicParam = new DynamicParameters();

                 //Đọc từng property của object
                 var properties = customer.GetType().GetProperties();

                 //Duyệt từng property
                 foreach (var item in properties)
                 {
                     //Lấy tên của prop
                     var itemName = item.Name;

                     //Lấy value của prop
                     var itemValue = item.GetValue(customer);

                     //Thêm param tương ứng với mỗi property
                     dynamicParam.Add($"@{itemName}", itemValue);

                     columnName += $"{itemName},";
                     columnValue += $"@{itemName},";
                 }

                 //Xóa dấu phẩy cuối cùng trong các dãy string columnName, columnValue
                 columnName = columnName.Remove(columnName.Length - 1, 1);
                 columnValue = columnValue.Remove(columnValue.Length - 1, 1);

                 var sqlCommand = $"INSERT INTO Customer({columnName}) VALUES({columnValue})";
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

        #region API xóa một khách hàng

        /// <summary>
        /// Xóa một khách hàng
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpDelete("{customerId}")]
        public IActionResult DeleteCustomer(Guid customerId)
        {
            try
            {
                var _serviceResult = _customerService.Delete(customerId);

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

        #region API sửa thông tin khách hàng

        /// <summary>
        /// Sửa bản ghi khách hàng
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPut("{customerId}")]
        public IActionResult EditCustomer(Guid customerId, Customer customer)
        {
            try
            {
                var _serviceResult = _customerService.Update(customerId, customer);
                //4. Trả về
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
