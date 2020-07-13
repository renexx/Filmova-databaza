using System;
using System.Collections.Generic;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;

namespace FilmDat.BL.Interfaces
{
    public interface IPersonRepository
    {
        void Delete(PersonDetailModel detailModel);
        void Delete(Guid id);
        PersonDetailModel GetById(Guid entityId);
        PersonDetailModel InsertOrUpdate(PersonDetailModel detailModel);
        IEnumerable<PersonListModel> GetAll();
    }
}