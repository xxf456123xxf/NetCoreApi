using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Crm.Sdk.Messages;
using System.ServiceModel.Description;
using System.Net;
using System.Security.Claims;


namespace ApiCoreCommon
{
    public class crmService
    {
        string name = Settings.CrmSetting.UserName;
        string password = Settings.CrmSetting.PassWord;
        string domain = Settings.CrmSetting.Domain;

        public Uri uri
        {
            get
            {

                string ipAddress = Settings.CrmSetting.IpAddress;
                return new Uri(ipAddress + "/XRMServices/2011/Organization.svc");
            }
        }
        //实例化OrganizationService
        public IOrganizationService GetOrganizationService()
        {

            System.ServiceModel.Description.ClientCredentials credentials = new ClientCredentials();
            if (uri.Scheme == "https")
            {
                name = domain + "\\" + name;
                credentials.UserName.UserName = name;
                credentials.UserName.Password = password;

            }
            else
            {
                credentials.Windows.ClientCredential = new System.Net.NetworkCredential(name, password, domain);
            }


            OrganizationServiceProxy foa = new OrganizationServiceProxy(uri, null, credentials, null);

            IOrganizationService service = (IOrganizationService)foa;

            return service;
        }
        public IOrganizationService GetOrganizationService(string name, string password)
        {


            System.ServiceModel.Description.ClientCredentials credentials = new ClientCredentials();

            if (uri.Scheme == "https")
            {
                name = domain + "\\" + name;
                credentials.UserName.UserName = name;
                credentials.UserName.Password = password;

            }
            else
            {
                credentials.Windows.ClientCredential = new System.Net.NetworkCredential(name, password, domain);
            }
            OrganizationServiceProxy foa = new OrganizationServiceProxy(uri, null, credentials, null);
            IOrganizationService service = (IOrganizationService)foa;

            return service;
        }
        //实例化OrganizationService
        public IOrganizationService GetOrganizationService(Guid UserId)
        {

            System.ServiceModel.Description.ClientCredentials credentials = new ClientCredentials();
            if (uri.Scheme == "https")
            {
                name = domain + "\\" + name;
                credentials.UserName.UserName = name;
                credentials.UserName.Password = password;

            }
            else
            {
                credentials.Windows.ClientCredential = new System.Net.NetworkCredential(name, password, domain);
            }
            OrganizationServiceProxy foa = new OrganizationServiceProxy(uri, null, credentials, null);
            foa.CallerId = UserId;
            IOrganizationService service = (IOrganizationService)foa;
            return service;

        }
        public IOrganizationService GetOrganizationService(ClientCredentials credentials)
        {

            OrganizationServiceProxy foa = new OrganizationServiceProxy(uri, null, credentials, null);
            IOrganizationService service = (IOrganizationService)foa;
            return service;
        }

    }
}
