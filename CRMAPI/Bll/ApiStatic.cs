



using System;
using System.Security.Claims;
using ApiCoreCommon;
namespace CRMAPI.Bll
{
    public class ApiStatic
    {

        public Microsoft.Xrm.Sdk.IOrganizationService service(Guid? userid = null)
        {

            crmService Service = new crmService();
            if (userid != null)
            {

                return Service.GetOrganizationService(userid.Value);
            }
            return Service.GetOrganizationService();
        }
        public Microsoft.Xrm.Sdk.IOrganizationService service(string username, string password)
        {

            crmService Service = new crmService();

            return Service.GetOrganizationService(username, password);
        }
    }
}