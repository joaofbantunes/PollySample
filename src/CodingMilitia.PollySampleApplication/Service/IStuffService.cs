using System.Threading.Tasks;

namespace CodingMilitia.PollySampleApplication.Service
{
    public interface IStuffService
    {
        Task<T> ThrowBoomerangAsync<T>(T value); 
    }
}
