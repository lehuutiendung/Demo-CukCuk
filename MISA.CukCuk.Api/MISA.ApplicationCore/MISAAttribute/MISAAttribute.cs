using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.MISAAttribute
{
    /// <summary>
    /// Custom các attribute cho property
    /// </summary>
    [AttributeUsage (AttributeTargets.Property)]
    public class MISARequired : Attribute
    {
        public string FieldName = string.Empty;

        public MISARequired(string fieldName)
        {
            FieldName = fieldName;
        }
    }

    /// <summary>
    /// Không map với các trường có trên database
    /// </summary>
    public class MISANotMap : Attribute
    {

    }
}
