﻿using CodingMilitia.PollySampleApplication.Service;
using Polly;
using System;

namespace CodingMilitia.PollySampleApplication.Samples
{
    public class ExecuteAndCaptureSample : AbstractSample
    {
        public override void Run()
        {
            var executeAndCapturePolicy = Policy
               .Handle<BoomerangThrowFailedException>()
               .RetryAsync(1);

            Console.WriteLine("|{0}|", nameof(executeAndCapturePolicy));

            PrintSuccessResult(executeAndCapturePolicy);
            PrintFailedResult(executeAndCapturePolicy);
        }

        private void PrintSuccessResult(Policy executeAndCapturePolicy)
        {
            Console.WriteLine("----------- Prints successful result -----------");
            var service = new FailNTimesStuffService(1);
            var policyResult = executeAndCapturePolicy.ExecuteAndCaptureAsync(async () => await service.ThrowBoomerangAsync("-> Thrown")).Result;
            Console.WriteLine("{0}: {1}", nameof(policyResult.Outcome), policyResult.Outcome);
            Console.WriteLine("{0}: {1}", nameof(policyResult.FinalException), policyResult.FinalException);
            Console.WriteLine("{0}: {1}", nameof(policyResult.ExceptionType), policyResult.ExceptionType);
            Console.WriteLine("{0}: {1}", nameof(policyResult.Result), policyResult.Result);
        }

        private void PrintFailedResult(Policy executeAndCapturePolicy)
        {
            Console.WriteLine("----------- Prints failed result -----------");
            var service = new FailNTimesStuffService(2);
            var policyResult = executeAndCapturePolicy.ExecuteAndCaptureAsync(async () => await service.ThrowBoomerangAsync("-> Thrown")).Result;
            Console.WriteLine("{0}: {1}", nameof(policyResult.Outcome), policyResult.Outcome);
            Console.WriteLine("{0}: {1}", nameof(policyResult.FinalException), policyResult.FinalException);
            Console.WriteLine("{0}: {1}", nameof(policyResult.ExceptionType), policyResult.ExceptionType);
            Console.WriteLine("{0}: {1}", nameof(policyResult.Result), policyResult.Result);
        }
    }
}
