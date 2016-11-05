using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CodingMilitia.PollySampleApplication.Service
{
    public class FailForSomeTimeStuffService : IStuffService
    {
        private readonly TimeSpan _failurePeriod;
        private readonly Stopwatch watch;

        public FailForSomeTimeStuffService(TimeSpan failurePeriod)
        {
            _failurePeriod = failurePeriod;
            watch = Stopwatch.StartNew();

        }
        public Task<T> ThrowBoomerangAsync<T>(T value)
        {
            if (watch.Elapsed < _failurePeriod)
            {
                throw new BoomerangThrowFailedException();
            }
            watch.Stop();
            return Task.FromResult(value);
        }
    }
}
