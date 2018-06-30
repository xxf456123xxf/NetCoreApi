using System;
using System.Collections.Generic;
using System.Web;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Web;///记得引用这个命名空间
using System.IO;
using System.Text;

using System.Data;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace ApiCoreCommon
{
   public class JsonUtil
    {
        public JsonUtil()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// 把对象序列化 JSON 字符串 
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实体</param>
        /// <returns>JSON字符串</returns>
        public static string GetJson(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 把JSON字符串还原为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="szJson">JSON字符串</param>
        /// <returns>对象实体</returns>
        public static T ParseFormJson<T>(string szJson)
        {
            return (T)JsonConvert.DeserializeObject<T>(szJson);
        }


        #region DataTable 转换为Json 字符串
        /// <summary>
        /// DataTable 对象 转换为Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static String dataTableToJson(DataTable dt)
        {
            return JsonConvert.SerializeObject(dt);

            //JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            //javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            //ArrayList arrayList = new ArrayList();
            //foreach (DataRow dataRow in dt.Rows)
            //{
            //    Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
            //    foreach (DataColumn dataColumn in dt.Columns)
            //    {
            //        dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
            //    }
            //    arrayList.Add(dictionary); //ArrayList集合中添加键值
            //}
            //string json = javaScriptSerializer.Serialize(arrayList);
            //return json;  //返回一个json字符串
        }
        #endregion

        #region Json 字符串 转换为 DataTable数据集合
        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable jsonToDataTable(string json)
        {
            return JsonConvert.DeserializeObject<DataTable>(json);

            //DataTable dataTable = new DataTable();  //实例化
            //DataTable result;
            //try
            //{
            //    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            //    javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            //    ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
            //    if (arrayList.Count > 0)
            //    {
            //        foreach (Dictionary<string, object> dictionary in arrayList)
            //        {
            //            if (dictionary.Keys.Count == 0)
            //            {
            //                result = dataTable;
            //                return result;
            //            }
            //            if (dataTable.Columns.Count == 0)
            //            {
            //                foreach (string current in dictionary.Keys)
            //                {
            //                    dataTable.Columns.Add(current, dictionary[current].GetType());
            //                }
            //            }
            //            DataRow dataRow = dataTable.NewRow();
            //            foreach (string current in dictionary.Keys)
            //            {
            //                dataRow[current] = dictionary[current];
            //            }

            //            dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
            //        }
            //    }
            //}
            //catch
            //{
            //}
            //result = dataTable;
            //return result;
        }
        #endregion

     


      
    }
}
