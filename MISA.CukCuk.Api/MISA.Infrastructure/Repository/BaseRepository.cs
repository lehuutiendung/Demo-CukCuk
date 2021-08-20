using Dapper;
using Microsoft.Extensions.Configuration;
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
        // Khai báo thông tin kết nối database
        protected IDbConnection _dbConnection;
        public readonly string _connectionString;

        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CukCukDatabase");
        }

        #region Tương tác với database thực hiện thêm mới
        /// <summary>
        /// Tương tác với database thực hiện thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(Entity entity)
        {
            var className = typeof(Entity).Name;
            // Khởi tạo đối tượng kết nối với database
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                // Thêm dữ liệu
                var columnName = string.Empty;
                var columnValue = string.Empty;

                //Khai bao dynamic parameters
                var dynamicParam = new DynamicParameters();
                // Parameter cho {className}Code
                var dynamicParameters = new DynamicParameters();

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

                    if (itemName == $"{className}Code")
                    {
                        dynamicParameters.Add($"@{className}Code", itemValue);
                    }

                    //Thêm param tương ứng với mỗi property
                    dynamicParam.Add($"@{itemName}", itemValue);

                    columnName += $"{itemName},";
                    columnValue += $"@{itemName},";
                }
                #region Check trùng mã
                // Mã khách hàng không được trùng
                var sqlCode = $"SELECT * FROM {className} WHERE {$"{className}Code"} = @{className}Code";
                //var sqlCode = $"SELECT * FROM Customer c WHERE c.CustomerCode = 'KH123' ";

                //Truy vấn mã khách hàng trong database (kiểm tra trùng lặp)
                var check = _dbConnection.Query<object>(sqlCode, param: dynamicParameters);
                if (check.Count() > 0)
                {
                    return (int)400;
                }
                #endregion

                //Xóa dấu phẩy cuối cùng trong các dãy string columnName, columnValue
                columnName = columnName.Remove(columnName.Length - 1, 1);
                columnValue = columnValue.Remove(columnValue.Length - 1, 1);


                var sqlCommand = $"INSERT INTO {className}({columnName}) VALUES({columnValue})";
                var rowEffects = _dbConnection.Execute(sqlCommand, param: dynamicParam);
                return rowEffects;
            }  
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

            using ( _dbConnection = new MySqlConnection(_connectionString))
            {
                //Lấy dữ liệu
                var className = typeof(Entity).Name;
                var sqlCommand = $"DELETE FROM {className} WHERE {className}Id = @{className}IdParam";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add($"@{className}IdParam", entityId);

                var entityEffect = _dbConnection.Execute(sqlCommand, dynamicParameters);
                return entityEffect;
            }

        }
        #endregion

        #region Tương tác với database thực hiện xóa nhiều bản ghi
        public int DeleteMultiple(List<Guid> listId)
        {

            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                //Biến kiểm soát roll back database khi có truy vấn không thành công
                var transaction = _dbConnection.BeginTransaction();
                //Lấy dữ liệu
                var className = typeof(Entity).Name;
                var arrayValue = new List<string>();

                foreach (var item in listId)
                {
                    var formatItem = "\'" + item + "\'";
                    //arrayValue += $"{item},";
                    arrayValue.Add(formatItem);
                };

                //Thực hiện truy vấn xóa dữ liệu
                var sqlCommand = $"DELETE FROM {className} WHERE {className}Id IN ({String.Join(", ", arrayValue.ToArray())})";
                var entityEffect = _dbConnection.Execute(sqlCommand, transaction: transaction);
                transaction.Commit();
                return entityEffect;
            }

                
        }
        #endregion

        #region Tương tác với database thực hiện filter, phân trang
        /// <summary>
        /// Tương tác với database thực hiện filter, phân trang
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public object Filter(int pageSize, int pageNumber, string filter)
        {
            //Khởi tạo đối tượng kết nối với database
            using ( _dbConnection = new MySqlConnection(_connectionString))
            {
                DynamicParameters dynamicParameters = new DynamicParameters();

                //Tính toán giá trị Offset
                int offSet = pageSize * (pageNumber - 1);

                //Câu lệnh truy vấn phân trang và filter theo các tiêu chí
                var className = typeof(Entity).Name;
                var sqlCommand = "";
                if (className == "Employee")
                {
                    sqlCommand = $"SELECT * FROM (SELECT e.*, d.DepartmentName, p.PositionName, " +
                                                       $"(CASE e.Gender WHEN 0 THEN 'Nữ' WHEN 1 THEN 'Nam' ELSE 'Không xác định' END) AS GenderName " +
                                                       $"FROM {className} e " +
                                                       $"LEFT JOIN Department d ON d.DepartmentId = e.DepartmentId " +
                                                       $"LEFT JOIN Position p ON p.PositionId = e.PositionId " +
                                                       $"ORDER BY e.CreatedDate DESC LIMIT {pageSize} OFFSET {offSet}) paginate " +
                                                       $"WHERE ( paginate.FullName LIKE @FullName OR paginate.{className}Code LIKE @{className}Code OR paginate.PhoneNumber LIKE @PhoneNumber OR paginate.PhoneNumber IS NULL)";

                }
                if (className == "Customer")
                {
                    sqlCommand = $"SELECT * FROM (SELECT c.*, cg.CustomerGroupName, " +
                                                        $"(CASE c.Gender WHEN 0 THEN 'Nữ' WHEN 1 THEN 'Nam' ELSE 'Không xác định' END) AS GenderName " +
                                                        $"FROM Customer c " +
                                                        $"LEFT JOIN CustomerGroup cg ON cg.CustomerGroupId = c.CustomerGroupId " +
                                                        $"ORDER BY c.CreatedDate DESC LIMIT {pageSize} OFFSET {offSet}) paginate " +
                                                        $"WHERE ( FullName LIKE @FullName OR {className}Code LIKE @{className}Code OR PhoneNumber LIKE @PhoneNumber OR paginate.PhoneNumber IS NULL)";



                }

                dynamicParameters.Add($"@FullName", $"%{filter}%");
                dynamicParameters.Add($"@{className}Code", $"%{filter}%");
                dynamicParameters.Add($"@PhoneNumber", $"%{filter}%");

                var sqltotalRecord = $"SELECT COUNT({className}Code) AS totalRecord FROM {className}";

                var entity = _dbConnection.Query<object>(sqlCommand, dynamicParameters);
                var totalRecord = _dbConnection.QueryFirstOrDefault<int>(sqltotalRecord);
                var totalPage = Math.Ceiling(((double)totalRecord / (double)pageSize));
                return new { TotalPage = totalPage, TotalRecord = totalRecord, Data = entity };
            }     
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
            // Khởi tạo đối tượng kết nối với database
            using ( _dbConnection = new MySqlConnection(_connectionString))
            {
                // Lấy dữ liệu
                var className = typeof(Entity).Name;
                var sqlCommand = "";
                if (className == "Employee")
                {
                    sqlCommand = $"SELECT e.*, d.DepartmentName, p.PositionName, (CASE e.Gender WHEN 0 THEN 'Nữ' WHEN 1 THEN 'Nam' ELSE 'Không xác định' END) AS GenderName FROM {className} e LEFT JOIN Department d ON d.DepartmentId = e.DepartmentId LEFT JOIN Position p ON p.PositionId = e.PositionId ORDER BY e.CreatedDate DESC";
                }
                if (className == "Customer")
                {
                    sqlCommand = $"SELECT c.*, cg.CustomerGroupName, (CASE c.Gender WHEN 0 THEN 'Nữ' WHEN 1 THEN 'Nam' ELSE 'Không xác định' END) AS GenderName FROM {className} c LEFT JOIN CustomerGroup cg ON cg.CustomerGroupId = c.CustomerGroupId ORDER BY c.CreatedDate DESC";
                }
                var resultObject = _dbConnection.Query<object>(sqlCommand);
                return (List<object>)resultObject;
            }    
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
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                //Lấy dữ liệu
                var className = typeof(Entity).Name;
                var sqlCommand = "";
                if(className == "Employee")
                {
                    sqlCommand = $"SELECT e.*, d.DepartmentName, p.PositionName, (CASE e.Gender WHEN 0 THEN 'Nữ' WHEN 1 THEN 'Nam' ELSE 'Không xác định' END) AS GenderName FROM {className} e LEFT JOIN Department d ON d.DepartmentId = e.DepartmentId LEFT JOIN Position p ON p.PositionId = e.PositionId WHERE {$"{className}Id"} = @{className}IdParam ORDER BY e.CreatedDate DESC";
                }
                if(className == "Customer")
                {
                    sqlCommand = $"SELECT c.*, cg.CustomerGroupName, (CASE c.Gender WHEN 0 THEN 'Nữ' WHEN 1 THEN 'Nam' ELSE 'Không xác định' END) AS GenderName FROM {className} c LEFT JOIN CustomerGroup cg ON cg.CustomerGroupId = c.CustomerGroupId WHERE {$"{className}Id"} = @{className}IdParam ORDER BY c.CreatedDate DESC";
                }
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add($"@{className}IdParam", entityId);

                //Trả về object khách hàng (nhân viên)
                var result = _dbConnection.QueryFirstOrDefault<object>(sqlCommand, dynamicParameters);
                return result;
            }
        }
        #endregion

        #region Tương tác với database thực hiện sinh mã (nhân viên, khách)
        /// <summary>
        /// Tương tác với database thực hiện sinh mã (nhân viên, khách)
        /// </summary>
        /// <returns>EmployeeCode hoặc CustomerCode</returns>
        public object NewCode()
        {

            // Khởi tạo đối tượng kết nối với database
            using ( _dbConnection = new MySqlConnection(_connectionString))
            {
                // Lấy dữ liệu
                var className = typeof(Entity).Name;
                var sqlCommand = $"SELECT CAST(SUBSTRING({className}Code, 3, LENGTH({className}Code)-1) AS int) AS c FROM {className} ORDER BY c DESC LIMIT 1";
                var resultCode = _dbConnection.QueryFirstOrDefault<int>(sqlCommand);
                resultCode++;
                if (className.Equals("Employee"))
                {
                    return new { EmployeeCode = string.Concat("NV", resultCode) };

                }
                else return new { CustomerCode = string.Concat("KH", resultCode) };
            }
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

            // Khởi tạo đối tượng kết nối với database
            using ( _dbConnection = new MySqlConnection(_connectionString))
            {
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

                var rowEffects = _dbConnection.Execute(sqlCommand, param: dynamicParam);
                return rowEffects;
            } 
        }
        #endregion

    }
}