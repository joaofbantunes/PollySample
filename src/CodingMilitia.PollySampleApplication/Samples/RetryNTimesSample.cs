using CodingMilitia.PollySampleApplication.Service;
using Polly;
using System;
using System.Threading.Tasks;

namespace CodingMilitia.PollySampleApplication.Samples
{
    public class RetryNTimesSample : AbstractSample
    {
        public override void Run()
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


            FailAfterSecondRetry(retryTwoTimesPolicy);
            SucceeOnSecondRetry(retryTwoTimesPolicy);
        }

        private void FailAfterSecondRetry(Policy retryTwoTimesPolicy)
        {
            Console.WriteLine("----------- Fails after second retry -----------");
            var service = new FailNTimesStuffService(3);
            try
            {
                Task.WaitAll(retryTwoTimesPolicy.ExecuteAsync(async () =>
                {
                    await ThrowBoomerangAsync(service);
                }));
            }
            catch (AggregateException ae) when (ae.InnerException is BoomerangThrowFailedException)
            {
                Console.WriteLine("-> Failed more times than the retries can handle.");
            }
        }

        private void SucceeOnSecondRetry(Policy retryTwoTimesPolicy)
        {
            Console.WriteLine("----------- Succeeds on second retry -----------");
            var service = new FailNTimesStuffService(2);
            Task.WaitAll(retryTwoTimesPolicy.ExecuteAsync(async () =>
            {
                await ThrowBoomerangAsync(service);
            }));
        }

    }
}
