using FilmDat.BL.Factories;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;
using FilmDat.DAL.Entities;
using FilmDat.DAL.Interfaces;

namespace FilmDat.BL.Mapper
{
    public static class DirectedFilmMapper
    {
        public static DirectedFilmListModel MapToListModel(DirectedFilmEntity entity) =>
            entity == null
                ? null
                : new DirectedFilmListModel()
                {
                    Id = entity.Id,
                    DirectorId = entity.DirectorId,
                    FilmId = entity.FilmId
                };

        public static DirectedFilmDetailModel MapToDetailModel(DirectedFilmEntity entity) =>
            entity == null
                ? null
                : new DirectedFilmDetailModel()
                {
                    Id = entity.Id,
                    DirectorId = entity.DirectorId,
                    FirstName = entity.Director.FirstName,
                    LastName = entity.Director.LastName,
                    FilmId = entity.FilmId,
                    OriginalName = entity.Film.OriginalName
                };

        public static DirectedFilmEntity MapToEntity(DirectedFilmDetailModel model, IEntityFactory entityFactory)
        {
            var entity = (entityFactory ??= new EntityFactory()).Create<DirectedFilmEntity>(model.Id);

            entity.Id = model.Id;
            entity.DirectorId = model.DirectorId;
            entity.Director = (entityFactory ??= new EntityFactory()).Create<PersonEntity>(model.DirectorId);
            entity.FilmId = model.FilmId;
            entity.Film = (entityFactory ??= new EntityFactory()).Create<FilmEntity>(model.FilmId);

            return entity;
        }
    }
}