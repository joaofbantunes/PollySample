using CodingMilitia.PollySampleApplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingMilitia.PollySampleApplication.Samples
{
    public abstract class AbstractSample
    {
        public abstract Task RunAsync();
        protected async Task ThrowBoomerangAsync(IStuffService service)
        {
            Console.WriteLine("Throwing...");
            Console.WriteLine(await service.ThrowBoomerangAsync("-> Thrown!"));
        }
    }
}
