using System;
using System.Collections.Generic;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;

namespace FilmDat.BL.Interfaces
{
    public interface IFilmRepository
    {
        void Delete(FilmDetailModel detailModel);
        void Delete(Guid id);
        FilmDetailModel GetById(Guid entityId);
        FilmDetailModel InsertOrUpdate(FilmDetailModel detailModel);
        IEnumerable<FilmListModel> GetAll();
    }
}