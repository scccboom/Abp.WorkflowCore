using System;
using System.Collections.Generic;

namespace WorkflowDemo.Workflows
{
    public class WorkflowParamDictionary : Dictionary<string, WorkflowParam>
    {
        public void Add(WorkflowParam param)
        {
            if (this.ContainsKey(param.Name))
            {
                throw new Exception($"'{param.Name}' has Contain!");
            }
            this[param.Name] = param;
        }
    }
}