
using ApiCoreCommon.Interface;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiCoreCommon
{
    public static class crmEntity
    {

        /// <summary>
        /// 获取属性值，获取不到值是抛出异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="entityAttr"></param>
        /// <returns></returns>
        public static T GetEntAttr<T>(this Entity entity, string entityAttr)
        {
            bool isName = false;
            if (entityAttr.EndsWith("$name"))
            {
                isName = true;
                entityAttr = entityAttr.Replace("$name", "");
            }
            //判断是否存在该属性
            if (!entity.Contains(entityAttr) || entity[entityAttr] == null)
            {
                //不包含属性 也没有设置默认值 弹出异常
                throw new Exception(string.Format("属性{0}不存在", entityAttr));
            }
            object temp = entity[entityAttr];
            object returnValue = getValAttr(entity, temp, entityAttr, isName);
            return (T)(Object)returnValue;
        }
        public static T GetEntAttr<T>(this Entity entity, string entityAttr, T DefaultValue)
        {
            bool isName = false;

            if (DefaultValue is String || entityAttr.IndexOf("$name") > 0)
            {
                isName = true;
                entityAttr = entityAttr.Replace("$name", "");
            }
            if (!entity.Contains(entityAttr) || entity[entityAttr] == null)
            {
                return DefaultValue;
            }
            object temp = entity[entityAttr];

            object returnValue = getValAttr(entity, temp, entityAttr, isName);
            if (returnValue == null)
            {
                return DefaultValue;
            }
            return (T)(Object)returnValue;
        }

        public static T GetEntityAttr<T>(this IOrganizationService ent, string LogicalName, dynamic Condition, string Columns, T defalutObject)
        {

            T Result = (T)defalutObject;

            QueryExpression qe = new QueryExpression(LogicalName);
            Type parameterType = Condition.GetType();
            PropertyInfo[] Properties = parameterType.GetProperties();
            foreach (var item in Properties)
            {
                ConditionExpression ce = new ConditionExpression(item.Name, ConditionOperator.Equal, item.GetValue(Condition));
                qe.Criteria.Conditions.AddRange(ce);
            }

            qe.ColumnSet.AddColumns(LogicalName + "id");
            if (!string.IsNullOrEmpty(Columns))
            {
                qe.ColumnSet.AddColumns(Columns);
            }
            EntityCollection entityCollection = ent.RetrieveMultiple(qe);
            if (entityCollection.Entities.Count > 0)
            {

                if (!string.IsNullOrEmpty(Columns))
                {
                    Result = (T)(object)entityCollection[0].GetEntAttr<object>(Columns, defalutObject);

                }
            }
            return Result;
        }
        public static void valType(this Entity entity, CrmType type, string entityAttr, object entityValue, string entityName = "")

        {
            //属性值为空 不处理
            if (entityValue == null)
            {
                return;
            }
            if (entityValue is string && string.IsNullOrEmpty(entityValue.ToString()))
            {
                entity[entityAttr] = null;
                return;

            }
            //判断类型为其赋值
            switch (type)
            {
                case CrmType.MainId:

                    entity.Id = Guid.Parse(entityValue.ToString());
                    break;
                case CrmType.LookUp:
                    var id = Guid.Empty;
                    if (entityValue is string)
                    {
                        id = new Guid(entityValue.ToString());
                    }
                    else if (entityValue is Guid)
                    {
                        id = (Guid)entityValue;
                    }
                    if (id != Guid.Empty)
                    {
                        entity[entityAttr] = new EntityReference(entityName, id);
                    }

                    break;
                case CrmType.OptionValue:
                    entity[entityAttr] = new OptionSetValue(Convert.ToInt32(entityValue));
                    break;
                case CrmType.Int:
                    entity[entityAttr] = Convert.ToInt32(Convert.ToDouble(entityValue));
                    break;
                case CrmType.Money:
                    entity[entityAttr] = new Money(Convert.ToDecimal(entityValue));
                    break;
                case CrmType.Double:
                    entity[entityAttr] = Convert.ToDouble(entityValue);
                    break;
                case CrmType.Decimal:
                    entity[entityAttr] = Convert.ToDecimal(entityValue);
                    break;
                case CrmType.String:
                    entity[entityAttr] = Convert.ToString(entityValue);
                    break;
                case CrmType.DateTime:
                    DateTime date = DateTime.MinValue;
                    if (entityValue is DateTime)
                    {
                        date = (DateTime)entityValue;
                        if ((DateTime)entityValue == DateTime.MinValue)
                        {
                            break;
                        }
                    }
                    else
                    {
                        date = Convert.ToDateTime(entityValue);

                    }
                    entity[entityAttr] = date.ToUniversalTime();
                    break;
                case CrmType.Boolean:
                    //特殊处理bool  0 1也可表示bool
                    int result = 0;
                    if (int.TryParse(entityValue.ToString(), out result))
                    {
                        entity[entityAttr] = Convert.ToBoolean(result);
                    }
                    else
                    {
                        entity[entityAttr] = Convert.ToBoolean(entityValue);
                    }
                    break;
            }
        }
        public static void valType(this Entity entity, CrmType type, string entityAttr, object entityValue, string nullMessage, string entityName = "")
        {
            //必填属性属性值为空 异常
            if (entityValue == null)
            {
                throw new NullReferenceException(nullMessage + "参数不能为空");

            }
            entity.valType(type, entityAttr, entityValue, entityName);
        }
        public static IEntity CrmDataToObject(this Entity ent, IEntity iEntity)
        {
            var propertys = iEntity.GetType().GetProperties().Where(a => a.GetCustomAttributes(true).Count() > 0).ToList();

            iEntity.id = ent.Id;
            foreach (PropertyInfo p in propertys)
            {
                object[] o = p.GetCustomAttributes(true);
                CRMInformation crminfor = new CRMInformation();
                crminfor = (CRMInformation)o[0];

                if (crminfor.method == methods.Set || (crminfor.Type == CrmType.MainId && crminfor.GetName == null))
                {
                    continue;
                }

                string crmAttribute = crminfor.Name;
                if (crminfor.GetName != null)
                {
                    crmAttribute = crminfor.GetName;
                }

                var value = ent.GetEntAttr(crmAttribute, crminfor.GetDefaultValue);

                value = ConvertType(p.PropertyType.Name, value);
                p.SetValue(iEntity, value);
            }
            return iEntity;
        }
        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entity"></param>
        public static void Disable(this IOrganizationService service, Entity entity)
        {

            if (entity == null || entity.Id == Guid.Empty)
            {
                return;
            }
            OptionSetValue state = new OptionSetValue();
            OptionSetValue status = new OptionSetValue();
            state.Value = 1;
            status.Value = 2;

            EntityReference moniker = new EntityReference(entity.LogicalName, entity.Id);

            OrganizationRequest request = new OrganizationRequest() { RequestName = "SetState" };
            request["EntityMoniker"] = moniker;
            request["State"] = state;
            request["Status"] = status;
            service.Execute(request);

        }
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entity"></param>
        public static void Enable(this IOrganizationService service, Entity entity)
        {

            if (entity == null || entity.Id == Guid.Empty)
            {
                return;
            }
            OptionSetValue state = new OptionSetValue();
            OptionSetValue status = new OptionSetValue();
            state.Value = 0;
            status.Value = 1;

            EntityReference moniker = new EntityReference(entity.LogicalName, entity.Id);

            OrganizationRequest request = new OrganizationRequest() { RequestName = "SetState" };
            request["EntityMoniker"] = moniker;
            request["State"] = state;
            request["Status"] = status;
            service.Execute(request);

        }
        /// <summary>
        /// 批量执行
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ents"></param>
        public static void BatchExecute(this IOrganizationService service, OrganizationRequestCollection ents)
        {
            if (ents == null) return;
            List<dynamic> list = new List<dynamic>();
            try
            {
                foreach (var item in ents)
                {
                    if (item is CreateRequest)
                    {
                        CreateResponse resp = (CreateResponse)service.Execute((CreateRequest)item);
                        list.Add(new { id = resp.id, name = ((CreateRequest)item).Target.LogicalName });

                    }
                    else
                    {
                        service.Execute(item);
                    }
                }


            }
            catch (Exception ex)
            {
                //删除创建的记录
                foreach (var item in list)
                {
                    service.Delete(item.name, item.id);
                }

                throw new Exception(ex.Message);
            }

        }
        private static object getValAttr(Entity entity, object temp, string entityAttr, bool isName)
        {
            object returnValue = temp;
            //判断取值的类型
            if (temp is EntityReference)
            {
                if (isName)
                {
                    returnValue = (temp as EntityReference).Name;
                }
                else
                {
                    returnValue = (temp as EntityReference).Id;
                }

            }
            else if (temp is OptionSetValue)
            {
                if (isName)
                {
                    returnValue = entity.FormattedValues[entityAttr];

                }
                else
                {
                    returnValue = (temp as OptionSetValue).Value;
                }

            }
            else if (temp is Money)
            {
                returnValue = (temp as Money).Value;
            }
            else if (temp is DateTime)
            {
                returnValue = ((DateTime)temp).ToLocalTime();
            }
            if (temp is AliasedValue)
            {
                returnValue = crmEntity.getValAttr(entity, (temp as AliasedValue).Value, entityAttr, isName);
            }
            return returnValue;
        }

        private static object ConvertType(string type, object obj)
        {
            type = type.ToLower();
            if (obj != null && obj.GetType().Name.ToLower() == type)
            {
                return obj;
            }
            switch (type)
            {
                case "guid":
                    if (obj == null)
                    {
                        obj = Guid.Empty;
                        break;
                    }
                    obj = new Guid(obj.ToString());
                    break;
                case "double":
                    if (obj == null)
                    {
                        obj = default(double);
                        break;
                    }
                    obj = Convert.ToDouble(obj);
                    break;
                case "int":
                    if (obj == null)
                    {
                        obj = default(Int32);
                        break;
                    }
                    obj = Convert.ToInt32(obj);
                    break;
                case "decimal":
                    if (obj == null)
                    {
                        obj = default(decimal);
                        break;
                    }
                    obj = Convert.ToDecimal(obj);
                    break;
                case "datetime":
                    if (obj == null)
                    {
                        obj = default(DateTime);
                        break;
                    }
                    obj = Convert.ToDateTime(obj);
                    break;
                case "boolean":
                    if (obj == null)
                    {
                        obj = default(Boolean);
                        break;
                    }
                    obj = Convert.ToBoolean(obj);
                    break;
                case "string":
                    if (obj == null)
                    {
                        break;
                    }
                    obj = Convert.ToString(obj);
                    break;
                default:
                    break;
            }
            return obj;

        }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class CRMInformation : Attribute
    {
        public string Name { get; set; }

        public CrmType Type { get; set; }

        public object DefaultValue { get; set; }
        //Type为LookUp有用
        public string EntityName { get; set; }

        public string EntityId { get; set; }
        public methods method { get; set; }
        //GET
        /// <summary>
        /// 获取值（默认Name）
        /// </summary>
        public string GetName { get; set; }
        /// <summary>
        /// 获取不到时默认值（可选）
        /// </summary>
        public Object GetDefaultValue { get; set; }
    }
    public enum CrmType
    {
        Null, MainId, LookUp, OptionValue, Int, Money, Double, String, DateTime, Boolean, Decimal
    }
    public enum methods
    {
        All, Get, Set
    }
}
