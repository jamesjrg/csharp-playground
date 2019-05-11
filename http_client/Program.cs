using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Formatting;

/*
* Investigating a few things:
 *
 * 1. How HTTP client factory is meant to work in console apps in latest preview of .NET Core 3. It seems the
options include installing the DependencyInjection package and creating a ServiceProvider, or adding .NET Core MVC nuget packages
 and creating a host builder. Then again, in console applications it's often simpler to just new up a client when the program starts and keep it around until the program ends...
 
2. the current state of JSON deserialization in HttpClient. It took me quite a while to work out which serializer ReadAsAsync uses in .NET Core preview 5. Once upon a time I think it used the framework DataContractJsonSerializer, then in .NET Core the Microsoft WebApi Client nuget package added a dependency on JSON.NET instead. .NET Core 3 preview 4 introduced its own JSON serializer, which was then updated in preview 5 to support (de)serializing to/from C# POCOs, however it appears this new serializer is currently only used by ASP.NET servers, not by instances of HttpClient.
    
3. Looking into the method ReadAsAsync from .Net.Http.Formatting instead of reading as a string then converting to an object. This avoids creating an intermediate string, which is more efficient, though whether it really matters for most applications is  debatable.
*/

namespace http_client
{
    public class GitHubResponse
    {
        public string current_user_url;
    }

    class MyLovelyHttpClient
    {
        HttpClient _client { get; }
        
        public MyLovelyHttpClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://api.github.com");
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-agent", "dont-return-403-please"); // GitHub API requires a user agent
        }

        public async Task<GitHubResponse> FetchSomeDataPlease()
        {
            var path = "/";
            var response = await _client.GetAsync(path);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<GitHubResponse>();

            return result;
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddHttpClient<MyLovelyHttpClient>(); 

            var serviceProvider = services.BuildServiceProvider();

            var client = serviceProvider.GetRequiredService<MyLovelyHttpClient>();

            Console.WriteLine((await client.FetchSomeDataPlease()).current_user_url);
        }
    }
}
