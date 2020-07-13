using System;
using System.Collections.Generic;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;

namespace FilmDat.BL.Interfaces
{
    public interface IActedInFilmRepository
    {
        void Delete(ActedInFilmDetailModel detailModel);
        void Delete(Guid id);
        ActedInFilmDetailModel GetById(Guid entityId);
        ActedInFilmDetailModel InsertOrUpdate(ActedInFilmDetailModel detailModel);
        IEnumerable<ActedInFilmListModel> GetAll();
    }
}