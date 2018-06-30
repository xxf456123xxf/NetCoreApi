using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace ApiCoreCommon
{
    public class StringUtil
    {
        #region String[] 转换为 String 字符串
        /// <summary>
        /// String 转换为 sql 字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string getString(string[] value)
        {
            if (value.Length == 0) return "";
            string result = "";
            for (int i = 0; i < value.Length; i++)
            {
                result += "," + value[i];
            }
            return result.Substring(1);
        }
        #endregion

        #region 编号1
        /// <summary>
        /// 编号1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string getDetailid()
        {
            DateTime dt = DateTime.Now;
            return string.Format("{0:yyyyMMddHHmmss}", dt);
        }
        public static string getDetailid(string time, int idx)
        {
            return time + (10000 + idx).ToString();
        }
        public static string getActDetailid()
        {
            DateTime dt = DateTime.Now;
            Random ro = new Random((int)DateTime.Now.Ticks);
            string s = ro.Next().ToString();
            return string.Format("{0:yyyyMMddHHmmss}", dt) + s.Substring(s.Length - 3, 3);
        }
        #endregion

        #region String 转换为 sql 字符串
        /// <summary>
        /// String 转换为 sql 字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String replaceSql(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            value = value.Replace("'", "''");
            return value;
        }
        #endregion

        #region String Md5加密
        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String Md5(string value)
        {
            //return FormsAuthentication.HashPasswordForStoringInConfigFile(value, "MD5");
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString().ToUpper();
        }
        #endregion

        #region 转换为string字符串类型
        /// <summary>
        ///  转换为string字符串类型
        /// </summary>
        /// <param name="s">获取需要转换的值</param>
        /// <param name="format">需要格式化的位数</param>
        /// <returns>返回一个新的字符串</returns>
        public static string ToStr(object s, string format)
        {
            string result = "";
            try
            {
                if (format == "")
                {
                    result = s.ToString();
                }
                else
                {
                    result = string.Format("{0:" + format + "}", s);
                }
            }
            catch
            {
            }
            return result;
        }
        #endregion
        #region 转换为string字符串 Sql 类型
        private static String rep_json(String value)
        {
            if (value == null)
            {
                value = "";
            }
            else
            {
                value = value.Replace("\"", "\\\"");
                value = value.Replace("\n", "");
            }
            return value;
        }
        #endregion


        #region 获取文件上传路径
        //public static string getFileUploadName(string filename, bool isName = false)
        //{

        //    string fileExt = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
        //    //uploadfile//当前日期//8随机数.fileExt
        //    DateTime dt = DateTime.Now;
        //    string d1 = string.Format("{0:yyyyMMdd}", dt);
        //    string d2 = string.Format("{0:HHmmssffff}", dt);
        //    string uploadFilePath = "//uploadfile//" + d1 + "//";
        //    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(HttpContext.Current.Request.MapPath("~/") + uploadFilePath);
        //    //判断文件夹否存在,不存在则创建
        //    if (!dir.Exists)
        //    {
        //        dir.Create();
        //    }

        //    Random ro = new Random(Guid.NewGuid().GetHashCode());
        //    if (isName)
        //    {
        //        d2 = getFileUploadNameCaption(filename) + d2;
        //    }
        //    return uploadFilePath + d2 + ro.Next().ToString() + "." + fileExt;
        //}
        #endregion
        //public static string getFileUploadName(string filename, string fileurl, bool isName = false)
        //{

        //    string fileExt = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
        //    //uploadfile//当前日期//8随机数.fileExt
        //    DateTime dt = DateTime.Now;
        //    //string d1 = string.Format("{0:yyyyMMdd}", dt);
        //    string d2 = string.Format("{0:HHmmssffff}", dt);
        //    string uploadFilePath = "//uploadfile//" + fileurl + "//";
        //    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(HttpContext.Current.Request.MapPath("~/") + uploadFilePath);
        //    //判断文件夹否存在,不存在则创建
        //    if (!dir.Exists)
        //    {
        //        dir.Create();
        //    }

        //    Random ro = new Random(Guid.NewGuid().GetHashCode());
        //    if (isName)
        //    {
        //        d2 = getFileUploadNameCaption(filename)+ d2;
        //    }
        //    return uploadFilePath + d2 + ro.Next().ToString() + "." + fileExt;
        //}
        #region 获取文件上传文件标题
        public static string getFileUploadNameCaption(string filename)
        {
            return filename.Substring(0, filename.LastIndexOf("."));
        }
        #endregion
        #region 处理名字中的特殊字符
        public static string getFileUploadNameDispose(string filename)
        {
            return filename.Replace("+", "%2B").Replace("/", "@2F").Replace("?", "%3F").Replace("%", "%25").Replace("#", "%23").Replace("&", "%26");
        }
        #endregion


    }
}
