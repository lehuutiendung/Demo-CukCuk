using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Services
{
    public interface IEmployeeService:IBaseService<Employee>
    {
        /*/// <summary>
        /// Lấy tất cả nhân viên
        /// </summary>
        /// <returns></returns>
        ServiceResult GetEmployee();

        /// <summary>
        /// Lấy thông tin của một nhân viên
        /// </summary>
        /// <param name="employeeId">Id của nhân viên (Khóa chính)</param>
        /// <returns>Thông tin của nhân viên</returns>
        ServiceResult GetEmployeeById(Guid employeeId);

        /// <summary>
        /// Bộ lọc theo Tên, Mã nhân viên, Số điện thoại
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="employeeCode"></param>
        /// <param name="phoneNumber"></param>
        /// <returns>
        ///     totalRecord
        ///     {employee}
        /// </returns>
        ServiceResult FilterEmployee(int pageSize, int pageNumber, String fullName, String employeeCode, String phoneNumber);

        /// <summary>
        /// Lấy thông tin mã nhân viên mới và trả về cho form nhập liệu
        /// </summary>
        /// <returns>Mã nhân viên</returns>
        ServiceResult NewEmployeeCode();

        /// <summary>
        /// Xóa thông tin của 1 nhân viên
        /// </summary>
        /// <returns></returns>
        ServiceResult DeleteEmployee(Guid employeeId);

        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="customer">Thông tin của nhân viên</param>
        /// <returns> ServiceResult : kết quả xử lý qua nghiệp vụ</returns>
        /// CreatedBy: LHTDUNG - 15/08/2021
        ServiceResult AddEmployee(Employee employee);

        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        /// <param name="employeeId">Khóa chính - Id của nhân viên</param>
        /// <param name="employee">Thông tin nhân viên</param>
        /// <returns> ServiceResult : kết quả xử lý qua nghiệp vụ</returns>
        /// CreatedBy: LHTDUNG - 15/08/2021
        ServiceResult UpdateEmployee(Guid employeeId, Employee employee);*/
    }
}
