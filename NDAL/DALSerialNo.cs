using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLibrary;
using NModel;
namespace NDAL
{
    public class DALFormatSerialNo : DalBase<NModel.FormatSerialNo>, IFormatSerialNoPersistent
    {

        public Dictionary<string, int> GetAll()
        {
            Dictionary<string, int> serialDict = new Dictionary<string, int>();
            IList<FormatSerialNo> list = base.GetAll<FormatSerialNo>();

            foreach (FormatSerialNo no in list)
            {
                serialDict.Add(no.SerialKey, no.SerialNo);
            }

            return serialDict;

        }

        public void Save(Dictionary<string, int> newSerialNos)
        {
            using (var t = session.BeginTransaction())
            {
                foreach (KeyValuePair<string, int> serial in newSerialNos)
                {
                    NHibernate.IQueryOver<FormatSerialNo> qo = session.QueryOver<FormatSerialNo>().Where(x => x.SerialKey == serial.Key);
                    FormatSerialNo model = GetOneByQuery(qo);
                    if (model == null)
                    {
                        model = new FormatSerialNo();
                        model.SerialKey = serial.Key;
                        model.SerialNo = serial.Value;
                        Save(model);
                    }
                    else
                    {
                        model.SerialNo = serial.Value;
                        Update(model);
                    }
                }
                t.Commit();
            }
        }
    }
}
