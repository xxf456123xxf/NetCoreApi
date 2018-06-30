using ApiCoreCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
namespace CRMAPI.Bll
{
    public class EntityData : Bal
    {
        public dynamic getOptions(List<string> name)
        {
            using (var db = conn.OpenConnection())
            {

                var values = db.Query(@"SELECT l.Label label,a.Value value,o.Name name
                        FROM dbo.LocalizedLabelView AS l
                        JOIN dbo.AttributePicklistValueView AS a
                        ON a.AttributePicklistValueId = l.ObjectId
                        JOIN dbo.OptionSetView AS o
                        ON o.OptionSetId = a.OptionSetId
                        WHERE o.Name in @name and l.Label != ''", new { name = name.ToArray() });
                Dictionary<string, List<dynamic>> keyValuePairs = new Dictionary<string, List<dynamic>>();

                foreach (var item in name)
                {
                    keyValuePairs[item] = values.Where(a => a.name == item).Select(a => new { a.label, a.value }).ToList<dynamic>();
                }
                return keyValuePairs;
            }

        }




    }
}
