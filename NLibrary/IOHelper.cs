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
            FileInfo fi = new FileInfo(filePath);
           
            string directory = fi.Directory.FullName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                
            }
        }
        public static DirectoryInfo EnsureDirectory(string directory)
        {
            DirectoryInfo dir = new DirectoryInfo(directory);
            if (!dir.Exists)
            {
                dir = Directory.CreateDirectory(directory);

            }
            return dir;
        }
        public static string EnsureFoldEndWithSlash(string directoryPath)
        {
            if (!directoryPath.EndsWith("\\"))
            {
                return directoryPath + "\\";
            }
            else return directoryPath;
        }

    }


    public static class ExtentedMethoed
    {
        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo dir, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");
            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            return files.Where(f => extensions.Contains(f.Extension.ToLower()));
        }
    }
}
