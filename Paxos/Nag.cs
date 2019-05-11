using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Paxos.Messages;

namespace Paxos
{
    internal static class Nag
    {
        const int LoopDelaySeconds = 5;
        
        public static async Task Run(List<string> targets)
        {
            var messenger = new MessageSender(targets);
            
            while (true)
            {
                var time = (DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;
                await messenger.PostMessage(new Prepare
                {
                    TimePeriod = (int)time,
                });
                await Task.Delay(LoopDelaySeconds * 1000);
            }
            
            /*
            Config{
                ..
            } <-atomically $ checking(( > 0). cNagPeriodSec) $ readTVar configVar
            threadDelay $ cNagPeriodSec * 1000000

            now < -getCurrentTime
            let timePeriod = floor $ diffUTCTime now unixEpoch
                value = Prepare Nothing $ SimpleTimePeriod timePeriod
            when cShowIncoming $ logMessage now Inbound "/nag" $ formatForLog value
            atomically $ do
                let minTimePeriod = timePeriod - 300
            modifyTVar minTimePeriodVar $ max minTimePeriod
            modifyTVar proposersByTimePeriodVar $ snd.M.split minTimePeriod
            writeTQueue incomingQueue("/nag", now, value)
            */
        }
    }
}