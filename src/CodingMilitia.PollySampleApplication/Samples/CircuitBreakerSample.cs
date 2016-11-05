using CodingMilitia.PollySampleApplication.Service;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Threading.Tasks;

namespace CodingMilitia.PollySampleApplication.Samples
{
    public class CircuitBreakerSample : AbstractSample
    {
        public override void Run()
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

            SucceedsAfterFirstCircuitBreak(breakCircuitAfterTwoFailuresPolicy);
        }

        private void SucceedsAfterFirstCircuitBreak(Policy breakCircuitAfterTwoFailuresPolicy)
        {

            Console.WriteLine("----------- Succeeds after first circuit break-----------");
            var service = new FailForSomeTimeStuffService(TimeSpan.FromSeconds(1));
            for (var i = 0; i < 4; ++i)
            {
                try
                {
                    Task.WaitAll(breakCircuitAfterTwoFailuresPolicy.ExecuteAsync(async () =>
                    {
                        await ThrowBoomerangAsync(service);
                    }));
                }
                catch (AggregateException ae) when (ae.InnerException is BrokenCircuitException)
                {
                    Console.WriteLine("Broken circuit exception, must wait until circuit is closed again.");
                    Task.WaitAll(Task.Delay(1000));
                }
                catch (AggregateException ae) when (ae.InnerException is BoomerangThrowFailedException)
                {
                    Console.WriteLine("Failed an execution.");
                }
            }

        }
    }
}
