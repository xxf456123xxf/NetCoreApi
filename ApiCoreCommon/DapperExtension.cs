using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCoreCommon
{
    public static class DapperExtension
    {
        public static IEnumerable<dynamic> ToPageList(this IDbConnection cnn, string sqlCount, string sqlList, int page, int pagesize, string orderby, ref int total, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            int startn = (page - 1) * pagesize + 1;
            int endn = page * pagesize;
            string sqlPage = string.Format("select  *  from (  select row_number() over (order by {2}) as rownumber,* from ({3}) m ) b   where rownumber  between {0} and  {1} order by rownumber", startn, endn, orderby.Replace("order by", ""), sqlList);
            if (page == 0 && pagesize == 0) return cnn.Query(sqlList, param, transaction, buffered, commandTimeout, commandType);
            total = cnn.QuerySingle<int>(sqlCount, param, transaction, commandTimeout, commandType);



            return cnn.Query(sqlPage, param, transaction, buffered, commandTimeout, commandType);

        }
    }
}
