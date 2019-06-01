using System.Threading.Tasks;

namespace Fibo.Business.Interfaces.Handlers
{
    public interface ICommandHandler<TMessage>
    {
        Task ExecuteAsync(TMessage message);
    }
}
