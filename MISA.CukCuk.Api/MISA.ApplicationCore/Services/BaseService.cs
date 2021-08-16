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
    public class BaseService<Entity> : IBaseService<Entity>
    {
        IBaseRepository<Entity> _baseRepository;
        ServiceResult _serviceResult;
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="entityRepository"></param>
        public BaseService(IBaseRepository<Entity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult();
        }

        #region Service thêm mới bản ghi

        /// <summary>
        /// Service thêm mới bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ServiceResult Add(Entity entity)
        {
            var isValid = ValidateCommon(entity).IsValid;
            _serviceResult = ValidateCommon(entity);
            if(isValid == true)
            {
                //Xử lý kết quả trả về sau khi tương tác với Database
                _serviceResult.Data = _baseRepository.Add(entity);
                return _serviceResult;
            }
            return _serviceResult;
        }
        #endregion

        #region Service xóa bản ghi

        /// <summary>
        /// Service xóa bản ghi
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public ServiceResult Delete(Guid entityId)
        {
            //Xử lý kết quả trả về sau khi tương tác với Database
            _serviceResult.Data = _baseRepository.Delete(entityId);
            return _serviceResult;
        }
        #endregion

        #region Service filter, phân trang

        /// <summary>
        /// Service filter, phân trang
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="fullName"></param>
        /// <param name="entityCode"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public ServiceResult Filter(int pageSize, int pageNumber, string fullName, string entityCode, string phoneNumber)
        {
            _serviceResult.Data = _baseRepository.Filter(pageSize, pageNumber, fullName, entityCode, phoneNumber);
            return _serviceResult;
        }
        #endregion

        #region Service lấy tất cả bản ghi

        /// <summary>
        /// Service lấy tất cả bản ghi
        /// </summary>
        /// <returns></returns>
        public ServiceResult GetAll()
        {
            //Xử lý kết quả trả về sau khi tương tác Database
            _serviceResult.Data = _baseRepository.GetAll<Entity>();
            return _serviceResult;
        }
        #endregion

        #region Service lấy Id

        /// <summary>
        /// Service lấy id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public ServiceResult GetById(Guid entityId)
        {
            //Xử lý kết quả trả về sau khi tương tác Database
            _serviceResult.Data = _baseRepository.GetById(entityId);
            return _serviceResult;
        }
        #endregion

        #region Service sinh mã mới

        /// <summary>
        /// Service sinh mã mới
        /// </summary>
        /// <returns></returns>
        public ServiceResult NewCode()
        {
            //Xử lý kết quả trả về sau khi tương tác Database
            _serviceResult.Data = _baseRepository.NewCode();
            return _serviceResult;
        }
        #endregion

        #region Service cập nhật bản ghi

        /// <summary>
        /// Service cập nhật bản ghi
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ServiceResult Update(Guid entityId, Entity entity)
        {
            var isValid = ValidateCommon(entity).IsValid;
            _serviceResult = ValidateCommon(entity);
            if (isValid == true)
            {
                //Xử lý kết quả trả về sau khi tương tác Database
                _serviceResult.Data = _baseRepository.Update(entityId, entity);
                return _serviceResult;
            }
            return _serviceResult;
        }
        #endregion

        #region Service validate các trường bắt buộc

        /// <summary>
        /// Validate các trường bắt buộc Code, FullName, Email, PhoneNumber
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private ServiceResult ValidateCommon (Entity entity)
        {
            var className = typeof(Entity).Name;
            for(var i = 0; i < typeof(Entity).GetProperties().Length; i++)
            {
                var _serviceResult = new ServiceResult();
                var item = typeof(Entity).GetProperties()[i];
                if (item.Name.Equals($"{className}Code") && item.GetValue(entity).Equals(""))
                {
                    _serviceResult.IsValid = false;
                    _serviceResult.Data = Resources.ResourcesCommon.Code_ErrorMsg;
                    return _serviceResult;
                }

                if (item.Name.Equals("FullName") && item.GetValue(entity).Equals(""))
                {
                    _serviceResult.IsValid = false;
                    _serviceResult.Data = Resources.ResourcesCommon.FullName_ErrorMsg;
                    return _serviceResult;
                };

                if (item.Name.Equals("Email"))
                {
                    if (item.GetValue(entity).Equals(""))
                    {
                        _serviceResult.IsValid = false;
                        _serviceResult.Data = Resources.ResourcesCommon.Email_ErrorMsg;
                        return _serviceResult;
                    }
                    else
                    {
                        // Email phải đúng định dạng
                        var emailValidate = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
                        var isMatch = Regex.IsMatch((string)item.GetValue(entity), emailValidate, RegexOptions.IgnoreCase);
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
                    }
                }
         
                if (item.Name.Equals("PhoneNumber") && item.GetValue(entity).Equals(""))
                {
                    _serviceResult.IsValid = false;
                    _serviceResult.Data = Resources.ResourcesCommon.PhoneNumber_ErrorMsg;
                    return _serviceResult;
                }
                
            }

            
            return _serviceResult;
        }
        #endregion

        #region Service validate custom

        /// <summary>
        /// Validate custom cho từng loại Entity (Khách hàng, Nhân viên)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual bool ValidateCustom (Entity entity)
        {
            return true;
        }
        #endregion

    }
}
