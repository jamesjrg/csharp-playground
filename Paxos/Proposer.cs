using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paxos.Messages;

namespace Paxos
{
    internal class Proposer : IBackgroundTask
    {
        readonly string _name;
        readonly IList<string> _acceptorNames;
        readonly ConcurrentQueue<Promised> _queue;
        readonly Func<string> _valueProposer;

        public Proposer(string name, IList<string> acceptorNames, ConcurrentQueue<Promised> queue,
            Func<string> valueProposer)
        {
            _name = name;
            _acceptorNames = acceptorNames;
            _queue = queue;
            _valueProposer = valueProposer;
        }

        public async Task Run()
        {
            var messenger = new MessageSender(_acceptorNames);
            var received = new Dictionary<int, List<Promised>>();
            var latestProposedTimePeriod = 0;

            await LoopUtil.LoopTheLoop(_queue, async message =>
            {
                if (message.TimePeriod <= latestProposedTimePeriod) return;

                if (!received.TryGetValue(message.TimePeriod, out var messagesForPeriod))
                    received[message.TimePeriod] = messagesForPeriod = new List<Promised>();

                var matchingMessage =
                    messagesForPeriod.FirstOrDefault(
                        prevMessage => message.By != prevMessage.By);

                if (matchingMessage == null) return;

                messagesForPeriod.Add(message);
                latestProposedTimePeriod = message.TimePeriod;

                var lastValueWithGreatestTime =
                    message.LastAcceptedTimePeriod > matchingMessage.LastAcceptedTimePeriod
                        ? message.LastAcceptedValue
                        : matchingMessage.LastAcceptedValue;

                var proposedValue = (message, matchingMessage) switch
                    {
                    ({HaveAccepted: true}, { HaveAccepted: true}) => lastValueWithGreatestTime,
                    ({HaveAccepted: true, LastAcceptedValue: var x}, _) => x,
                    (_, { HaveAccepted: true, LastAcceptedValue: var x}) => x,
                    _ => _valueProposer(),
                    };

                await messenger.PostMessage(new Proposed
                {
                    TimePeriod = message.TimePeriod,
                    Value = proposedValue,
                });

                Console.WriteLine($"Proposed value '{proposedValue}' in time period {message.TimePeriod}");
            });
        }
    }
}