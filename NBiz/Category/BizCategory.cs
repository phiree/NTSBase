using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NModel;
namespace NBiz
{
    public class BizCategory:BLLBase<Category>
    {
        /// <summary>
        /// stream 相对于 filepath的好处: filepath只能是服务器上的物理文件路径; stream可以是客户端文件信息(比如 fileupload 的 PostFileStream属性)
        /// </summary>
        /// <param name="stream"></param>
        public void ImportCategoryFromExcel(System.IO.Stream stream)
        {
            IExcelReader<Category> CategoryReader = new CategoryExcelReader();
            ImportToDatabaseFromExcel<Category> importor = new ImportToDatabaseFromExcel<Category>(CategoryReader, this);
            importor.Import(stream);
        }
    }
}
