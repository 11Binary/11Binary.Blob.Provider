using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Shared;
using Zeus.Shared;
using System.Text.Json;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace _11Binary.Blob.Provider
{
    class GetAllStorageAcountsActor :SubscriberActorBase
    {
        Dictionary<string, object> props = new Dictionary<string, object>();
        public GetAllStorageAcountsActor(IZeusClient c, Dictionary<string, object> s)
            : base(c, s)
        {
            _nameSpace = "Olympus.Blob.Provider";
            _entity = "StoraeAccounts";
            _entityVersion = "1.0.0";
            _method = "GetAll";


        }

        public override bool Execute(Mercury.Shared.ReceivedMessage rm)
        {
            //strip all of the values out of the message for the execution of this actor
            _token = rm.Token;
            //The usertoken is the only parameter required. In later actors other parameters  might apply. 
            //ActionRequest ar = JsonConvert.DeserializeObject<ActionRequest>(rm.Message);
            //props = JsonConvert.DeserializeObject<Dictionary<string, object>>(ar.Message.ToString());
           
            //string myresponse = RequestData();
            
            //ActionResponse response = new ActionResponse();
            //response.ContentType = "application/json";
            //rm.To = rm.ReplyTo;
            ////rm.NameSpace = rm.Originator;
            //rm.ReplyTo = _nameSpace;
            //response.Id = rm.MessageId;
            //response.ResponseMessage = myresponse;
            //response.Response = myresponse;
            //response.ResponseMessage = "Success";
            //response.StatusCode = "200";
            //response.OrderNo = 1;
            //response.TotalRecords = 1;
            //rm.Message = response;
            //_client.PublishMessage(null, rm, Zeus.Shared.ConnectionTypes.ConnType.SBQueue);

            return true;
        }


        private string RequestData()
        {
            KeyValuePair<string, object> kvpSubscriptionId = props.Where(p => p.Key == "SubscriptionId").FirstOrDefault();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string SubscriptionId = kvpSubscriptionId.Value.ToString();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            string servicceURL = GetServiceManagementUrl("Azure Cloud");
            servicceURL = servicceURL + SubscriptionId + "/services/storageservices";
            HttpClient request = (HttpClient)HttpWebRequest.Create(servicceURL);
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _token);
            request.Headers.Add("x-ms-version", "2009-10-01");
            request.Method = "GET";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string xml = new StreamReader(response.GetResponseStream()).ReadToEnd();
                


                return xml;

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;

        }
    }
}
