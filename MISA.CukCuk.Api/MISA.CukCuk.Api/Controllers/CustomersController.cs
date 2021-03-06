using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Services;
using MySqlConnector;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : BaseController<Customer>
    {
        ICustomerService _customerService;
        public CustomersController(ICustomerService customerService, IBaseService<Customer> baseService) : base(baseService)
        {
            _customerService = customerService;
        }

        #region Ghi đè API thêm mới để thay đổi Resouces Msg

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Add(Customer customer)
        {
            try
            {
                var serviceResult = _customerService.Add(customer);
                if (serviceResult.IsValid == true)
                {
                    if ((int)serviceResult.Data == 400)
                    {
                        var errorObj = new
                        {
                            devMsg = MISA.ApplicationCore.Resources.ResourcesCommon.Duplicate_ErrorMsg.Replace("{}", "khách hàng"),
                            userMsg = MISA.ApplicationCore.Resources.ResourcesCommon.Duplicate_ErrorMsg.Replace("{}", "khách hàng"),
                            errorCode = "",
                            moreInfo = "",
                            traceId = ""
                        };
                        return StatusCode(400, errorObj);
                    }
                    return StatusCode(201, serviceResult.Data); ;
                }
                else
                {
                    var errorObj = new
                    {
                        devMsg = serviceResult.Data,
                        userMsg = serviceResult.Data,
                        errorCode = "",
                        moreInfo = "",
                        traceId = ""
                    };
                    return BadRequest(errorObj);
                }

            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = MISA.ApplicationCore.Resources.ResourcesCommon.Exception_ErrorMsg,
                    errorCode = "",
                    moreInfo = "",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }

        }
        #endregion

        /// <summary>
        /// Import file excel
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("Import")]
        public IActionResult Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            try
            {
                //Check file null
                if (formFile == null || formFile.Length <= 0)
                {
                    var errorObj = new
                    {
                        devMsg = MISA.ApplicationCore.Resources.ResourcesCommon.File_ErrorMsg,
                        userMsg = MISA.ApplicationCore.Resources.ResourcesCommon.File_ErrorMsg,
                    };
                    return BadRequest(errorObj);
                }

                //Check định dạng file .xlsx
                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    var errorObj = new
                    {
                        devMsg = MISA.ApplicationCore.Resources.ResourcesCommon.NotSupportFile_ErrorMsg,
                        userMsg = MISA.ApplicationCore.Resources.ResourcesCommon.NotSupportFile_ErrorMsg,
                    };
                    return BadRequest(errorObj);
                }

                var customersList = new List<Customer>();
                var dataRow = new List<ExcelWorksheet>();
                using (var stream = new MemoryStream())
                {
                    formFile.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                        var rowCount = workSheet.Dimension.Rows;
                        var cellCount = workSheet.Dimension.Columns;
                        for (int row = 3; row <= rowCount; row++)
                        {
                            var customer = new Customer();
                            //Sinh CustomerId cho mỗi customer khác nhau
                            customer.CustomerId = Guid.NewGuid();
                            var customerCode = workSheet.Cells[row, 1].Value;
                            var phoneNumber = workSheet.Cells[row, 5].Value;
                            var groupName = workSheet.Cells[row, 4].Value;

                            //Các trường chỉ cần check NULL
                            var fullName = workSheet.Cells[row, 2].Value;
                            var memberCardCode = workSheet.Cells[row, 3].Value;
                            var dateOfBirth = workSheet.Cells[row, 6].Value;
                            var companyName = workSheet.Cells[row, 7].Value;
                            var email = workSheet.Cells[row, 9].Value;
                            var address = workSheet.Cells[row, 10].Value;
                            if (fullName != null)
                            {
                                customer.FullName = fullName.ToString().Trim();
                            }
                            if(memberCardCode != null)
                            {
                                customer.MemberCardCode = memberCardCode.ToString().Trim();
                            }
                            if(companyName != null)
                            {
                                customer.CompanyName = companyName.ToString().Trim();
                            }
                            if (email != null)
                            {
                                customer.Email = email.ToString().Trim();
                            }
                            if (address != null)
                            {
                                customer.Address = address.ToString().Trim();
                            }
                            if (dateOfBirth != null)
                            {
                                var serviceResult = _customerService.FormatDate(dateOfBirth.ToString().Trim());
                                DateTime dateTime = DateTime.ParseExact(serviceResult.Data.ToString(), "dd/MM/yyyy", null);
                                customer.DateOfBirth = dateTime;
                            }

                            //Validate dữ liệu được import
                            _customerService.ImportValidateValue(customer, customerCode, phoneNumber, groupName, customersList);

                            customersList.Add(customer);
                        }

                    }
                }
                return StatusCode(200, customersList);
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = MISA.ApplicationCore.Resources.ResourcesCommon.Exception_ErrorMsg,
                    errorCode = "",
                    moreInfo = "",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }
        }

        /// <summary>
        /// Phương thức thực hiện thêm mới các bản ghi được import từ file excel
        /// </summary>
        /// <param name="customersList"></param>
        /// <returns></returns>
        [HttpPost("ImportAccept")]
        public IActionResult ImportAccept(List<Customer> customersList)
        {
            //Danh sách chứa các bản ghi có thể thêm 
            var successList = new List<Customer>();
            //Danh sách chứa các bản ghi lỗi không thể add
            var errorList = new List<Customer>();
            foreach (var customer in customersList)
            {
                if (customer.ImportError.Count() > 0)
                {
                    //Thêm các bản ghi có lỗi vào errorList
                    errorList.Add(customer);
                }
                else
                {
                    //Thêm các bản ghi OK vào successList
                    successList.Add(customer);
                }
            }
            try
            {
                var serviceResult = _customerService.ImportAccept(successList);
                if (serviceResult.IsValid)
                {
                    //Trả về số bản ghi thêm mới thành công, số bản ghi lỗi
                    var response = StatusCode(201, new { SuccessRow = serviceResult.Data, ErrorRow = errorList.Count() });
                    return response;
                }
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = MISA.ApplicationCore.Resources.ResourcesCommon.Exception_ErrorMsg,
                    errorCode = "",
                    moreInfo = "",
                    traceId = ""
                };
                return StatusCode(500, errorObj);
            }

            return Ok();
        }

        [HttpPost("Export")]
        public IActionResult Export(CancellationToken cancellationToken, List<Customer> customersErrorList)
        {
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = "Mã khách hàng";
                workSheet.Cells[1, 2].Value = "Tên khách hàng";
                workSheet.Cells[1, 3].Value = "Mã thẻ thành viên";
                workSheet.Cells[1, 4].Value = "Nhóm khách hàng";
                workSheet.Cells[1, 5].Value = "Số điện thoại";
                workSheet.Cells[1, 6].Value = "Ngày sinh";
                workSheet.Cells[1, 7].Value = "Tên công ty"; 
                workSheet.Cells[1, 8].Value = "Mã số thuế";
                workSheet.Cells[1, 9].Value = "Email";
                workSheet.Cells[1, 10].Value = "Địa chỉ";
                workSheet.Cells[1, 11].Value = "Ghi chú";

                var i = 0;
                var totalRows = customersErrorList.Count();
                for(int row = 2; row <= totalRows + 1; row++)
                {
                    workSheet.Cells[row, 1].Value = customersErrorList[i].CustomerCode;
                    workSheet.Cells[row, 2].Value = customersErrorList[i].FullName;
                    workSheet.Cells[row, 3].Value = customersErrorList[i].MemberCardCode;
                    workSheet.Cells[row, 5].Value = customersErrorList[i].PhoneNumber;
                    workSheet.Cells[row, 6].Value = customersErrorList[i].DateOfBirth;
                    workSheet.Cells[row, 7].Value = customersErrorList[i].CompanyName;
                    workSheet.Cells[row, 9].Value = customersErrorList[i].Email;
                    workSheet.Cells[row, 10].Value = customersErrorList[i].Address;
                    i++;
                }

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Danh sách import lỗi.xlsx";
            string fileGuid = Guid.NewGuid().ToString();
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
