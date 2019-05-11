using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Paxos.Messages;

namespace Paxos
{
    public class Acceptor: IBackgroundTask
    {
        readonly string _name;
        readonly List<string> _proposerNames;
        readonly List<string> _learnerNames;
        readonly ConcurrentQueue<AcceptorReceived> _queue;

        public Acceptor(string name, List<string> proposerNames, List<string> learnerNames, ConcurrentQueue<AcceptorReceived> queue)
        {
            _name = name;
            _proposerNames = proposerNames;
            _learnerNames = learnerNames;
            _queue = queue;
        }


        public async Task Run()
        {
            var proposerSender = new MessageSender(_proposerNames);
            var learnerSender = new MessageSender(_learnerNames);
            
            var greatestPromisedTimePeriod = -1;
            var lastAcceptedTime = -1;
            string lastAcceptedValue = null;

            await LoopUtil.LoopTheLoop(_queue, async message =>
            {
                if (message.TimePeriod <= lastAcceptedTime) return;

                switch (message.Type)
                {
                    case "prepare":
                        if (message.TimePeriod > greatestPromisedTimePeriod)
                            greatestPromisedTimePeriod = message.TimePeriod;
                        
                        await proposerSender.PostMessage(new Promised
                        {
                            By = _name,
                            TimePeriod = message.TimePeriod,
                            HaveAccepted = lastAcceptedTime > -1,
                            LastAcceptedTimePeriod = lastAcceptedTime,
                            LastAcceptedValue = lastAcceptedValue
                        });
                        break;

                    case "proposed":
                        if (message.TimePeriod < greatestPromisedTimePeriod) return;

                        lastAcceptedTime = message.TimePeriod;
                        lastAcceptedValue = message.Value;

                        await learnerSender.PostMessage(new Accepted
                        {
                            By = _name,
                            TimePeriod = message.TimePeriod,
                            Value = message.Value
                        });
                        break;
                }
            });
        }
    }
}