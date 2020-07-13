using FilmDat.BL.Interfaces;
using FilmDat.BL.Mapper;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;
using FilmDat.DAL.Entities;
using FilmDat.DAL.Interfaces;
using FilmDat.DAL.Repositories;

namespace FilmDat.BL.Repositories
{
    public class ActedInFilmRepository : RepositoryBase<ActedInFilmEntity, ActedInFilmListModel, ActedInFilmDetailModel>, IActedInFilmRepository
    {
        public ActedInFilmRepository(IDbContextFactory dbContextFactory)
            : base(dbContextFactory,
                ActedInFilmMapper.MapToEntity,
                ActedInFilmMapper.MapToListModel,
                ActedInFilmMapper.MapToDetailModel,
                null,
                null,
                null)
        {
        }
    }
}