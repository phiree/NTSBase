using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace NLibrary
{
    public class IOHelper
    {

        public static string[] ReadAllLinesFromFile(string filePath)
        {

            FileInfo file = EnsureFile(filePath);
            string[] result = File.ReadAllLines(filePath);

            return result;
        }
        /// <summary>
        /// 保证文件存在
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static FileInfo EnsureFile(string filepath)
        {
            FileInfo fi = new FileInfo(filepath);
            if (fi.Exists)
            {
                return fi;
            }
            EnsureFileDirectory(filepath);

            FileStream fs = fi.Create();
            fs.Close();
            return fi;


        }

        /// <summary>
        /// 保证路径中的目录都存在
        /// </summary>
        /// <param name="filePath"></param>
        public static void EnsureFileDirectory(string filePath)
        {
            string directory = Path.GetFullPath(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                
            }
        }
    }

    public class TextHelper
    { 
     
    }
}
