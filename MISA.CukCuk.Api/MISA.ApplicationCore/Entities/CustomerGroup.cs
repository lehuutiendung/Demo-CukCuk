using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    public class CustomerGroup:BaseEntity
    {
        #region Property
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid CustomerGroupId { get; set; }

        /// <summary>
        /// Tên nhóm khách hàng
        /// </summary>      
        public string CustomerGroupName { get; set; }

        /// <summary>
        /// Miêu tả
        /// </summary>
        public string Description { get; set; }


        #endregion
    }
}
