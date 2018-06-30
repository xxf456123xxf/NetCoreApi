
using ApiCoreCommon.Interface;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ApiCoreCommon
{
    public class crmFetchKit
    {
        public IOrganizationService service { get; set; }
        public DataCollection<Entity> entities;
        public int count { get; set; }
        public EntityCollection returnCollection { get; set; }
        public crmFetchKit(IOrganizationService service)
        {
            this.service = service;
        }
        public DataCollection<Entity> Fetch(string fetchXml)
        {
            FetchExpression fetch = new FetchExpression(fetchXml.ToLower());
            this.returnCollection = service.RetrieveMultiple(fetch);
            this.entities = this.returnCollection.Entities;
            this.count = this.entities.Count;
            return this.entities;
        }
        public Entity GetById(string EntityName, Guid EntityId, string[] columns)
        {
            ColumnSet column = new ColumnSet(columns);
            return service.Retrieve(EntityName, EntityId, column);

        }
        public DataCollection<Entity> FetchALL(string fetchXml)
        {

            var collection = new EntityCollection();
            int PageNumber = 1;
            string PagingCookie = "";

            EntityCollection tempCollection;
            do
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(fetchXml);
                XmlAttribute page = doc.CreateAttribute("page");
                page.Value = PageNumber.ToString();
                doc.FirstChild.Attributes.Append(page);
                XmlAttribute cookie = doc.CreateAttribute("paging-cookie");
                cookie.Value = PagingCookie;
                doc.FirstChild.Attributes.Append(cookie);
                FetchExpression fetch = new FetchExpression(doc.InnerXml.ToLower());
                tempCollection = service.RetrieveMultiple(fetch);

                PageNumber++;
                PagingCookie = tempCollection.PagingCookie;
                collection.Entities.AddRange(tempCollection.Entities);
            }
            while (tempCollection.MoreRecords);


            collection.MoreRecords = false;
            collection.TotalRecordCount = collection.Entities.Count;
            this.returnCollection = collection;
            this.count = collection.Entities.Count;
            this.entities = this.returnCollection.Entities;
            return entities;

        }
        public List<T> toObject<T>() where T : new()
        {
            var list = new List<T>();
            do
            {
                if (this.entities == null || this.entities.Count == 0)
                {
                    break;

                }
                foreach (var item in this.entities)
                {
                    IEntity entity = (IEntity)new T();

                    list.Add((T)item.CrmDataToObject(entity));
                }

            } while (false);
            return list;
        }

    }


}
