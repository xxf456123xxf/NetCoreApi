
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ApiCoreCommon;

namespace CRMAPI.Bll
{
    public class Bal: ApiStatic
    {
        public SqlConnection conn = Settings.CrmSetting.SqlConn;
    }
}
