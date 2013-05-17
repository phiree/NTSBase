using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace NLibrary
{
   public class StringHelper
    {
       public static string ReplaceSpace(string input)
       {
           string patern = @"\s*";
           return Regex.Replace(input, patern, string.Empty);
       }
       public static string ReplaceInvalidChaInFileName(string input,string replacement)
       {
           string partern = @"[\\\/\:\'\?\*\<\>\|\n]";
           return Regex.Replace(input, partern, replacement);
       }
       public static string ReplaceInvalidChaInFileName(string input)
       {
           return ReplaceInvalidChaInFileName(input, string.Empty);
       }
       /// <summary>
       /// 保证字符串的字符数量
       /// </summary>
       /// <param name="input"></param>
       /// <param name="exceptLenth">希望的长度</param>
       /// <param name="fillChar"> 填充字符 </param>
       /// <returns></returns>
       public static string EnsureStringLength(string input, int exceptLenth, char fillChar)
       {
           throw new NotImplementedException();
       }
       
    }
}
