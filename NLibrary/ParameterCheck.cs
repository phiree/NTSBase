using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLibrary
{
    public class ParameterCheck
    {
        public static bool Check(object parameter,out string errMsg)
        {
            bool result = false;
            errMsg = "参数错误:";
            Type t = parameter.GetType();
            switch (t.Name.ToLower())
            {
                case "string":
                   string  strParameter = (string)parameter;
                   if (string.IsNullOrEmpty(strParameter))
                   { 
                    errMsg="不能为空";
                   }
                    break;
                case "int": break;
                case "datetime": break;
                case "decimal": break;
                default: 
                    
                    break;
            }

            return result;
        }
    }
}
