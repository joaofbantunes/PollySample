using CodingMilitia.PollySampleApplication.Service;
using Polly;
using System;
using System.Threading.Tasks;

namespace CodingMilitia.PollySampleApplication.Samples
{
    public class ExecuteAndCaptureSample : AbstractSample
    {
        public override async Task RunAsync()
        {
            var executeAndCapturePolicy = Policy
               .Handle<BoomerangThrowFailedException>()
               .RetryAsync(1);

            Console.WriteLine("|{0}|", nameof(executeAndCapturePolicy));

            await PrintSuccessResult(executeAndCapturePolicy);
            await PrintFailedResult(executeAndCapturePolicy);
        }

        private async Task PrintSuccessResult(Policy executeAndCapturePolicy)
        {
            Console.WriteLine("----------- Prints successful result -----------");
            var service = new FailNTimesStuffService(1);
            var policyResult = await executeAndCapturePolicy.ExecuteAndCaptureAsync(async () => await service.ThrowBoomerangAsync("-> Thrown"));
            Console.WriteLine("{0}: {1}", nameof(policyResult.Outcome), policyResult.Outcome);
            Console.WriteLine("{0}: {1}", nameof(policyResult.FinalException), policyResult.FinalException);
            Console.WriteLine("{0}: {1}", nameof(policyResult.ExceptionType), policyResult.ExceptionType);
            Console.WriteLine("{0}: {1}", nameof(policyResult.Result), policyResult.Result);
        }

        private async Task PrintFailedResult(Policy executeAndCapturePolicy)
        {
            Console.WriteLine("----------- Prints failed result -----------");
            var service = new FailNTimesStuffService(2);
            var policyResult = await executeAndCapturePolicy.ExecuteAndCaptureAsync(async () => await service.ThrowBoomerangAsync("-> Thrown"));
            Console.WriteLine("{0}: {1}", nameof(policyResult.Outcome), policyResult.Outcome);
            Console.WriteLine("{0}: {1}", nameof(policyResult.FinalException), policyResult.FinalException);
            Console.WriteLine("{0}: {1}", nameof(policyResult.ExceptionType), policyResult.ExceptionType);
            Console.WriteLine("{0}: {1}", nameof(policyResult.Result), policyResult.Result);
        }
    }
}
