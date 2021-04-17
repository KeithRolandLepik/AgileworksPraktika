using Data.Common;

namespace Domain.Common
{
    public abstract class Entity<TData> where TData : UniqueEntityData, new()
    {
        public TData Data { get; internal set; }

        protected internal Entity(TData data = null) => Data = data;

    }
}
