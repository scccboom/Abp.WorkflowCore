﻿using System;

namespace WorkflowDemo.Workflow
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class UInputTypeAttribute : System.Attribute
    {
        public Type InputType { get; set; }

        public string DisplayName { get; set; }

        public object DefaultValue { get; set; }

        public UInputTypeAttribute(Type inputType, string displayName, object defaultValue = null)
        {
            this.DisplayName = displayName;
            this.InputType = inputType;
            this.DefaultValue = defaultValue;
        }
    }
}