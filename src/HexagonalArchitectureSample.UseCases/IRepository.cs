using System.Threading.Tasks;

namespace HexagonalArchitectureSample.UseCases
{
    public interface IRepository<in TId, TAggregateRoot>
    {
        Task CreateAsync(TAggregateRoot aggregateRoot);
        Task<AggregateRoot<TAggregateRoot>?> GetByIdAsync(TId id);
        Task UpdateAsync(AggregateRoot<TAggregateRoot> aggregateRoot);
    }
}
