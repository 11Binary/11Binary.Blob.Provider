using Mercury.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Zeus.Shared;

namespace _11Binary.Blob.Provider
{
    class SubscriberActorBase : Mercury.Shared.ISubscriberActor
    {
        protected IZeusClient _client;
        protected Dictionary<string, object> _settings = new Dictionary<string, object>();
        protected string _nameSpace = null;
        protected string _entity = null;
        protected string _method = null;
        protected string _entityVersion = null;
        protected string _token = null;
        string _tenant = null;
        protected Dictionary<string, string[]> _serviceurls = new Dictionary<string, string[]>
        {
            {  "Azure Cloud", new[] { "https://management.core.windows.net/", "https://login.windows.net/" } },
            {  "Azure China Cloud", new[] { "https://management.core.chinacloudapi.cn/", "https://login.chinacloudapi.cn/" } },
            {  "Azure Germany Cloud", new[] { "https://management.core.cloudapi.de/", "https://login.microsoftonline.de/" } },
            {  "Azure US Government Cloud", new[] { "https://management.core.usgovcloudapi.net/", "https://login.windows.net/" } },
        };
        public SubscriberActorBase(IZeusClient client, Dictionary<string, object> settings)
        {
            _client = client;
            _settings = settings;


        }



        public virtual bool Start()
        {
            _token = _settings["Token"].ToString();
            _tenant = _settings["Tenant"].ToString();

            return true;
        }

        void _client_OnDataAvailable(object Sender, DataReturnArgs e)
        {
            ReceivedMessage rm = e.ReceivedMessage;
            if (rm != null && rm.To == _nameSpace && rm.Action == _method && rm.Entity == _entity && rm.Version == _entityVersion)
            {
                Execute(e.ReceivedMessage);
            }


        }

        public virtual bool Subscribe()
        {
            _client.OnDataAvailable += _client_OnDataAvailable;
            return true;
        }

        public virtual bool Unsubscribe()
        {
            _client.OnDataAvailable -= _client_OnDataAvailable;
            return true;
        }

        public virtual bool Stop()
        {
            _token = null;
            return true;
        }

        public virtual bool Execute(Mercury.Shared.ReceivedMessage rm)
        {
            // always override this with the subclass actors
            throw new NotImplementedException();
        }

        public XmlDocument RemoveXmlns(String xml)
        {
            if (!xml.StartsWith("<"))
            {
                xml = $"<data>{xml}</data>";
            }
            XDocument d = XDocument.Parse(xml);
            d.Root.DescendantsAndSelf().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in d.Descendants())
                elem.Name = elem.Name.LocalName;

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(d.CreateReader());
            string myxml = xmlDocument.OuterXml;
            return xmlDocument;
        }

        protected string GetServiceManagementUrl(string cloudName)
        {
            return _serviceurls[cloudName][0];
        }

        string GetLoginUrl(string cloudName)
        {
            return _serviceurls[cloudName][1];
        }



    }
}
