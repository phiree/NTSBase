using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NModel
{
    public class FormatSerialNo
    {
        public virtual Guid Id { get; set; }
        public virtual string SerialKey { get; set; }
        public virtual int SerialNo { get; set; }
    }
}
