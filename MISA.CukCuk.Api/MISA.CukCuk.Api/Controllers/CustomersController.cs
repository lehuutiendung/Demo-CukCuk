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
    public class CustomersController : ControllerBase
    {
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
                //1. Khai báo thông tin kết nối database
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
                    "User Id = dev;" +
                    "Password = 12345678";

                //2. Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                //3. Lấy dữ liệu
                var sqlCommand = "SELECT * FROM Customer";
                var customers = dbConnection.Query<object>(sqlCommand);

                //4. Trả về cho client
                if(customers.Count() > 0)
                {
                    var response = StatusCode(200, customers);
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
                    userMsg = Properties.ResourcesCommon.Exception_ErrorMsg,
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
                //Connect toi Database
                var connectionString = "Host = 47.241.69.179;" +
                "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

                IDbConnection dbConnection = new MySqlConnection(connectionString);

                //Lấy dữ liệu
                var sqlCommand = $"SELECT * FROM Customer WHERE CustomerId = @CustomerIdParam";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@CustomerIdParam", customerId);

                var customer = dbConnection.QueryFirstOrDefault<object>(sqlCommand, dynamicParameters);

                //4. Trả về cho client
                var response = StatusCode(200, customer);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourcesCommon.Exception_ErrorMsg,
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
                //1. Khai báo thông tin kết nối database
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
                    "User Id = dev;" +
                    "Password = 12345678";

                //2. Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                DynamicParameters dynamicParameters = new DynamicParameters();

                //Tính toán giá trị Offset
                int offSet = pageSize * pageNumber - pageSize;

                //Câu lệnh truy vấn phân trang và filter theo các tiêu chí
                var sqlCommand = $"SELECT * FROM (SELECT * FROM Customer LIMIT {pageSize} OFFSET {offSet}) paginate WHERE ( FullName LIKE @FullName AND CustomerCode LIKE @CustomerCode AND PhoneNumber LIKE @PhoneNumber )";
                
                dynamicParameters.Add($"@FullName", $"%{fullName}%");
                dynamicParameters.Add($"@CustomerCode", $"%{customerCode}%");
                dynamicParameters.Add($"@PhoneNumber", $"%{phoneNumber}%");

                var sqltotalRecord = $"SELECT COUNT(CustomerCode) AS totalRecord FROM Customer";

                var customer = dbConnection.Query<object>(sqlCommand, dynamicParameters);
                var totalRecord = dbConnection.QueryFirstOrDefault<int>(sqltotalRecord);
                var totalPage = (totalRecord / pageSize) + 1;

                var response = StatusCode(200, new { TotalPage = totalPage, TotalRecord = totalRecord, Data = customer });
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourcesCommon.Exception_ErrorMsg,
                    errorCode = "",
                    moreInfo = "",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
            
        }
        #endregion

        #region API sinh mã khách hàng

        [HttpGet("NewCustomerCode")]
        public IActionResult NewCustomerCode()
        {
            try
            {
                //1. Khai báo thông tin kết nối database
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
                    "User Id = dev;" +
                    "Password = 12345678";

                //2. Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                //3. Lấy dữ liệu
                var sqlCommand = "SELECT CAST(SUBSTRING(CustomerCode, 3, LENGTH(CustomerCode)-1) AS int) AS c FROM Customer ORDER BY c DESC LIMIT 1";
                var customerCode = dbConnection.QueryFirstOrDefault<int>(sqlCommand);

                //4. Trả về cho client
                if (customerCode != null)
                {
                    customerCode++;
                    var response = StatusCode(200, new { CustomerCode = string.Concat("KH", customerCode) });
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
                    userMsg = Properties.ResourcesCommon.Exception_ErrorMsg,
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
                // Validate các trường bắt buộc: CustomerCode, FullName, Email, PhoneNumber
                if (customer.CustomerCode == "" || customer.CustomerCode == null)
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
                var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);

                //4. Trả về
                if(rowEffects > 0)
                {
                    var response = StatusCode(201);
                    return response;
                }
                else
                {
                    var errorObj = new
                    {
                        devMsg = Properties.ResourcesCommon.NotExist,
                        userMsg = Properties.ResourcesCommon.NotExist,
                        errorCode = "",
                        moreInfo = "",
                        traceId = ""
                    };
                    return StatusCode(204);
                }
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourcesCommon.Exception_ErrorMsg,
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
                //Connect toi Database
                var connectionString = "Host = 47.241.69.179;" +
                "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

                IDbConnection dbConnection = new MySqlConnection(connectionString);

                //Lấy dữ liệu
                var sqlCommand = $"DELETE FROM Customer WHERE CustomerId = @CustomerIdParam";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@CustomerIdParam", customerId);

                var customer = dbConnection.Execute(sqlCommand, dynamicParameters);

                //4. Trả về cho client
                var response = StatusCode(200, customer);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourcesCommon.Exception_ErrorMsg,
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
                //1. Khai báo thông tin kết nối database
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
                    "User Id = dev;" +
                    "Password = 12345678";

                //2. Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                //3. Thêm dữ liệu
                var columnName = string.Empty;
                var columnValue = string.Empty;

                //Khai bao dynamic parameters
                var dynamicParam = new DynamicParameters();

                //Lưu vào List các cặp giá trị (key,value)
                var queryValue = new List<string>();

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
                    queryValue.Add($"{itemName} = @{itemName}");

                }

                dynamicParam.Add("@CustomerId", customerId);
                // Thực hiện truy vấn, thêm mới bản ghi
                var sqlCommand = $"UPDATE Customer SET {String.Join(", ", queryValue.ToArray())} " +
                                    $"WHERE CustomerId = @CustomerId";

                var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);

                //4. Trả về
                var response = StatusCode(200, rowEffects);
                return response;
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.ResourcesCommon.Exception_ErrorMsg,
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
