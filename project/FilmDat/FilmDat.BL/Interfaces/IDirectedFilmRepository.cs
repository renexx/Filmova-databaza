using System;
using System.Collections.Generic;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;

namespace FilmDat.BL.Interfaces
{
    public interface IDirectedFilmRepository
    {
        void Delete(DirectedFilmDetailModel detailModel);
        void Delete(Guid id);
        DirectedFilmDetailModel GetById(Guid entityId);
        DirectedFilmDetailModel InsertOrUpdate(DirectedFilmDetailModel detailModel);
        IEnumerable<DirectedFilmListModel> GetAll();
    }
}