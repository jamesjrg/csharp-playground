using System.Threading.Tasks;

namespace Paxos
{
    public interface IBackgroundTask
    {
        Task Run();
    }
}