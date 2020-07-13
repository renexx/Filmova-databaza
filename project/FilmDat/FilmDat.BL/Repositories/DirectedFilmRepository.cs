using FilmDat.BL.Interfaces;
using FilmDat.BL.Mapper;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;
using FilmDat.DAL.Entities;
using FilmDat.DAL.Interfaces;
using FilmDat.DAL.Repositories;

namespace FilmDat.BL.Repositories
{
    public class DirectedFilmRepository : RepositoryBase<DirectedFilmEntity, DirectedFilmListModel, DirectedFilmDetailModel>, IDirectedFilmRepository
    {
        public DirectedFilmRepository(IDbContextFactory dbContextFactory)
            : base(dbContextFactory,
                DirectedFilmMapper.MapToEntity,
                DirectedFilmMapper.MapToListModel,
                DirectedFilmMapper.MapToDetailModel,
                null,
                null,
                null)
        {
        }
    }
}