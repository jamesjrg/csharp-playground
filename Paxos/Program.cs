using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Paxos.Messages;

namespace Paxos
{
    public class Queues
    {
        public readonly Dictionary<string, ConcurrentQueue<Promised>> ProposerQueues;
        public readonly Dictionary<string, ConcurrentQueue<Accepted>> LearnerQueues;
        public readonly Dictionary<string, ConcurrentQueue<AcceptorReceived>> AcceptorQueues;

        public Queues()
        {
            ProposerQueues = new Dictionary<string, ConcurrentQueue<Promised>>();
            LearnerQueues = new Dictionary<string, ConcurrentQueue<Accepted>>();
            AcceptorQueues = new Dictionary<string, ConcurrentQueue<AcceptorReceived>>();
        }
    }
    
    static class Program
    {
        /*
         * If each type of background task had only a single instance then it would make sense to use
         * background tasks created with IHostedService/BackgroundService in the web host. However, with
         * multiple copies of each background task it is easier to just use Task.Run
         */
        static async Task RunTaskAfterDelay(IBackgroundTask t)
        {
            await Task.Delay(500);
            await t.Run();
        }
        
        public static void Main(string[] args)
        {
            var valueProposer = new Func<string>(() => new Random().Next(2).ToString());
            
            var queues = new Queues();

            var proposerNames = new[] {"proposer1"}.ToList();
            var learnerNames = new[] {"learner1"}.ToList();
            var acceptorNames = Enumerable.Range(0, 3).Select(i => $"acceptor{i}").ToList();

            foreach (var name in proposerNames)
            {
                var queue = new ConcurrentQueue<Promised>();
                queues.ProposerQueues[name] = queue;
                // ReSharper disable once ImplicitlyCapturedClosure
                Task.Run(() => RunTaskAfterDelay(new Proposer(name, acceptorNames, queue, valueProposer)));
            }
            
            foreach (var name in learnerNames)
            {
                var queue = new ConcurrentQueue<Accepted>();
                queues.LearnerQueues[name] = queue;
                Task.Run(() => RunTaskAfterDelay(new Learner(name, queue)));
            }
            
            foreach (var name in acceptorNames)
            {
                var queue = new ConcurrentQueue<AcceptorReceived>();
                queues.AcceptorQueues[name] = queue;
                // ReSharper disable once ImplicitlyCapturedClosure
                Task.Run(() => RunTaskAfterDelay(new Acceptor(name, learnerNames, queue)));
            }
            
            // ReSharper disable once ImplicitlyCapturedClosure
            Task.Run(() => RunTaskAfterDelay(new Nag(proposerNames, acceptorNames)));
            
            CreateHostBuilder(args, queues).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, Queues queues) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(queues);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseUrls("http://localhost:5001/");
                });
    }
}
