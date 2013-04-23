using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NModel;
using NBiz;

namespace NTest.NBizTest
{
    [TestFixture]
    public class CopyImageTest
    {
        [Test]
        public void CopyTest()
        {
            var finfo = new System.IO.FileInfo(Environment.CurrentDirectory+(@"\config\log4net.config"));
            log4net.Config.XmlConfigurator.Configure(finfo);
            ImageCopy imageCopy = new ImageCopy();
            imageCopy.SourcePath = Environment.CurrentDirectory + "\\TestFiles\\Images\\";
            imageCopy.TargetPath = Environment.CurrentDirectory + "\\TestFiles\\TargetImages\\";
            imageCopy.Copy();
        }
    }
}
