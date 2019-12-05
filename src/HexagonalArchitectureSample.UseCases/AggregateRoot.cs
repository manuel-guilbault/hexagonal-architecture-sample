namespace HexagonalArchitectureSample.UseCases
{
    public class AggregateRoot<TEntity>
    {
        public AggregateRoot(TEntity entity, VersionTag versionTag)
        {
            Entity = entity;
            VersionTag = versionTag;
        }

        public TEntity Entity { get; }
        public VersionTag VersionTag { get; }

        public bool IsUpToDate(VersionTag? versionTag)
        {
            return versionTag?.Equals(VersionTag) ?? true;
        }
    }

    public static class AggregateRoot
    {
        public static AggregateRoot<TEntity> Create<TEntity>(TEntity entity, VersionTag versionTag)
        {
            return new AggregateRoot<TEntity>(entity, versionTag);
        }
    }
}
