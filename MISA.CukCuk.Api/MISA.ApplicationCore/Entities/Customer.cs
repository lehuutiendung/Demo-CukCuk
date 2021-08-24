using MISA.ApplicationCore.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    public class Customer:BaseEntity
    {
        #region Property
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        [MISARequired("Mã khách hàng")]
        public string CustomerCode { get; set; }

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
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [MISARequired("Email")]
        public string Email { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [MISARequired("Số điện thoại")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Id nhóm khách hàng
        /// </summary>
        public string CustomerGroupId { get; set; }

        /// <summary>
        /// Chứa danh sách mã của từng khách hàng khi import file
        /// </summary>
        [MISANotMap]
        public List<string> ImportError { get; set; } = new List<string>();

        /// <summary>
        /// Mã thẻ thành viên
        /// </summary>
        public string MemberCardCode { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        public string CompanyName { get; set; }

        #endregion
    }
}
