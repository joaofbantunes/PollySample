using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polly;
using CodingMilitia.PollySampleApplication.Service;
using Polly.CircuitBreaker;
using CodingMilitia.PollySampleApplication.Samples;

namespace CodingMilitia.PollySampleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {

            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            var samples = new AbstractSample[]
            {
                new RetryNTimesSample(),
                new RetryAndWaitSample(),
                new RetryAndWaitUsingTimeSpanProviderSample(),
                new CircuitBreakerSample(),
                new AdvancedCircuitBreakerSample(),
                new ExecuteAndCaptureSample(),
                new BulkheadSample()
            };


            if (args.Length > 0)
            {
                int sampleIndexToRun = 0;
                if (int.TryParse(args[0], out sampleIndexToRun) && sampleIndexToRun >= 0 && sampleIndexToRun < samples.Length)
                {
                    await samples[sampleIndexToRun].RunAsync();
                }
                else
                {
                    Console.WriteLine("Invalid sample index provided: \"{0}\"", args[0]);
                }
            }
            else
            {
                foreach (var sample in samples)
                {
                    await sample.RunAsync();
                }
            }
        }
    }
}
