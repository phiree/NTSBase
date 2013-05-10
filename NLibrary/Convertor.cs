using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
namespace NLibrary
{
    public class ObjectConvertor
    {
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                string colNameFromDesc = prop.Description;
                if (string.IsNullOrEmpty(colNameFromDesc)) continue;

                table.Columns.Add(colNameFromDesc, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    string colNameFromDesc = prop.Description;
                    if (string.IsNullOrEmpty(colNameFromDesc)) continue;

                    row[colNameFromDesc] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
