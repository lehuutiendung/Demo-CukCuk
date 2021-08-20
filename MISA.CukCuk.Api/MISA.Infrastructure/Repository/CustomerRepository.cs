using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repository;
using MISA.ApplicationCore.MISAAttribute;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IConfiguration configuration):base(configuration)
        {

        }

        /// <summary>
        /// Truy vấn đếm số CustomerCode đã có
        /// </summary>
        /// <param name="cutomerCode"></param>
        /// <returns></returns>
        public int DuplicateCode(string cutomerCode)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                // Mã khách hàng không được trùng
                var dynamicParameters = new DynamicParameters();
                var sqlCode = $"SELECT * FROM Customer WHERE CustomerCode = @CustomerCode";
                dynamicParameters.Add("@CustomerCode", cutomerCode);
                //Truy vấn mã khách hàng trong database (kiểm tra trùng lặp)
                var check = _dbConnection.Query<object>(sqlCode, param: dynamicParameters);
                return check.Count();
            }
        }

        /// <summary>
        /// Truy vấn đếm SĐT đã có
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public int DuplicatePhoneNumber(string phoneNumber)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                // Mã khách hàng không được trùng
                var dynamicParameters = new DynamicParameters();
                var sqlCode = $"SELECT * FROM Customer WHERE PhoneNumber = @PhoneNumber";
                dynamicParameters.Add("@PhoneNumber", phoneNumber);
                //Truy vấn mã khách hàng trong database (kiểm tra trùng lặp)
                var check = _dbConnection.Query<object>(sqlCode, param: dynamicParameters);
                return check.Count();
            }
        }

        /// <summary>
        /// Truy vấn database lấy ra id của group (nếu group tồn tại)
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public IEnumerable<object> NotGroup(string groupName)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                // Mã khách hàng không được trùng
                var dynamicParameters = new DynamicParameters();
                var sqlCode = $"SELECT CustomerGroupId from CustomerGroup WHERE CustomerGroupName = @CustomerGroupName";
                dynamicParameters.Add("@CustomerGroupName", groupName);
                //Truy vấn mã khách hàng trong database (kiểm tra trùng lặp)
                var check = _dbConnection.Query<object>(sqlCode, param: dynamicParameters);
                return check;
            }
        }

        /// <summary>
        /// Thực hiện truy vấn database, Excute thêm mới các bản ghi
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        public int ImportAccept(List<Customer> customers)
        {
            var columnName = string.Empty;
            //Chỉ lấy tên các cột database lần đầu, các lần tiếp theo trùng lặp thì loại bỏ
            var countColumnName = 0;
            var columnValueList = new List<string>();
            var paramList = new List<DynamicParameters>();
            for(var i = 0; i < customers.Count(); i++)
            {
                countColumnName++;    

                // Thêm dữ liệu
                var columnValue = string.Empty;

                //Khai bao dynamic parameters
                var dynamicParam = new DynamicParameters();
                // Parameter cho {className}Code
                var dynamicParameters = new DynamicParameters();

                //Đọc từng property của object
                var properties = customers[i].GetType().GetProperties();

                //Duyệt từng property
                foreach (var item in properties)
                {
                    var propertyNotMap = item.GetCustomAttributes(typeof(MISANotMap), true);
                    if(propertyNotMap.Length == 0)
                    {
                        //Lấy tên của prop
                        var itemName = item.Name;

                        //Lấy value của prop
                        var itemValue = item.GetValue(customers[i]);

                        if (itemName == "CustomerId" && item.PropertyType == typeof(Guid))
                        {
                            // Sinh mã Guid cho EmployeeId
                            itemValue = Guid.NewGuid();
                        }

                        if (itemName == "CustomerCode")
                        {
                            dynamicParameters.Add("@CustomerCode", itemValue);
                        }

                        //Thêm param tương ứng với mỗi property
                        dynamicParam.Add($"@{itemName}", itemValue);

                        //Nếu là lần lặp đầu tiên, thực hiện lấy tên cột database
                        if (countColumnName == 1)
                        {
                            columnName += $"{itemName},";
                        }
                        columnValue += $"@{itemName},";
                    } 
                }
                paramList.Add(dynamicParam);
                columnValue = columnValue.Remove(columnValue.Length - 1, 1);
                columnValue = string.Concat("(", columnValue, ")");
                columnValueList.Add(columnValue);
            }
            //Xóa dấu phẩy cuối cùng trong các dãy string columnName, columnValue
            columnName = columnName.Remove(columnName.Length - 1, 1);

            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                _dbConnection.Open();
                var transaction = _dbConnection.BeginTransaction();
                var countEffect = 0;

                for (var j = 0; j < paramList.Count(); j++)
                {
                    var sqlCommand = $"INSERT INTO Customer ({columnName}) VALUES{columnValueList[j]}";
                
                    var rowEffects = _dbConnection.Execute(sqlCommand, transaction: transaction, param: paramList[j]);
                    if(rowEffects > 0)
                    {
                        countEffect++;
                    }
                }
                transaction.Commit();
                return countEffect;
            }
   
        }

    }
}
