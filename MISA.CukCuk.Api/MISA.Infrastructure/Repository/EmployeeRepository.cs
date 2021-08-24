using Dapper;
using Microsoft.Extensions.Configuration;
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
    public class EmployeeRepository:BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration):base(configuration)
        {
            
        }
       
        public object FilterEmployee(int pageSize, int pageNumber, string filter, string department, string position)
        {
            //Khởi tạo đối tượng kết nối với database
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                DynamicParameters dynamicParameters = new DynamicParameters();

                //Tính toán giá trị Offset
                int offSet = pageSize * (pageNumber - 1);

                //Câu lệnh truy vấn phân trang và filter theo các tiêu chí
                var sqlCommand = $"SELECT * FROM (" +
                                      $"SELECT e.*, d.DepartmentName, p.PositionName, " +
                                           $"(CASE e.Gender WHEN 0 THEN 'Nữ' WHEN 1 THEN 'Nam' ELSE 'Không xác định' END) AS GenderName FROM Employee e " +
                                           $"LEFT JOIN Department d ON d.DepartmentId = e.DepartmentId " +
                                           $"LEFT JOIN `Position` p ON p.PositionId = e.PositionId " +
                                           $"ORDER BY e.CreatedDate DESC LIMIT {pageSize} OFFSET {offSet}" +
                                 $") paginate " +
                                 $"WHERE ( FullName LIKE @FullName OR EmployeeCode LIKE @EmployeeCode OR PhoneNumber LIKE @PhoneNumber AND ISNULL(NULL)) " +
                                 $"AND (paginate.DepartmentId = @DepartmentId OR @DepartmentId = '' OR @DepartmentId IS NULL) " +
                                 $"AND (paginate.PositionId = @PositionId OR @PositionId = '' OR @PositionId IS NULL)";


                dynamicParameters.Add($"@FullName", $"%{filter}%");
                dynamicParameters.Add($"@EmployeeCode", $"%{filter}%");
                dynamicParameters.Add($"@PhoneNumber", $"%{filter}%");
                dynamicParameters.Add($"@DepartmentId", $"{department}");
                dynamicParameters.Add($"@PositionId", $"{position}");


                var sqltotalRecord = $"SELECT COUNT(EmployeeCode) AS totalRecord FROM Employee";

                var entity = _dbConnection.Query<object>(sqlCommand, dynamicParameters);
                var totalRecord = _dbConnection.QueryFirstOrDefault<int>(sqltotalRecord);
                var totalPage = Math.Ceiling(((double)totalRecord / (double)pageSize));
                return new { TotalPage = totalPage, TotalRecord = totalRecord, Data = entity };
            }
        }
    }
}
