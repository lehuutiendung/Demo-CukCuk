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
    public class CustomerService : ICustomerService
    {
        ICustomerRepository _customerRepository;
        ServiceResult _serviceResult;

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="customerRepository"></param>
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _serviceResult = new ServiceResult();
        }

        public ServiceResult AddCustomer(Customer customer)
        {
            //Xử lý nghiệp vụ
            // Validate các trường bắt buộc: CustomerCode, FullName, Email, PhoneNumber
            if (customer.CustomerCode == "" || customer.CustomerCode == null)
            {
                var errorObj = new
                {
                    devMsg = Resources.ResourcesCustomer.CustomerCode_ErrorMsg,
                    userMsg = Resources.ResourcesCustomer.CustomerCode_ErrorMsg,
                    errorCode = "",
                };
                _serviceResult.IsValid = false;
                _serviceResult.Data = errorObj;
                return _serviceResult;
            }
            else if (customer.FullName == "" || customer.FullName == null)
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
            else if (customer.Email == "" || customer.Email == null)
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
            else if (customer.PhoneNumber == "" || customer.PhoneNumber == null)
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
            var isMatch = Regex.IsMatch(customer.Email, emailValidate, RegexOptions.IgnoreCase);
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
            _serviceResult.Data = _customerRepository.AddCustomer(customer);
            return _serviceResult;
        }

        public ServiceResult DeleteCustomer(Guid customerId)
        {
            _serviceResult.Data = _customerRepository.DeleteCustomer(customerId);
            return _serviceResult;
        }

        public ServiceResult FilterCustomer(int pageSize, int pageNumber, string fullName, string customerCode, string phoneNumber)
        {
            _serviceResult.Data = _customerRepository.FilterCustomer(pageSize, pageNumber, fullName, customerCode, phoneNumber);
            return _serviceResult;
        }

        public ServiceResult GetCustomer()
        {
            //Xử lý kết quả trả về sau khi tương tác Database
            _serviceResult.Data = _customerRepository.GetCustomer();
            return _serviceResult;
        }

        public ServiceResult GetCustomerById(Guid customerId)
        {
            _serviceResult.Data = _customerRepository.GetCustomerById(customerId);
            return _serviceResult;
        }

        public ServiceResult NewCustomerCode()
        {
            _serviceResult.Data = _customerRepository.NewCustomerCode();
            return _serviceResult;
        }

        public ServiceResult UpdateCustomer(Guid customerId, Customer customer)
        {
            _serviceResult.Data = _customerRepository.UpdateCustomer(customerId, customer);
            return _serviceResult;
        }
    }
}
