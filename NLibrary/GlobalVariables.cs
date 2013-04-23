using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLibrary
{
    public class GlobalVariables
    {
        /// <summary>
        /// 每次生成之后
        /// </summary>
        public static readonly string SerialNumberFile =AppDomain.CurrentDomain.BaseDirectory + @"\Serial.Number";
        public static readonly string SerialNumberBackupFile = AppDomain.CurrentDomain.BaseDirectory + @"\backup\SerialNumber" + DateTime.Now.ToString("yyyy-MM-dd-hh_mm_ss");
        
        /// <summary>
        /// 测试生成的物料表
        /// </summary>
        public static string ErpXslFileOutTest
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"\测试生成\物料" + DateTime.Now.ToString("yyyy-MM-dd_hh_mm_ss") + ".xls";
            }
        }
        /// <summary>
        /// 正式生成的物料表
        /// </summary>
        public readonly static string ErpXslFileOut = AppDomain.CurrentDomain.BaseDirectory + @"\正式生成\" + DateTime.Now.ToString("yyyy-MM-dd_hh_mm_ss") + @"\物料.xls";
        /// <summary>
        /// Erp物料表标准格式.
        /// </summary>
        public static readonly string ErpXslFileTemplate = AppDomain.CurrentDomain.BaseDirectory + @"\标准格式\ERP物料格式.xls";
        /// <summary>
        /// 供应商列表
        /// </summary>
        public static readonly string XslSupplierList = AppDomain.CurrentDomain.BaseDirectory + @"\标准格式\供应商列表.xls";

    }
}
