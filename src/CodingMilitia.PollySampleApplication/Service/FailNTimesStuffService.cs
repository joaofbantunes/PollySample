using System.Threading.Tasks;

namespace CodingMilitia.PollySampleApplication.Service
{
    public class FailNTimesStuffService : IStuffService
    {
        private int _failuresRemaining;

        public FailNTimesStuffService(int failCount)
        {
            _failuresRemaining = failCount;

        }

        public Task<T> ThrowBoomerangAsync<T>(T value)
        {
            if (_failuresRemaining > 0)
            {
                --_failuresRemaining;
                throw new BoomerangThrowFailedException();
            }
            return Task.FromResult(value);
        }
    }
}
