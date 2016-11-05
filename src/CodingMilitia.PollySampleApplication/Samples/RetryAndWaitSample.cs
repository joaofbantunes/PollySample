using CodingMilitia.PollySampleApplication.Service;
using Polly;
using System;
using System.Threading.Tasks;

namespace CodingMilitia.PollySampleApplication.Samples
{
    public class RetryAndWaitSample : AbstractSample
    {
        public override void Run()
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

            FailAfterAllWaits(retryAndWaitPolicy);
            SucceedAfterWaitingThreeTimesRetry(retryAndWaitPolicy);
        }

        protected void FailAfterAllWaits(Policy retryAndWaitPolicy)
        {
            Console.WriteLine("----------- Fails after all waits -----------");
            var service = new FailForSomeTimeStuffService(TimeSpan.FromSeconds(10));
            try
            {
                Task.WaitAll(retryAndWaitPolicy.ExecuteAsync(async () =>
                {
                    await ThrowBoomerangAsync(service);
                }));
            }
            catch (AggregateException ae) when (ae.InnerException is BoomerangThrowFailedException)
            {
                Console.WriteLine("-> Failed more times than specified waits.");
            }
        }

        protected void SucceedAfterWaitingThreeTimesRetry(Policy retryAndWaitPolicy)
        {
            Console.WriteLine("----------- Succeeds after waiting three times-----------");
            var service = new FailForSomeTimeStuffService(TimeSpan.FromSeconds(8));
            Task.WaitAll(retryAndWaitPolicy.ExecuteAsync(async () =>
            {
                await ThrowBoomerangAsync(service);
            }));
        }
    }
}
