using Dapper;
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
    public class BaseRepository<Entity> : IBaseRepository<Entity>
    {
        #region Tương tác với database thực hiện thêm mới
        /// <summary>
        /// Tương tác với database thực hiện thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(Entity entity)
        {
            var className = typeof(Entity).Name;

            // Khai báo thông tin kết nối database
            var connectionString = "Host = 47.241.69.179;" +
                "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

            // Khởi tạo đối tượng kết nối với database
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            /*// Mã khách hàng không được trùng
            var sqlCode = $"SELECT * FROM {className} WHERE {className}Code = @{className}Code";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add($"@{className}Code", employee.EmployeeCode);
            //Truy vấn mã khách hàng trong database (kiểm tra trùng lặp)
            var check = dbConnection.Query<int>(sqlCode, dynamicParameters);
            if (check.Count() > 0)
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

            // Thêm dữ liệu
            var columnName = string.Empty;
            var columnValue = string.Empty;

            //Khai bao dynamic parameters
            var dynamicParam = new DynamicParameters();

            //Đọc từng property của object
            var properties = entity.GetType().GetProperties();

            //Duyệt từng property
            foreach (var item in properties)
            {
                //Lấy tên của prop
                var itemName = item.Name;

                //Lấy value của prop
                var itemValue = item.GetValue(entity);

                if (itemName == $"{className}Id" && item.PropertyType == typeof(Guid))
                {
                    // Sinh mã Guid cho EmployeeId
                    itemValue = Guid.NewGuid();
                }

                //Thêm param tương ứng với mỗi property
                dynamicParam.Add($"@{itemName}", itemValue);

                columnName += $"{itemName},";
                columnValue += $"@{itemName},";
            }

            //Xóa dấu phẩy cuối cùng trong các dãy string columnName, columnValue
            columnName = columnName.Remove(columnName.Length - 1, 1);
            columnValue = columnValue.Remove(columnValue.Length - 1, 1);


            var sqlCommand = $"INSERT INTO {className}({columnName}) VALUES({columnValue})";
            var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);
            return rowEffects;
        }
        #endregion

        #region Tương tác với database thực hiện xóa một bản ghi
        /// <summary>
        /// Tương tác với database thực hiện xóa một bản ghi
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public int Delete(Guid entityId)
        {
            //Connect toi Database
            var connectionString = "Host = 47.241.69.179;" +
            "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
            "User Id = dev;" +
            "Password = 12345678";

            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //Lấy dữ liệu
            var className = typeof(Entity).Name;
            var sqlCommand = $"DELETE FROM {className} WHERE {className}Id = @{className}IdParam";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add($"@{className}IdParam", entityId);

            var entityEffect = dbConnection.Execute(sqlCommand, dynamicParameters);
            return entityEffect;
        }
        #endregion

        #region Tương tác với database thực hiện filter, phân trang
        /// <summary>
        /// Tương tác với database thực hiện filter, phân trang
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="fullName"></param>
        /// <param name="entityCode"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public object Filter(int pageSize, int pageNumber, string fullName, string entityCode, string phoneNumber)
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
            var className = typeof(Entity).Name;
            var sqlCommand = $"SELECT * FROM (SELECT * FROM {className} ORDER BY CreatedDate DESC LIMIT {pageSize} OFFSET {offSet}) paginate WHERE ( FullName LIKE @FullName AND {className}Code LIKE @{className}Code AND PhoneNumber LIKE @PhoneNumber )";

            dynamicParameters.Add($"@FullName", $"%{fullName}%");
            dynamicParameters.Add($"@{className}Code", $"%{entityCode}%");
            dynamicParameters.Add($"@PhoneNumber", $"%{phoneNumber}%");

            var sqltotalRecord = $"SELECT COUNT({className}Code) AS totalRecord FROM {className}";

            var entity = dbConnection.Query<object>(sqlCommand, dynamicParameters);
            var totalRecord = dbConnection.QueryFirstOrDefault<int>(sqltotalRecord);
            var totalPage = Math.Ceiling(((double)totalRecord / (double)pageSize));
            return new { TotalPage = totalPage, TotalRecord = totalRecord, Data = entity };
        }
        #endregion

        #region Tương tác với database thực hiện lấy tất cả bản ghi
        /// <summary>
        /// Tương tác database lấy tất cả bản ghi
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <returns></returns>
        public List<object> GetAll<Entity>()
        {
            //1. Khai báo thông tin kết nối database
            var connectionString = "Host = 47.241.69.179;" +
                "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

            //2. Khởi tạo đối tượng kết nối với database
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //3. Lấy dữ liệu
            var className = typeof(Entity).Name;
            var sqlCommand = $"SELECT * FROM {className} ORDER BY CreatedDate DESC";
            var resultObject = dbConnection.Query<object>(sqlCommand);
            return (List<object>)resultObject;
        }
        #endregion

        #region Tương tác database lấy bản ghi theo id
        /// <summary>
        /// Tương tác database lấy bản ghi theo id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public object GetById(Guid entityId)
        {
            //Connect toi Database
            var connectionString = "Host = 47.241.69.179;" +
            "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
            "User Id = dev;" +
            "Password = 12345678";

            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //Lấy dữ liệu
            var className = typeof(Entity).Name;
            var sqlCommand = $"SELECT * FROM {className} WHERE {$"{className}Id"} = @{className}IdParam";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add($"@{className}IdParam", entityId);

            //Trả về object khách hàng (nhân viên)
            var result = dbConnection.QueryFirstOrDefault<object>(sqlCommand, dynamicParameters);
            return result;
        }
        #endregion

        #region Tương tác với database thực hiện sinh mã (nhân viên, khách)
        /// <summary>
        /// Tương tác với database thực hiện sinh mã (nhân viên, khách)
        /// </summary>
        /// <returns>EmployeeCode hoặc CustomerCode</returns>
        public object NewCode()
        {
            //1. Khai báo thông tin kết nối database
            var connectionString = "Host = 47.241.69.179;" +
                "Database = WEB07.MF928.LHTDUNG.CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

            //2. Khởi tạo đối tượng kết nối với database
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //3. Lấy dữ liệu
            var className = typeof(Entity).Name;
            var sqlCommand = $"SELECT CAST(SUBSTRING({className}Code, 3, LENGTH({className}Code)-1) AS int) AS c FROM {className} ORDER BY c DESC LIMIT 1";
            var resultCode = dbConnection.QueryFirstOrDefault<int>(sqlCommand);
            resultCode++;
            if (className.Equals("Employee"))
            {
                return new { EmployeeCode = string.Concat("NV", resultCode) };

            }
            else return new { CustomerCode = string.Concat("KH", resultCode) };
        }
        #endregion

        #region Tương tác với database thực hiện cập nhật bản ghi
        /// <summary>
        /// Tương tác với database thực hiện cập nhật bản ghi
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(Guid entityId, Entity entity)
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
            var properties = entity.GetType().GetProperties();

            //Duyệt từng property
            foreach (var item in properties)
            {
                //Lấy tên của prop
                var itemName = item.Name;

                //Lấy value của prop
                var itemValue = item.GetValue(entity);

                //Thêm param tương ứng với mỗi property
                dynamicParam.Add($"@{itemName}", itemValue);
                queryValue.Add($"{itemName} = @{itemName}");

            }
            var className = typeof(Entity).Name;
            dynamicParam.Add($"@{className}Id", entityId);
            // Thực hiện truy vấn, thêm mới bản ghi
            var sqlCommand = $"UPDATE {className} SET {String.Join(", ", queryValue.ToArray())} " +
                                $"WHERE {className}Id = @{className}Id";

            var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);
            return rowEffects;
        }
        #endregion

    }
}
