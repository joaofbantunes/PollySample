using System;
using Polly;
using System.Threading.Tasks;
using CodingMilitia.PollySampleApplication.Service;
using System.Collections.Generic;
using System.Linq;
using Polly.Bulkhead;
using System.Diagnostics;

namespace CodingMilitia.PollySampleApplication.Samples
{
    public class BulkheadSample : AbstractSample
    {
        public override async Task RunAsync()
        {
            var bulkheadPolicy = Policy.BulkheadAsync(2, 2);
            await FirstPairSucceedsSecondPairSucceedsAfterWaitFifthFails(bulkheadPolicy);
        }

        private async Task FirstPairSucceedsSecondPairSucceedsAfterWaitFifthFails(Policy bulkheadPolicy)
        {
            Console.WriteLine("----------- First pair succeeds second pair succeeds after wait fifth fails -----------");

            var service = new SlowService(TimeSpan.FromSeconds(2));
            var taskList = new List<Task>();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (var i = 0; i < 5; ++i)
            {
                var capturedI = i;
                taskList.Add(Task.Run(async () =>
                {
                    try
                    {
                        await bulkheadPolicy.ExecuteAsync(async () =>
                        {
                            Console.WriteLine($"Executing task {capturedI} at {stopWatch.Elapsed}");
                            await service.ThrowBoomerangAsync(capturedI);
                        });
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Task {capturedI} failed with BulkheadRejectedException");
                    }
                }));
                await Task.Delay(100); //allow the tasks to run in order for better understanding of the sample
            }

            await Task.WhenAll(taskList);
            stopWatch.Stop();
        }
    }
}