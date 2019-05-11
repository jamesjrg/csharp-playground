using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Paxos
{
    public class MessageSender
    {
        readonly HttpClient _client;
        readonly IList<string> _uris;

        public MessageSender(IList<string> targets)
        {
            // generally HTTP Clients should be reused via HttpClientFactory, but in this program the messenger
            // instances are only created once, when the program starts, so this is fine
            _client = new HttpClient();
            _uris = targets.Select(target => $"http://localhost/paxos/{target}").ToList();
        }
        
        public async Task PostMessage<T>(T message)
        {
            StringContent content = null;
            try
            {
                content = new StringContent(
                    JsonConvert.SerializeObject(message),
                    Encoding.UTF8,
                    "application/json");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception serializing messages(): {0}", e);
                return;
            }

            foreach (var uri in _uris)
            {
                try
                {
                    var response = await _client.PostAsync(uri, content);
                    Console.WriteLine("PostMessage(): got {0}", response.StatusCode);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception in PostMessage(): {0}", e);
                }
            }
        }
    }
}