using CodingMilitia.PollySampleApplication.Service;
using Polly;
using System;
using System.Threading.Tasks;

namespace CodingMilitia.PollySampleApplication.Samples
{
    public class RetryAndWaitSample : AbstractSample
    {
        public override async Task RunAsync()
        {
            var retryAndWaitPolicy = Policy
               .Handle<BoomerangThrowFailedException>()
               .WaitAndRetryAsync(
                   new TimeSpan[] { TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(2) },
                   (ex, span) =>
                   {
                       Console.WriteLine("Failed! Waiting {0}", span);
                       Console.WriteLine("Error was {0}", ex.GetType().Name);
                   }
               );

            Console.WriteLine("|{0}|", nameof(retryAndWaitPolicy));

            await FailAfterAllWaits(retryAndWaitPolicy);
            await SucceedAfterWaitingThreeTimesRetry(retryAndWaitPolicy);
        }

        protected async Task FailAfterAllWaits(Policy retryAndWaitPolicy)
        {
            Console.WriteLine("----------- Fails after all waits -----------");
            var service = new FailForSomeTimeStuffService(TimeSpan.FromSeconds(10));
            try
            {
                await retryAndWaitPolicy.ExecuteAsync(async () =>
                {
                    await ThrowBoomerangAsync(service);
                });
            }
            catch (BoomerangThrowFailedException)
            {
                Console.WriteLine("-> Failed more times than specified waits.");
            }
        }

        protected async Task SucceedAfterWaitingThreeTimesRetry(Policy retryAndWaitPolicy)
        {
            Console.WriteLine("----------- Succeeds after waiting three times-----------");
            var service = new FailForSomeTimeStuffService(TimeSpan.FromSeconds(8));
            await retryAndWaitPolicy.ExecuteAsync(async () =>
            {
                await ThrowBoomerangAsync(service);
            });
        }
    }
}
