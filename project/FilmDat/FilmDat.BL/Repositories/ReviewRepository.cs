using FilmDat.BL.Interfaces;
using FilmDat.BL.Mapper;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;
using FilmDat.DAL.Entities;
using FilmDat.DAL.Interfaces;
using FilmDat.DAL.Repositories;

namespace FilmDat.BL.Repositories
{
    public class ReviewRepository : RepositoryBase<ReviewEntity, ReviewListModel, ReviewDetailModel>, IReviewRepository
    {
        public ReviewRepository(IDbContextFactory dbContextFactory)
            : base(dbContextFactory,
                ReviewMapper.MapToEntity,
                ReviewMapper.MapToListModel,
                ReviewMapper.MapToDetailModel,
                null,
                null,
                null)
        {
        }
    }
}