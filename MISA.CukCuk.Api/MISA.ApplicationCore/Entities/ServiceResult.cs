using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    public class ServiceResult
    {
        /// <summary>
        /// Kiểm soát kết quả trả về đúng hay sai
        /// </summary>
        public bool IsValid { get; set; } = true;

        /// <summary>
        /// Data mà kết quả trả về
        /// </summary>
        public object Data { get; set; }

        public string Messenger { get; set; }
    }
}
