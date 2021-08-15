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
    public class EmployeeRepository:IEmployeeRepository
    {
        public int AddEmployee(Employee employee)
        {
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
            //var check = dbConnection.Query<int>(sqlCode, dynamicParameters);
            /*if (check.Count() > 0)
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
            }*/
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
            var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);
            return rowEffects;
        }

        public int DeleteEmployee(Guid employeeId)
        {
            //Connect toi Database
            var connectionString = "Host = 47.241.69.179;" +
            "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
            "User Id = dev;" +
            "Password = 12345678";

            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //Lấy dữ liệu
            var sqlCommand = $"DELETE FROM Employee WHERE EmployeeId = @EmployeeIdParam";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@EmployeeIdParam", employeeId);

            var employeeEffect = dbConnection.Execute(sqlCommand, dynamicParameters);
            return employeeEffect;
        }

        public object FilterEmployee(int pageSize, int pageNumber, string fullName, string employeeCode, string phoneNumber)
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
            var sqlCommand = $"SELECT * FROM (SELECT * FROM Employee ORDER BY CreatedDate DESC LIMIT {pageSize} OFFSET {offSet}) paginate WHERE ( FullName LIKE @FullName AND EmployeeCode LIKE @EmployeeCode AND PhoneNumber LIKE @PhoneNumber )";

            dynamicParameters.Add($"@FullName", $"%{fullName}%");
            dynamicParameters.Add($"@EmployeeCode", $"%{employeeCode}%");
            dynamicParameters.Add($"@PhoneNumber", $"%{phoneNumber}%");

            var sqltotalRecord = $"SELECT COUNT(EmployeeCode) AS totalRecord FROM Employee";

            var employee = dbConnection.Query<object>(sqlCommand, dynamicParameters);
            var totalRecord = dbConnection.QueryFirstOrDefault<int>(sqltotalRecord);
            var totalPage = Math.Ceiling(((double)totalRecord / (double)pageSize));
            return new { TotalPage = totalPage, TotalRecord = totalRecord, Data = employee };
        }

        public List<object> GetEmployee()
        {
            //1. Khai báo thông tin kết nối database
            var connectionString = "Host = 47.241.69.179;" +
                "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

            //2. Khởi tạo đối tượng kết nối với database
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //3. Lấy dữ liệu
            var sqlCommand = "SELECT * FROM Employee ORDER BY CreatedDate DESC";
            var employees = dbConnection.Query<object>(sqlCommand);
            return (List<object>)employees;
        }

        public object GetEmployeeById(Guid employeeId)
        {
            //Connect toi Database
            var connectionString = "Host = 47.241.69.179;" +
            "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
            "User Id = dev;" +
            "Password = 12345678";

            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //Lấy dữ liệu
            var sqlCommand = $"SELECT * FROM Employee WHERE EmployeeId = @EmployeeIdParam";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@EmployeeIdParam", employeeId);

            var employee = dbConnection.QueryFirstOrDefault<object>(sqlCommand, dynamicParameters);
            return employee;
        }

        public object NewEmployeeCode()
        {
            //1. Khai báo thông tin kết nối database
            var connectionString = "Host = 47.241.69.179;" +
                "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

            //2. Khởi tạo đối tượng kết nối với database
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //3. Lấy dữ liệu
            var sqlCommand = "SELECT CAST(SUBSTRING(EmployeeCode, 3, LENGTH(EmployeeCode)-1) AS int) AS c FROM Employee ORDER BY c DESC LIMIT 1";
            var employeeCode = dbConnection.QueryFirstOrDefault<int>(sqlCommand);
            employeeCode++;
            return new { EmployeeCode = string.Concat("KH", employeeCode) };
        }

        public int UpdateEmployee(Guid employeeId, Employee employee)
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
                queryValue.Add($"{itemName} = @{itemName}");

            }

            dynamicParam.Add("@EmployeeId", employeeId);
            // Thực hiện truy vấn, thêm mới bản ghi
            var sqlCommand = $"UPDATE Employee SET {String.Join(", ", queryValue.ToArray())} " +
                                $"WHERE EmployeeId = @EmployeeId";

            var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);
            return rowEffects;
        }
    }
}
