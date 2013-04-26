using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NLibrary;
namespace NBiz
{
    /// <summary>
    /// 流水号管理
    /// </summary>
    public class SerialNumberManager
    {
        Dictionary<string, int> originalSerialList = new Dictionary<string, int>();


        public SerialNumberManager()
        {
            ReadSerialNumberFile();
        }
        public void ReadSerialNumberFile()
        {
            string[] serialNumberLines = IOHelper.ReadAllLinesFromFile(GlobalVariables.SerialNumberFile);
            foreach (string line in serialNumberLines)
            {
                if (line.StartsWith("#")) continue;
                string[] values = line.Split('|');
                if (values.Length != 2)
                {
                    throw new Exception("格式有误." + line);
                }
                string key = values[0];
                string value = values[1];
                int number;
                if (!int.TryParse(value, out number))
                {
                    throw new Exception("序列号格式有误,应该是数字." + line);

                }
                originalSerialList.Add(key, number);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void WriteSerialNumberFile()
        {
            //备份上次上次的文件
            IOHelper.EnsureFileDirectory(GlobalVariables.SerialNumberBackupFile);
            File.Copy(GlobalVariables.SerialNumberFile, GlobalVariables.SerialNumberBackupFile,true);
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, int> serialnumber in originalSerialList)
            {
                string line = serialnumber.Key + "|" + serialnumber.Value;
                sb.AppendLine(line);
            }
            File.WriteAllText(GlobalVariables.SerialNumberFile, sb.ToString());
        }
        public int GetSerialNo(string baseNumber, bool test)
        {
            if (test)
            {
                if (!originalSerialList.ContainsKey(baseNumber))
                {
                    return 1;
                }
                else
                {

                    return originalSerialList[baseNumber];

                }
            }
            else
            {

                if (!originalSerialList.ContainsKey(baseNumber))
                {
                    originalSerialList.Add(baseNumber, 1);
                }
                else
                {

                    originalSerialList[baseNumber] = originalSerialList[baseNumber] + 1;

                }
            }
            return originalSerialList[baseNumber];
        }
        public string GetFormatedSerialNo(string catelogCode, string suppierCode, bool isTest)
        {
            string baseNumber = catelogCode + "." + suppierCode;
            //获取当前分类和当前供应商的最大编码
            int serialNumber = GetSerialNo(baseNumber, isTest);
            //新编码
            string newSerialNumber = "0000" + serialNumber;
            newSerialNumber = newSerialNumber.Substring(newSerialNumber.Length - 5, 5);
            string ntsCode = baseNumber + newSerialNumber;
            return ntsCode;
        }
    }
}
