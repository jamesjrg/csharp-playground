using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Paxos
{
    public static class LoopUtil
    {
        const int LoopDelayMilliseconds = 50;
        
        public static async Task LoopTheLoop<T>(
            ConcurrentQueue<T> queue, Func<T, Task> funky)
        {
            while (true)
            {
                T message;
                var gotMessage = queue.TryDequeue(out message);

                if (!gotMessage)
                {
                    await Task.Delay(LoopDelayMilliseconds);
                    continue;
                }

                await funky(message);
            }
        }
    }
}