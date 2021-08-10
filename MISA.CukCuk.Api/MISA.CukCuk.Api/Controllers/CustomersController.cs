using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Api.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        /// <summary>
        /// Lấy tất cả khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCustomers()
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

            var response = StatusCode(200, customers);
            return response;
        }

        [HttpGet("{customerId}")]
        public IActionResult GetCustomerById(Guid customerId)
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

        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddCustomer(Customer customer)
        {
            //Sinh mã Guid cho EmployeeId
            customer.CustomerId = Guid.NewGuid();

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
            var response = StatusCode(200, rowEffects);
            return response;
        }

        [HttpDelete("{customerId}")]
        public IActionResult deleteCustomer(Guid customerId)
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

        /*[HttpPut("{customerId}")]
        public IActionResult editCustomer(Guid customerId, Customer customer)
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

            var sqlCommand = $"UPDATE Customer({columnName}) VALUES({columnValue})";
            var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);

            //4. Trả về
            var response = StatusCode(200, rowEffects);
            return response;
        }*/
    }
}
