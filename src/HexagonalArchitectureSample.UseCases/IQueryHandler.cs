using System.Threading.Tasks;

namespace HexagonalArchitectureSample.UseCases
{
    public interface IQueryHandler<in TQuery, TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
