using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repository;
using MISA.ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Services
{
    public class EmployeeService:BaseService<Employee>, IEmployeeService
    {
        IEmployeeRepository _employeeRepository;
        ServiceResult _serviceResult;

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="employeeRepository"></param>
        public EmployeeService(IBaseRepository<Employee> baseRepository, IEmployeeRepository employeeRepository) :base(baseRepository)
        {
            _employeeRepository = employeeRepository;
            _serviceResult = new ServiceResult();
        }

        public ServiceResult FilterEmployee(int pageSize, int pageNumber, string filter, string department, string position)
        {
            _serviceResult.Data = _employeeRepository.Filter(pageSize, pageNumber, filter, department, position);
            return _serviceResult;
        }

        #region Code Clean Architecture

        /*public ServiceResult AddEmployee(Employee employee)
        {
            //Xử lý nghiệp vụ
            // Validate các trường bắt buộc: EmployeeCode, FullName, Email, PhoneNumber
            if (employee.EmployeeCode == "" || employee.EmployeeCode == null)
            {
                var errorObj = new
                {
                    devMsg = Resources.ResourcesEmployee.EmployeeCode_ErrorMsg,
                    userMsg = Resources.ResourcesEmployee.EmployeeCode_ErrorMsg,
                    errorCode = "",
                };
                _serviceResult.IsValid = false;
                _serviceResult.Data = errorObj;
                return _serviceResult;
            }
            else if (employee.FullName == "" || employee.FullName == null)
            {
                var errorObj = new
                {
                    devMsg = Resources.ResourcesCommon.FullName_ErrorMsg,
                    userMsg = Resources.ResourcesCommon.FullName_ErrorMsg,
                    errorCode = "",
                };
                _serviceResult.IsValid = false;
                _serviceResult.Data = errorObj;
                return _serviceResult;
            }
            else if (employee.Email == "" || employee.Email == null)
            {
                var errorObj = new
                {
                    devMsg = Resources.ResourcesCommon.Email_ErrorMsg,
                    userMsg = Resources.ResourcesCommon.Email_ErrorMsg,
                    errorCode = "",
                };
                _serviceResult.IsValid = false;
                _serviceResult.Data = errorObj;
                return _serviceResult;
            }
            else if (employee.PhoneNumber == "" || employee.PhoneNumber == null)
            {
                var errorObj = new
                {
                    devMsg = Resources.ResourcesCommon.PhoneNumber_ErrorMsg,
                    userMsg = Resources.ResourcesCommon.PhoneNumber_ErrorMsg,
                    errorCode = "",
                };
                _serviceResult.IsValid = false;
                _serviceResult.Data = errorObj;
                return _serviceResult;
            }

            // Email phải đúng định dạng
            var emailValidate = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            var isMatch = Regex.IsMatch(employee.Email, emailValidate, RegexOptions.IgnoreCase);
            if (isMatch == false)
            {
                var errorObj = new
                {
                    devMsg = Resources.ResourcesCommon.ValidateEmail_ErrorMsg,
                    userMsg = Resources.ResourcesCommon.ValidateEmail_ErrorMsg,
                    errorCode = "",
                    moreInfo = "",
                    traceId = ""
                };
                _serviceResult.IsValid = false;
                _serviceResult.Data = errorObj;
                return _serviceResult;
            }
            //Xử lý kết quả trả về sau khi tương tác với Database
            _serviceResult.Data = _employeeRepository.AddEmployee(employee);
            return _serviceResult;
        }

        public ServiceResult DeleteEmployee(Guid employeeId)
        {
            _serviceResult.Data = _employeeRepository.DeleteEmployee(employeeId);
            return _serviceResult;
        }

        public ServiceResult FilterEmployee(int pageSize, int pageNumber, string fullName, string employeeCode, string phoneNumber)
        {
            _serviceResult.Data = _employeeRepository.FilterEmployee(pageSize, pageNumber, fullName, employeeCode, phoneNumber);
            return _serviceResult;
        }

        public ServiceResult GetEmployee()
        {
            //Xử lý kết quả trả về sau khi tương tác Database
            _serviceResult.Data = _employeeRepository.GetEmployee();
            return _serviceResult;
        }

        public ServiceResult GetEmployeeById(Guid employeeId)
        {
            _serviceResult.Data = _employeeRepository.GetEmployeeById(employeeId);
            return _serviceResult;
        }

        public ServiceResult NewEmployeeCode()
        {
            _serviceResult.Data = _employeeRepository.NewEmployeeCode();
            return _serviceResult;
        }

        public ServiceResult UpdateEmployee(Guid employeeId, Employee employee)
        {
            _serviceResult.Data = _employeeRepository.UpdateEmployee(employeeId, employee);
            return _serviceResult;
        }*/

        #endregion

    }
}
