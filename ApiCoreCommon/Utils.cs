using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System;
using System.IO;

namespace ApiCoreCommon
{
    public class Utils
    {
        public static string RequestGetService(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    HttpWebRequest myReq =
                      (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
                    // Get the stream associated with the response.
                    Stream receiveStream = response.GetResponseStream();

                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    return readStream.ReadToEnd();
                }
               
            }
            catch (Exception ex)
            {



                throw new Exception(url + "//" + ex.Message);
            }

        }
        /// <summary>
        /// 请求接口服务(Post)
        /// </summary>
        /// <returns></returns>
        public static string RequestPostService(string url, string json, string ContentType = "application/json")
        {
            try
            {


                string requestStr = "";
                using (WebClient client = new WebClient())
                {
                    byte[] requestData = Encoding.GetEncoding("UTF-8").GetBytes(json);
                    client.Headers.Add("Content-Type", ContentType);
                    client.Headers.Add("ContentLength", requestData.Length.ToString());
                    byte[] responseData = client.UploadData(url, "POST", requestData);
                    requestStr = Encoding.GetEncoding("UTF-8").GetString(responseData);
                }
                return requestStr;
            }
            catch (Exception ex)
            {



                throw new Exception(url + "//" + ex.Message);
            }

        }


    }
}
