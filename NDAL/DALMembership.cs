using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDAL
{
    public class DALMembership:DalBase<NModel.NTSMember>
    {
        public NModel.NTSMember GetByUserName(string username)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUser(string username, string encryptedPwd)
        {
            string query = "select m from NTSMember m where m.Name='" + username
                   + "' and  m.Password='" + encryptedPwd + "'";
            NModel.NTSMember member= GetOneByQuery(query);
            return member != null;
        }
    }
}
