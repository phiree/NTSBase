using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NLibrary;
using NUnit.Mocks;
using Rhino.Mocks;
namespace NTest.NLibraryTest
{
    public class FormatSerialNoTest
    {
        public void GetSerialNoTest()
        {
            //IFormatSerialNoPersistent persisten=
            var persistent = MockRepository.GenerateStub<IFormatSerialNoPersistent>();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            dict.Add("1", 4);
            persistent.Stub(x => x.GetAll()).Return(dict);

            persistent = new NDAL.DALFormatSerialNo() as IFormatSerialNoPersistent;

            NLibrary.FormatSerialNoUnit fu = new NLibrary.FormatSerialNoUnit(persistent);
           
           //Assert.AreEqual("100001", fu.GetFormatedSerialNo("1"));
           //Assert.AreEqual("100002", fu.GetFormatedSerialNo("1"));
           //Assert.AreEqual("200001", fu.GetFormatedSerialNo("2"));
            fu.GetFormatedSerialNo("01.032.00003");
            fu.GetFormatedSerialNo("1");
           fu.Save();
        }
    }
}
