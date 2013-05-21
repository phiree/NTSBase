using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
using NDAL;
namespace NBiz
{
    public class BizCategory:BLLBase<Category>
    {

        DALCategory _dalCategory;
        public DALCategory DalCategory
        {
            get
            {
                if (_dalCategory == null)
                {
                    _dalCategory = new DALCategory();
                }
                return _dalCategory;
            }
            set {
                _dalCategory = value;
            }
        }
        /// <summary>
        /// stream 相对于 filepath的好处: filepath只能是服务器上的物理文件路径; stream可以是客户端文件信息(比如 fileupload 的 PostFileStream属性)
        /// </summary>
        /// <param name="stream"></param>
        public void ImportCategoryFromExcel(System.IO.Stream stream,out string errMsg)
        {
            IDataTableConverter<Category> CategoryReader = new CategoryDataTableConverter();
            ImportToDatabaseFromExcel<Category> importor = new ImportToDatabaseFromExcel<Category>(CategoryReader, this);
        
           importor.ImportXslData(stream, out errMsg);
        }
        public IList<Category> ReadListFromExcel(System.IO.Stream stream, out string errMsg)
        {
            IDataTableConverter<Category> CategoryReader = new CategoryDataTableConverter();
            ImportToDatabaseFromExcel<Category> importor = new ImportToDatabaseFromExcel<Category>(CategoryReader, this);
            return importor.ReadList(stream, out  errMsg);
        }
        public override IList<Category> SaveList(IList<Category> list, out string errMsg)
        {
            errMsg = string.Empty;
            base.SaveList(list);
            return list;
            
        }
        public Category GetOneByCode(string code)
        {
            return  DalCategory.GetOneByCode(code);
        }
        public Category GetOneByCodes(string code, string parentCode)
        {
           return DalCategory.GetOneByCodes(code,parentCode);
        }
        public string GetCateName(string code)
        {
            string[] cates = code.Split('.');
            if (cates.Length != 2)
            {
                return string.Empty;
            }
            string cateCode=cates[1];
            string parentCode = cates[0];
            string name = GetOneByCodes(cateCode, parentCode).Name;
            string parentName = GetOneByCode(parentCode).Name;
            return parentName + "." + name;
        }
    }
}
