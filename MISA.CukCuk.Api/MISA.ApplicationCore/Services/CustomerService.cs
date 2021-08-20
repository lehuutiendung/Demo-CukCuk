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
    public class CustomerService :BaseService<Customer>, ICustomerService
    {
        ICustomerRepository _customerRepository;
        ServiceResult _serviceResult;

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="customerRepository"></param>
        public CustomerService(IBaseRepository<Customer> baseRepository, ICustomerRepository customerRepository):base(baseRepository)
        {
            _customerRepository = customerRepository;
            _serviceResult = new ServiceResult();
        }

        /// <summary>
        /// Format định dạng ngày tháng từ các bản ghi được Import file excel
        /// </summary>
        /// <param name="dateOfBirth"></param>
        /// <returns></returns>
        public ServiceResult FormatDate(string dateOfBirth)
        {
            //Chỉ nhập năm sinh => 01/01/yyyy
            if(dateOfBirth.Length == 4)
            {
                dateOfBirth = string.Concat("01/01/", dateOfBirth); 
            }
            //Chỉ nhập tháng/năm sinh => 01/mm/yyyy
            if (dateOfBirth.Length == 6)
            {
                dateOfBirth = string.Concat("01/0", dateOfBirth);
            }
            if (dateOfBirth.Length == 7)
            {
                dateOfBirth = string.Concat("01/", dateOfBirth);
            }
            
            _serviceResult.Data = dateOfBirth;
            return _serviceResult;
        }

        /// <summary>
        /// Xử lý nghiệp vụ kiểm tra trùng mã được nhập từ file
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        public ServiceResult DuplicateCode(string customerCode)
        {
            _serviceResult = new ServiceResult();
            var customerCodeCount = _customerRepository.DuplicateCode(customerCode);
            if(customerCodeCount > 0)
            {
                var errorMsg = Resources.ResourcesCustomer.CustomerCode_Duplicate_ErrorMsg;
                _serviceResult.IsValid = false;
                _serviceResult.Messenger = errorMsg;
                return _serviceResult;
            }
            _serviceResult.Data = customerCodeCount;
            return _serviceResult;
        }

        /// <summary>
        /// Kiểm tra trùng số điện thoại được nhập từ file
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public ServiceResult DuplicatePhoneNumber(string phoneNumber)
        {
            _serviceResult = new ServiceResult();
            var phoneNumberCount = _customerRepository.DuplicatePhoneNumber(phoneNumber);
            if (phoneNumberCount > 0)
            {
                var errorMsg = Resources.ResourcesCustomer.PhoneNumber_Duplicate_ErrorMsg;
                _serviceResult.IsValid = false;
                _serviceResult.Messenger = errorMsg;
                return _serviceResult;
            }
            _serviceResult.Data = phoneNumberCount;
            return _serviceResult;
        }

        /// <summary>
        /// Tổng hợp kiểm tra trùng mã, số điện thoại, tên group tồn tại
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="customerCodeSheet"></param>
        /// <param name="phoneNumberSheet"></param>
        /// <param name="groupName"></param>
        /// <param name="customersList"></param>
        /// <returns></returns>
        public ServiceResult ImportValidateValue(Customer customer, object customerCodeSheet, object phoneNumberSheet, object groupName, List<Customer> customersList)
        {
            //Check mã khách hàng
            if (customerCodeSheet != null)
            {
                customer.CustomerCode = customerCodeSheet.ToString().Trim();
                var serviceResult = DuplicateCode(customer.CustomerCode);
                if (serviceResult.IsValid == false)
                {
                    customer.ImportError.Add(serviceResult.Messenger);
                }
            }

            //Check số điện thoại
            if (phoneNumberSheet != null)
            {
                customer.PhoneNumber = phoneNumberSheet.ToString().Trim();
                var serviceResult = DuplicatePhoneNumber(customer.PhoneNumber);
                if (serviceResult.IsValid == false)
                {
                    customer.ImportError.Add(serviceResult.Messenger);
                }
            }

            //Check tên group
            if (groupName != null)
            {
                var customerGroupName = groupName.ToString().Trim();
                var groupNameObject = _customerRepository.NotGroup(customerGroupName);
                if(groupNameObject.Count() <= 0)
                {
                    var errorMsg = Resources.ResourcesCustomer.GroupName_ErrorMsg;
                    customer.ImportError.Add(errorMsg);
                }
                else
                {
                    //Lấy giá trị của CustomerGroupId
                    var firstRow = groupNameObject.FirstOrDefault();
                    var Heading = ((IDictionary<string, object>)firstRow).Keys.ToArray();
                    var details = ((IDictionary<string, object>)firstRow);
                    customer.CustomerGroupId = details[Heading[0]].ToString();
                     
                }
            }

            foreach (var item in customersList)
            {
                if (customer.CustomerCode == item.CustomerCode)
                {
                    //Mã lỗi: Trùng mã với khách hàng trong tệp nhập khẩu
                    var duplicateCodeInFile = Resources.ResourcesCustomer.CustomerCode_DuplicateFile_ErrorMsg;
                    //Kiểm tra và thêm mã lỗi vào cả 2 đối tượng được khi được so sánh
                    if (!item.ImportError.Contains(duplicateCodeInFile))
                    {
                        item.ImportError.Add(duplicateCodeInFile);
                    }
                    customer.ImportError.Add(duplicateCodeInFile);
                }

                if (customer.PhoneNumber == item.PhoneNumber)
                {
                    //Mã lỗi: Trùng SĐT với khách hàng trong tệp nhập khẩu
                    var duplicatePhoneInFile = Resources.ResourcesCustomer.PhoneNumber_DuplicateFile_ErrorMsg;
                    //Kiểm tra và thêm mã lỗi vào cả 2 đối tượng được khi được so sánh
                    if (!item.ImportError.Contains(duplicatePhoneInFile))
                    {
                        item.ImportError.Add(duplicatePhoneInFile);
                    }
                    customer.ImportError.Add(duplicatePhoneInFile);
                }

            }

            return _serviceResult;
        }

        /// <summary>
        /// Thực hiện gọi hàm của Repo => Excute, thêm mới các bản ghi vào database
        /// </summary>
        /// <param name="customersList"></param>
        /// <returns></returns>
        public ServiceResult ImportAccept(List<Customer> customersList)
        {
            _serviceResult.Data = _customerRepository.ImportAccept(customersList);
            return _serviceResult;
        }
        #region Code Clean Architecture - Old Code

        /*public ServiceResult AddCustomer(Customer customer)
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
        }*/
        #endregion

    }
}
