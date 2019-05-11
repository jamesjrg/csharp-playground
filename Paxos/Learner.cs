using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paxos.Messages;

namespace Paxos
{
    public class Learner: IBackgroundTask
    {
        readonly string _name;
        readonly ConcurrentQueue<Accepted> _queue;

        public Learner(string name, ConcurrentQueue<Accepted> queue)
        {
            _name = name;
            _queue = queue;
        }

        public async Task Run()
        {
            var received = new Dictionary<int, List<Accepted>>();

            await LoopUtil.LoopTheLoop(_queue, async message =>
            {
                if (!received.TryGetValue(message.TimePeriod, out var messagesForPeriod))
                    received[message.TimePeriod] = messagesForPeriod = new List<Accepted>();
                
                var matchingMessage = messagesForPeriod.FirstOrDefault(
                    prevMessage => message.By != prevMessage.By);

                messagesForPeriod.Add(message);

                if (matchingMessage == null) return;

                Console.WriteLine($"Learned value '{message.Value}' in time period {message.TimePeriod}");
            });
        }
    }
}