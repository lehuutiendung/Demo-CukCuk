using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Services
{
    public interface ICustomerService:IBaseService<Customer>
    {
        /*/// <summary>
        /// Lấy tất cả khách hàng
        /// </summary>
        /// <returns></returns>
        ServiceResult GetCustomer();

        /// <summary>
        /// Lấy thông tin của một khách hàng
        /// </summary>
        /// <param name="customerId">Id của khách hàng (Khóa chính)</param>
        /// <returns>Thông tin của khách hàng</returns>
        ServiceResult GetCustomerById(Guid customerId);

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
        ServiceResult FilterCustomer(int pageSize, int pageNumber, String fullName, String customerCode, String phoneNumber);

        /// <summary>
        /// Lấy thông tin mã khách hàng mới và trả về cho form nhập liệu
        /// </summary>
        /// <returns>Mã khách hàng</returns>
        ServiceResult NewCustomerCode();

        /// <summary>
        /// Xóa thông tin cả 1 khách hàng
        /// </summary>
        /// <returns></returns>
        ServiceResult DeleteCustomer(Guid customerId);

        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="customer">Thông tin của khách hàng</param>
        /// <returns> ServiceResult : kết quả xử lý qua nghiệp vụ</returns>
        /// CreatedBy: LHTDUNG - 13/08/2021
        ServiceResult AddCustomer(Customer customer);

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="customerId">Khóa chính - Id của khách hàng</param>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns> ServiceResult : kết quả xử lý qua nghiệp vụ</returns>
        /// CreatedBy: LHTDUNG - 13/08/2021
        ServiceResult UpdateCustomer(Guid customerId, Customer customer);
*/
    }
}
