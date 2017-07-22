using CodingMilitia.PollySampleApplication.Service;
using Polly;
using System;
using System.Threading.Tasks;

namespace CodingMilitia.PollySampleApplication.Samples
{
    public class RetryAndWaitUsingTimeSpanProviderSample : RetryAndWaitSample
    {
        public override async Task RunAsync()
        {
            var retryTimeSpanMap = new TimeSpan?[] { null, TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(2) };
            var retryAndWaitUsingTimeSpanProviderPolicy = Policy
               .Handle<BoomerangThrowFailedException>()
               .WaitAndRetryAsync(
                   3,
                   (retryCount) => retryTimeSpanMap[retryCount].Value,
                   (ex, span) =>
                   {
                       Console.WriteLine("Failed! Waiting {0}", span);
                       Console.WriteLine("Error was {0}", ex.GetType().Name);
                   }
               );

            Console.WriteLine("|{0}|", nameof(retryAndWaitUsingTimeSpanProviderPolicy));

            await FailAfterAllWaitsAsync(retryAndWaitUsingTimeSpanProviderPolicy);
            await SucceedAfterWaitingThreeTimesRetryAsync(retryAndWaitUsingTimeSpanProviderPolicy);
        }
    }
}
