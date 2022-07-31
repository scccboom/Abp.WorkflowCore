using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.UI.Inputs
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [InputType("SELECT_USERS")]
    public class SelectUserInputType : InputTypeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public SelectUserInputType()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validator"></param>
        public SelectUserInputType(IValueValidator validator)
            : base(validator)
        {
        }
    }
}
