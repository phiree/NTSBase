using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace NBiz
{

    public interface IExcelReader<T>
    {
       
        IList<T> Read(Stream stream,out string readResult);
    }
    public interface IDataTableConverter<T> {
        IList<T> Convert(System.Data.DataTable table);
    }
}
