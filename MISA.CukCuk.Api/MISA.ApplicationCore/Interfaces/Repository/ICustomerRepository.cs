using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Repository
{
    public interface ICustomerRepository:IBaseRepository<Customer>
    {
        /*List<object> GetCustomer();

        Object GetCustomerById(Guid customerId);

        Object FilterCustomer(int pageSize, int pageNumber, String fullName, String customerCode, String phoneNumber);

        Object NewCustomerCode();

        int AddCustomer(Customer customer);

        int DeleteCustomer(Guid customerId);

        int UpdateCustomer(Guid customerId, Customer customer);*/
        
        /// <summary>
        /// Interface kiểm tra trùng mã 
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        int DuplicateCode(String customerCode);

        /// <summary>
        /// Interface kiểm tra trùng số điện thoại
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        int DuplicatePhoneNumber(String phoneNumber);

        /// <summary>
        /// Interface kiểm tra tồn tại tên group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        IEnumerable<object> NotGroup(String groupName);

        /// <summary>
        /// Interface giao tiếp truy vấn thêm mới danh sách từ import file
        /// </summary>
        /// <param name="customersList"></param>
        /// <returns></returns>
        int ImportAccept(List<Customer> customersList);
    }
}
