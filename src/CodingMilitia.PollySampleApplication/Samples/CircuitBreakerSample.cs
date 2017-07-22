using CodingMilitia.PollySampleApplication.Service;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Threading.Tasks;

namespace CodingMilitia.PollySampleApplication.Samples
{
    public class CircuitBreakerSample : AbstractSample
    {
        public override async Task RunAsync()
        {
            var breakCircuitAfterTwoFailuresPolicy = Policy
              .Handle<BoomerangThrowFailedException>()
              .CircuitBreakerAsync(
                   2,
                   TimeSpan.FromSeconds(1),
                    (ex, span) =>
                    {
                        Console.WriteLine("Failed! Circuit open, waiting {0}", span);
                        Console.WriteLine("Error was {0}", ex.GetType().Name);
                    },
                    () => Console.WriteLine("First execution after circuit break succeeded, circuit is reset.")
                   );

            Console.WriteLine("|{0}|", nameof(breakCircuitAfterTwoFailuresPolicy));

            await SucceedsAfterFirstCircuitBreak(breakCircuitAfterTwoFailuresPolicy);
        }

        private async Task SucceedsAfterFirstCircuitBreak(Policy breakCircuitAfterTwoFailuresPolicy)
        {

            Console.WriteLine("----------- Succeeds after first circuit break-----------");
            var service = new FailForSomeTimeStuffService(TimeSpan.FromSeconds(1));
            for (var i = 0; i < 4; ++i)
            {
                try
                {
                    await breakCircuitAfterTwoFailuresPolicy.ExecuteAsync(async () =>
                    {
                        await ThrowBoomerangAsync(service);
                    });
                }
                catch (BrokenCircuitException)
                {
                    Console.WriteLine("Broken circuit exception, must wait until circuit is closed again.");
                    await Task.Delay(1000);
                }
                catch (BoomerangThrowFailedException)
                {
                    Console.WriteLine("Failed an execution.");
                }
            }

        }
    }
}
