using Dapper;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        public int AddCustomer(Customer customer)
        {
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
            //var check = dbConnection.Query<int>(sqlCode, dynamicParameters);
            /*if (check.Count() > 0)
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
            }*/            
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
            return rowEffects;
        }

        public int DeleteCustomer(Guid customerId)
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

            var customerEffect = dbConnection.Execute(sqlCommand, dynamicParameters);
            return customerEffect;
        }

        public object FilterCustomer(int pageSize, int pageNumber, string fullName, string customerCode, string phoneNumber)
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
            var sqlCommand = $"SELECT * FROM (SELECT * FROM Customer ORDER BY CreatedDate DESC LIMIT {pageSize} OFFSET {offSet}) paginate WHERE ( FullName LIKE @FullName AND CustomerCode LIKE @CustomerCode AND PhoneNumber LIKE @PhoneNumber )";

            dynamicParameters.Add($"@FullName", $"%{fullName}%");
            dynamicParameters.Add($"@CustomerCode", $"%{customerCode}%");
            dynamicParameters.Add($"@PhoneNumber", $"%{phoneNumber}%");

            var sqltotalRecord = $"SELECT COUNT(CustomerCode) AS totalRecord FROM Customer";

            var customer = dbConnection.Query<object>(sqlCommand, dynamicParameters);
            var totalRecord = dbConnection.QueryFirstOrDefault<int>(sqltotalRecord);
            var totalPage = Math.Ceiling(((double)totalRecord / (double)pageSize));
            return new { TotalPage = totalPage, TotalRecord = totalRecord, Data = customer };
        }

        public List<object> GetCustomer()
        {
            //1. Khai báo thông tin kết nối database
            var connectionString = "Host = 47.241.69.179;" +
                "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

            //2. Khởi tạo đối tượng kết nối với database
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //3. Lấy dữ liệu
            var sqlCommand = "SELECT * FROM Customer ORDER BY CreatedDate DESC";
            var customers = dbConnection.Query<object>(sqlCommand);
            return (List<object>)customers;
        }

        public object GetCustomerById(Guid customerId)
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
            return customer;
        }

        public object NewCustomerCode()
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
            customerCode++;
            return new { CustomerCode = string.Concat("KH", customerCode) };
        }

        public int UpdateCustomer(Guid customerId, Customer customer)
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
            return rowEffects;
        }
    }
}
