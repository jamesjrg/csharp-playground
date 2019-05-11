using System;
using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

/*
I'm sure there's a way to write this as a bash one liner that combines ping with two different hosts, adding a timestamp to each line, and output redirection, but I felt like doing it in C#.
 */
namespace network_poller
{
    class Program
    {
        const string LogFile = "/home/james/network_poller.txt";
        
        static void WriteLog(string msg)
        {
            var currentTime = DateTime.UtcNow.ToString("s");
            using (StreamWriter writer = new StreamWriter(LogFile, append: true))
            {
                writer.WriteLine($"{currentTime}: {msg}");
            }
        }

        static async Task Ping(Ping ping, string address)
        {
            var startTime = DateTime.UtcNow.ToString("s");

            try
            {
                var reply = await ping.SendPingAsync(address);
                var success = reply.Status == IPStatus.Success ? "success" : "failure";

                WriteLog($"{address,-20} {success} in {reply.RoundtripTime}ms");
            }
            catch (PingException e)
            {
                WriteLog($"{address} exception {e} (start time {startTime})");
            }
        }

        static async Task Main(string[] args)
        {            
            //const string google = "www.google.com";
            const string google = "216.58.204.36";
            const string defaultGateway = "192.168.0.1";

            using (var ping1 = new Ping())
            using (var ping2 = new Ping())
            {
                while (true)
                {                
                    var task1 = Task.Run(() => Ping(ping1, defaultGateway));
                    var task2 = Task.Run(() => Ping(ping2, google));

                    await Task.WhenAll(new[]{task1, task2});

                    WriteLog("");

                    await Task.Delay(1000);
                }
            }
        }
    }
}
