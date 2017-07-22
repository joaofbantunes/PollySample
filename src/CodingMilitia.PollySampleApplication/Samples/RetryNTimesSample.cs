using CodingMilitia.PollySampleApplication.Service;
using Polly;
using System;
using System.Threading.Tasks;

namespace CodingMilitia.PollySampleApplication.Samples
{
    public class RetryNTimesSample : AbstractSample
    {
        public override async Task RunAsync()
        {
            var retryTwoTimesPolicy = Policy
             .Handle<BoomerangThrowFailedException>()
             .RetryAsync(2, (ex, count) =>
             {
                 Console.WriteLine("Failed! Retry number {0}", count);
                 Console.WriteLine("Error was {0}", ex.GetType().Name);
             }
             );

            Console.WriteLine("|{0}|", nameof(retryTwoTimesPolicy));


            await FailAfterSecondRetry(retryTwoTimesPolicy);
            await SucceeOnSecondRetry(retryTwoTimesPolicy);
        }

        private async Task FailAfterSecondRetry(Policy retryTwoTimesPolicy)
        {
            Console.WriteLine("----------- Fails after second retry -----------");
            var service = new FailNTimesStuffService(3);
            try
            {
                await retryTwoTimesPolicy.ExecuteAsync(async () =>
                {
                    await ThrowBoomerangAsync(service);
                });
            }
            catch (BoomerangThrowFailedException)
            {
                Console.WriteLine("-> Failed more times than the retries can handle.");
            }
        }

        private async Task SucceeOnSecondRetry(Policy retryTwoTimesPolicy)
        {
            Console.WriteLine("----------- Succeeds on second retry -----------");
            var service = new FailNTimesStuffService(2);
            await retryTwoTimesPolicy.ExecuteAsync(async () =>
            {
                await ThrowBoomerangAsync(service);
            });
        }

    }
}
