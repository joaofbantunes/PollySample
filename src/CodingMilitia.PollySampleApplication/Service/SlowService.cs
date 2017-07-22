using System;
using System.Threading.Tasks;

namespace CodingMilitia.PollySampleApplication.Service
{
    public class SlowService : IStuffService
    {
        private TimeSpan _responseTime;
        public SlowService(TimeSpan responseTime)
        {
            _responseTime = responseTime;
        }
        public async Task<T> ThrowBoomerangAsync<T>(T value)
        {
            await Task.Delay(_responseTime);
            return value;
        }
    }
}