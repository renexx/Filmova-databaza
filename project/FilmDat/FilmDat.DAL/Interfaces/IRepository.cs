using System;
using System.Collections.Generic;
using FilmDat.Common;

namespace FilmDat.DAL.Interfaces
{
    public interface IRepository<TEntity, out TListModel, TDetailModel> where TEntity :
        class, IEntity, new()
        where TListModel : IId, new()
        where TDetailModel : IId, new()
    {
        TDetailModel InsertOrUpdate(TDetailModel model);
        void Delete(TDetailModel entity);
        void Delete(Guid entityId);
        TDetailModel GetById(Guid entityId);
        IEnumerable<TListModel> GetAll();
    }
}