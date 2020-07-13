using System;

namespace FilmDat.DAL.Interfaces
{
    public interface IEntityFactory
    {
        TEntity Create<TEntity>(Guid id) where TEntity : class, IEntity, new();
    }
}