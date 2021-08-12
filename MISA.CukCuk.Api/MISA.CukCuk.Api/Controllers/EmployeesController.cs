using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Api.Model;
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
        /// <summary>
        /// Request lấy tất cả nhân viên
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetEmployees()
        {
            try
            {
                //1. Khai báo thông tin kết nối database
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = MISA.CukCuk_Demo_NVMANH;" +
                    "User Id = dev;" +
                    "Password = 12345678";

                //2. Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                //3. Lấy dữ liệu
                var sqlCommand = "SELECT * FROM Employee";
                var employees = dbConnection.Query<object>(sqlCommand);

                //4. Trả về cho client

                var response = StatusCode(200, employees);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resources.Exception_ErrorMsg,
                    errorCode = "",
                    moreInfo = "",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
            
        }

        /// <summary>
        /// Request lấy nhân viên theo Id (Khóa chính)
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("{employeeId}")]
        public IActionResult GetEmployeeById(Guid employeeId)
        {
            try
            {
                //1. Khai báo thông tin kết nối database
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = MISA.CukCuk_Demo_NVMANH;" +
                    "User Id = dev;" +
                    "Password = 12345678";

                //2. Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                //3. Lấy dữ liệu
                var sqlCommand = $"SELECT * FROM Employee WHERE EmployeeId = @EmployeeIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeIdParam", employeeId);

                var employee = dbConnection.QueryFirstOrDefault<object>(sqlCommand, param: parameters);

                //4. Trả về cho client

                var response = StatusCode(200, employee);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resources.Exception_ErrorMsg,
                    errorCode = "",
                    moreInfo = "",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
            
        }

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
                // Mã khách hàng bắt buộc phải có
                if (employee.EmployeeCode == "" || employee.EmployeeCode == null)
                {
                    var errorObj = new
                    {
                        devMsg = Properties.Resources.EmployeeCode_ErrorMsg,
                        userMsg = Properties.Resources.EmployeeCode_ErrorMsg,
                        errorCode = "",

                    };
                    return BadRequest(errorObj);
                }

                // Email phải đúng định dạng
                var emailValidate = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
                var isMatch = Regex.IsMatch(employee.Email, emailValidate, RegexOptions.IgnoreCase);
                if (isMatch == false)
                {
                    var errorObj = new
                    {
                        devMsg = Properties.Resources.EmployeeEmail_ErrorMsg,
                        userMsg = Properties.Resources.EmployeeEmail_ErrorMsg,
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
                var sqlCode = $"SELECT * FROM Employee WHERE EmployeeCode = @EmployeeCode";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@EmployeeCode", employee.EmployeeCode);
                //Truy vấn mã khách hàng trong database (kiểm tra trùng lặp)
                var check = dbConnection.Query<int>(sqlCode, dynamicParameters);
                if (check.Count() > 0)
                {
                    var errorObj = new
                    {
                        devMsg = Properties.Resources.CustomerCode_Duplicate_ErrorMsg,
                        userMsg = Properties.Resources.CustomerCode_Duplicate_ErrorMsg,
                        errorCode = "",
                        moreInfo = "",
                        traceId = ""
                    };
                    return BadRequest(errorObj);
                }

                //Sinh mã Guid cho EmployeeId
                employee.EmployeeId = Guid.NewGuid();

                //3. Thêm dữ liệu
                var columnName = string.Empty;
                var columnValue = string.Empty;

                //Khai bao dynamic parameters
                var dynamicParam = new DynamicParameters();

                //Đọc từng property của object
                var properties = employee.GetType().GetProperties();

                //Duyệt từng property
                foreach (var prop in properties)
                {
                    //Lấy tên của prop
                    var propName = prop.Name;

                    //Lấy value của prop
                    var propValue = prop.GetValue(employee);

                    //Thêm param tương ứng với mỗi property
                    dynamicParam.Add($"@{propName}", propValue);

                    columnName += $"{propName},";
                    columnValue += $"@{propName},";
                }

                //Xóa dấu phẩy cuối cùng trong các dãy string columnName, columnValue
                columnName = columnName.Remove(columnName.Length - 1, 1);
                columnValue = columnValue.Remove(columnValue.Length - 1, 1);

                var sqlCommand = $"INSERT INTO Employee({columnName}) VALUES({columnValue})";
                var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);

                //4. Trả về
                var response = StatusCode(201, rowEffects);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resources.Exception_ErrorMsg,
                    errorCode = "",
                    moreInfo = "",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
            
        }
    }
}
