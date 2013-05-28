using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLibrary
{
    /// <summary>
    /// 格式化流水号
    /// </summary>
    public class FormatSerialNoUnit
    {
        /// <summary>
        /// 需要增加流水号的序列对象.
        /// </summary>
        Dictionary<string, int> SerialList = new Dictionary<string, int>();
        IFormatSerialNoPersistent persistent;
        
       
        public FormatSerialNoUnit(IFormatSerialNoPersistent persistent)
        {
            this.persistent = persistent;
            SerialList = persistent.GetAll();
        }
        /// <summary>
        /// 获取某序列对象的流水号
        /// </summary>
        /// <returns></returns>
        private int GetSerialNo(string serialKey)
        {
            int newSerialNo = 1;
            if (SerialList.ContainsKey(serialKey))
            {
                newSerialNo = SerialList[serialKey] + 1;
                SerialList[serialKey] = newSerialNo;
            }
            else
            {
                SerialList.Add(serialKey, newSerialNo);
            }
            return newSerialNo;
        }
        public string GetFormatedSerialNo(string serialKey)
        {
            return GetFormatedSerialNo(serialKey, "00000");
        }
        private string GetFormatedSerialNo(string serialKey, string format)
        {
            int newNo = GetSerialNo(serialKey);
            string rawNo = format + newNo;
            string strSerialNo = rawNo.Substring(rawNo.Length - format.Length, format.Length);
            return serialKey + strSerialNo;
        }
        public void Save()
        {
            persistent.Save(SerialList);
        }
    }
    public interface IFormatSerialNoPersistent
    {
        Dictionary<string, int> GetAll();
        void Save(Dictionary<string, int> newSerialNos);
    }
}
