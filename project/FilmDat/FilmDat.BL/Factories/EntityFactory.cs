using System;
using FilmDat.DAL.Interfaces;

namespace FilmDat.BL.Factories
{
    public class EntityFactory : IEntityFactory
    {
        public EntityFactory()
        {
        }

        public TEntity Create<TEntity>(Guid id) where TEntity : class, IEntity, new() => new TEntity();
    }
}