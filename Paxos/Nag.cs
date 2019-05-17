using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Paxos.Messages;

namespace Paxos
{
    internal class Nag: IBackgroundTask
    {
        readonly List<string> _proposerNames;
        readonly List<string> _acceptorNames;
        int _proposerIndex = 0;
        const int LoopDelaySeconds = 5;
        

        public Nag(List<string> proposerNames, List<string> acceptorNames)
        {
            _proposerNames = proposerNames;
            _acceptorNames = acceptorNames;
        }
        
        public async Task Run()
        {
            var messenger = new MessageSender();
            
            while (true)
            {
                var time = (DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;

                if (_proposerNames.Count == 0)
                    continue;
                _proposerIndex = (_proposerIndex + 1) % _proposerNames.Count;
                
                await messenger.PostMessage(_acceptorNames, "prepare", new Prepare
                {
                    TimePeriod = (int)time,
                    ProposerName = _proposerNames[_proposerIndex]
                });
                await Task.Delay(LoopDelaySeconds * 1000);
            }
        }
    }
}