using System;
using System.Collections.Generic;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;

namespace FilmDat.BL.Interfaces
{
    public interface IReviewRepository
    {
        void Delete(ReviewDetailModel detailModel);
        void Delete(Guid id);
        ReviewDetailModel GetById(Guid entityId);
        ReviewDetailModel InsertOrUpdate(ReviewDetailModel detailModel);
        IEnumerable<ReviewListModel> GetAll();
    }
}