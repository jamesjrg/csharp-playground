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
        public static void Main(string[] args)
        {
            var valueProposer = new Func<string>(() => new Random().Next(2).ToString());
            
            var queues = new Queues();

            var proposerNames = new[] {"proposer1"}.ToList();
            var learnerNames = new[] {"learner1"}.ToList();
            var acceptorNames = Enumerable.Range(0, 3).Select(i => $"acceptor{i}").ToList();

            /*
             * Due to the slightly strange way this means there are several identical copies of exactly the same service need to run at the same time I'm using Task.Run instead of adding background tasks to the web host by implementing IHostedService or BackgroundService.     
             */
            foreach (var name in proposerNames)
            {
                var queue = new ConcurrentQueue<Promised>();
                queues.ProposerQueues[name] = queue;
                Task.Run(() => new Proposer(name, acceptorNames, queue, valueProposer).Run());
            }
            
            foreach (var name in learnerNames)
            {
                var queue = new ConcurrentQueue<Accepted>();
                queues.LearnerQueues[name] = queue;
                Task.Run(() => new Learner(name, queue).Run());
            }
            
            foreach (var name in acceptorNames)
            {
                var queue = new ConcurrentQueue<AcceptorReceived>();
                queues.AcceptorQueues[name] = queue;
                Task.Run(() => new Acceptor(name, proposerNames, learnerNames, queue).Run());
            }
            
            Task.Run(() => Nag.Run(acceptorNames));
            
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
                    webBuilder.UseStartup<Startup>();
                });
    }
}
