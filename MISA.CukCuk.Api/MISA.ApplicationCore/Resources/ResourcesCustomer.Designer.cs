//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MISA.ApplicationCore.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ResourcesCustomer {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourcesCustomer() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MISA.ApplicationCore.Resources.ResourcesCustomer", typeof(ResourcesCustomer).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mã khách hàng đã tồn tại trong hệ thống..
        /// </summary>
        public static string CustomerCode_Duplicate_ErrorMsg {
            get {
                return ResourceManager.GetString("CustomerCode_Duplicate_ErrorMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mã khách hàng đã trùng với khách hàng khác trong tệp nhập khẩu..
        /// </summary>
        public static string CustomerCode_DuplicateFile_ErrorMsg {
            get {
                return ResourceManager.GetString("CustomerCode_DuplicateFile_ErrorMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mã khách hàng không được để trống!.
        /// </summary>
        public static string CustomerCode_ErrorMsg {
            get {
                return ResourceManager.GetString("CustomerCode_ErrorMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nhóm khách hàng không có trong hệ thống..
        /// </summary>
        public static string GroupName_ErrorMsg {
            get {
                return ResourceManager.GetString("GroupName_ErrorMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SĐT đã có trong hệ thống..
        /// </summary>
        public static string PhoneNumber_Duplicate_ErrorMsg {
            get {
                return ResourceManager.GetString("PhoneNumber_Duplicate_ErrorMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SĐT đã trùng với SĐT của khách hàng khác trong tệp nhập khẩu..
        /// </summary>
        public static string PhoneNumber_DuplicateFile_ErrorMsg {
            get {
                return ResourceManager.GetString("PhoneNumber_DuplicateFile_ErrorMsg", resourceCulture);
            }
        }
    }
}
