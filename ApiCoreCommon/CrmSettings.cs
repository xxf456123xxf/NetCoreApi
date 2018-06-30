using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Xml;

namespace ApiCoreCommon
{

    public class Settings
    {
        public static Lazy<CrmSettings> _CrmSetting = new Lazy<CrmSettings>() { };

        public static CrmSettings CrmSetting
        {
            get
            {
                return Settings._CrmSetting.Value;
            }
        }
    }
    public class CrmSettings
    {

        public IConfiguration Configuration { get; }

        public CrmSettings(IConfigurationBuilder builder)
        {
            Configuration = builder.Build();

        }

        /// <summary>
        /// CRM UserName
        /// </summary>
        public string UserName => getSetting("CrmAdmin");
        public string PassWord => getSetting("CrmAdminPwd");
        public string Domain => getSetting("CrmAdminDomain");
        public string IpAddress => getSetting("IpAddress");
        public SqlConnection SqlConn => new SqlConnection(getSetting("StrConn"));
        /// <summary>
        /// 事业部
        /// </summary>
        public Guid BusinessDepartmentId => new Guid(getSetting("BusinessDepartmentId"));
        /// <summary>
        /// 订单类型
        /// </summary>
        public Guid OrdertypeId => new Guid(getSetting("OrdertypeId"));
        /// <summary>
        /// 货币
        /// </summary>
        public Guid CurrencyId => new Guid(getSetting("CurrencyId"));
        ///// <summary>
        ///// 售后原因细分
        ///// </summary>
        //public Guid ReasonDetailId => new Guid(getSetting("ReasonDetailId"));

        public string crmConfingerUrl => Configuration["crmConfingerUrl"];
        public string getSetting(string name)
        {
            string path = crmConfingerUrl;
            //如果配置文件中存在 则从配置文件中取
            if (string.IsNullOrEmpty(path) || Configuration["Crm:" + name] != null)
            {
                return Configuration["CRM:" + name];
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlNode root = xmlDoc.SelectSingleNode("CRMConnect");
                XmlNode node = root.SelectSingleNode(name);

                if (!string.IsNullOrEmpty(node.InnerText))
                    return node.InnerText;
                else
                    return string.Empty;
            }
        }
    }
    public static class CrmConfigurationProvider
    {
        public static IConfigurationBuilder AddCrmSettingsConfig(this IConfigurationBuilder builder)
        {

            Settings._CrmSetting = new Lazy<CrmSettings>(() =>
            {
                CrmSettings appSettings = new CrmSettings(builder);
                // builder.Build().GetSection("Crm").Bind(appSettings);
                return appSettings;
            });

            return builder;
        }
    }
}
