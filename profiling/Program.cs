#define LIBLOG_PUBLIC

using System;
using System.Diagnostics;
using Serilog;
using profiling.Logging;

namespace profiling
{
    /* https://github.com/dotnet/BenchmarkDotNet is infinitely better for profiling.
    This was a somewhat abortive attempt to see how the functions were affected by being called more times, rather than
    using the steady state model of BenchmarkDotNet.
    and hence to what extent different types of caching (CLR, CPU) are important, but doesn't really work very well at at all.
    
    At the time of writing there is an open BenchmarkDotNet PR with a run mode designed to cause CPU cache misses:
    https://github.com/dotnet/BenchmarkDotNet/issues/553
    */
    class Program
    {   
        static int Loops = 1;

        static void TimeItWithFor()
        {
            long nanosecPerTick = (1000L*1000L*1000L) / Stopwatch.Frequency;

            for (int i = 0; i != 10000; ++i)
            {
                var z = Guid.NewGuid();
            }
            
            Stopwatch sw = Stopwatch.StartNew();

            var startAllocations = GC.GetAllocatedBytesForCurrentThread();
            
            var logger = LogProvider.For<Program>();
            logger.Info("example");

            var endTicks = sw.ElapsedTicks;
            var endAllocations = GC.GetAllocatedBytesForCurrentThread() - startAllocations;
            
            Console.WriteLine($"Time to call For<Program>: {endTicks * nanosecPerTick}ns");
            Console.WriteLine($"Allocated {endAllocations} bytes  ");
        }

        static void TimeItWithGetCurrent()
        {
            long nanosecPerTick = (1000L*1000L*1000L) / Stopwatch.Frequency;

            for (int i = 0; i != 10000; ++i)
            {
                var z = Guid.NewGuid().ToString();
            }
            
            Stopwatch sw = Stopwatch.StartNew();

            var startAllocations = GC.GetAllocatedBytesForCurrentThread();
            
            var logger = LogProvider.GetCurrentClassLogger();
            logger.Info("example");

            var endTicks = sw.ElapsedTicks;
            var endAllocations = GC.GetAllocatedBytesForCurrentThread() - startAllocations;
            
            Console.WriteLine($"Time to call GetCurrent: {endTicks * nanosecPerTick}ns");
            Console.WriteLine($"Allocated {endAllocations} bytes  ");
        }

        static void Main(string[] args)
        {
            // for (int i = 0; i != 7; ++i)
            //     TimeItWithGetCurrent();

            for (int i = 0; i != 7; ++i)
                TimeItWithFor();
        }
    }
}
