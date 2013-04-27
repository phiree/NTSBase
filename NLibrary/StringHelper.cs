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
    }
}
