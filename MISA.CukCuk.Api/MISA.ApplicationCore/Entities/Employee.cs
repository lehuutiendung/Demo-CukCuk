using MISA.ApplicationCore.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    public class Employee:BaseEntity
    {
        #region Property
        /// <summary>
        /// Khóa chính EmployeeId
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [MISARequired("Mã nhân viên")]

        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên lót
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Tên họ
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        [MISARequired("Họ tên")]

        public string FullName { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public int? Gender { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [MISARequired("Email")]

        public string Email { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [MISARequired("Số điện thoại")]

        public string PhoneNumber { get; set; }

        /// <summary>
        /// Lương
        /// </summary>
        public double? Salary { get; set; }

        /// <summary>
        /// Số CMTND/Căn cước
        /// </summary>
        [MISARequired("Số căn cước")]

        public string IdentityNumber { get; set; }
        #endregion

        /// <summary>
        /// Ngày cấp căn cước
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp
        /// </summary>
        public string IdentityPlace { get; set; }

        /// <summary>
        /// Id phòng ban
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// Id vị trí
        /// </summary>
        public string PositionId { get; set; }

        /// <summary>
        /// Mã số thuế cá nhân
        /// </summary>
        public string PersonalTaxCode { get; set; }

    }
}
