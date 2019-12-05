using System.Threading.Tasks;

namespace HexagonalArchitectureSample.UseCases
{
    public interface ICommandHandler<in TCommand, TResult>
    {
        Task<TResult> HandleAsync(TCommand command);
    }
}
