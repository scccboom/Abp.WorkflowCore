using Abp.Runtime.Validation;
using Abp.UI.Inputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.UI.Inputs
{
    /// <summary>
    /// 
    /// </summary>

    [Serializable]
    [InputType("SELECT_ROLES")]
    public class SelectRoleInputType : InputTypeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public SelectRoleInputType()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validator"></param>
        public SelectRoleInputType(IValueValidator validator)
            : base(validator)
        {
        }
    }
}
