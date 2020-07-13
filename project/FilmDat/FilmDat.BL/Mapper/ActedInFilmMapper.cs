using FilmDat.BL.Factories;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;
using FilmDat.DAL.Entities;
using FilmDat.DAL.Interfaces;

namespace FilmDat.BL.Mapper
{
    public static class ActedInFilmMapper
    {
        public static ActedInFilmListModel MapToListModel(ActedInFilmEntity entity) =>
            entity == null
                ? null
                : new ActedInFilmListModel()
                {
                    Id = entity.Id,
                    ActorId = entity.ActorId,
                    FilmId = entity.FilmId
                };

        public static ActedInFilmDetailModel MapToDetailModel(ActedInFilmEntity entity) =>
            entity == null
                ? null
                : new ActedInFilmDetailModel()
                {
                    Id = entity.Id,
                    ActorId = entity.ActorId,
                    FirstName = entity.Actor.FirstName,
                    LastName = entity.Actor.LastName,
                    FilmId = entity.FilmId,
                    OriginalName = entity.Film.OriginalName
                };

        public static ActedInFilmEntity MapToEntity(ActedInFilmDetailModel model, IEntityFactory entityFactory)
        {
            var entity = (entityFactory ??= new EntityFactory()).Create<ActedInFilmEntity>(model.Id);

            entity.Id = model.Id;
            entity.ActorId = model.ActorId;
            entity.Actor = (entityFactory ??= new EntityFactory()).Create<PersonEntity>(model.ActorId);
            entity.FilmId = model.FilmId;
            entity.Film = (entityFactory ??= new EntityFactory()).Create<FilmEntity>(model.FilmId);

            return entity;
        }
    }
}