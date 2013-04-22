using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
namespace NLibrary
{
    public class NLogger
    {
        public static readonly ILog Logger = LogManager.GetLogger("NLogger");
       

    }
}
