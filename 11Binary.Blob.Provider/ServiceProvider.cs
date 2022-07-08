using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeus.Shared;
using Mercury.Shared;

namespace _11Binary.Blob.Provider
{
    public class ServiceProvider : Mercury.Shared.IMythSuiteServiceProvider
    {
        private List<Mercury.Shared.ISubscriberActor> Actors = new List<Mercury.Shared.ISubscriberActor>();
        private IZeusClient _client; 
        private Dictionary<string, object> _settings = new Dictionary<string, object>();
        public ServiceProvider(ZeusClient client, Dictionary<string, object> settings)
        {
            _client = client;
            _settings = settings;
        }

        public bool Register()
        {

            Mercury.Shared.ISubscriberActor a1 = (Mercury.Shared.ISubscriberActor)new GetAllStorageAcountsActor(_client, _settings);
            Actors.Add(a1);
            return true;
        }

        public bool Start()
        {
            foreach (Mercury.Shared.ISubscriberActor actor in Actors)
            {
                actor.Start();
            }
            return true;
        }

        public bool Stop()
        {
            foreach (Mercury.Shared.ISubscriberActor actor in Actors)
            {
                actor.Stop();
            }
            return true;
        }

        public bool Subscribe()
        {
            foreach (Mercury.Shared.ISubscriberActor actor in Actors)
            {
                actor.Subscribe();
            }
            return true;
        }

        public bool Unsubscribe()
        {
            foreach (Mercury.Shared.ISubscriberActor actor in Actors)
            {
                actor.Unsubscribe();
            }
            return true;
        }
    }
}
