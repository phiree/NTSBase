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
       
    }
}
