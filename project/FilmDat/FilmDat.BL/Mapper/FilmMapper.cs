using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FilmDat.BL.Factories;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;
using FilmDat.DAL.Entities;
using FilmDat.DAL.Enums;
using FilmDat.DAL.Interfaces;

namespace FilmDat.BL.Mapper
{
    public static class FilmMapper
    {
        public static FilmListModel MapToListModel(FilmEntity entity) =>
            entity == null
                ? null
                : new FilmListModel()
                {
                    Id = entity.Id,
                    OriginalName = entity.OriginalName
                };

        public static FilmDetailModel MapToDetailModel(FilmEntity entity) =>
            entity == null
                ? null
                : new FilmDetailModel()
                {
                    Id = entity.Id,
                    OriginalName = entity.OriginalName,
                    CzechName = entity.CzechName,
                    Genre = (GenreEnum) entity.Genre,
                    TitleFotoUrl = entity.TitleFotoUrl,
                    Country = entity.Country,
                    Duration = entity.Duration,
                    Description = entity.Description,

                    Actors = entity.Actors.Select(
                        actedInFilmEntity => new ActedInFilmDetailModel()
                        {
                            Id = actedInFilmEntity.Id,
                            ActorId = actedInFilmEntity.ActorId,
                            FirstName = actedInFilmEntity.Actor.FirstName,
                            LastName = actedInFilmEntity.Actor.LastName,
                            FilmId = entity.Id,
                            OriginalName = entity.OriginalName
                        }).ToList(),

                    Directors = entity.Directors.Select(
                        directedFilmEntity => new DirectedFilmDetailModel()
                        {
                            Id = directedFilmEntity.Id,
                            DirectorId = directedFilmEntity.DirectorId,
                            FirstName = directedFilmEntity.Director.FirstName,
                            LastName = directedFilmEntity.Director.LastName,
                            FilmId = entity.Id,
                            OriginalName = entity.OriginalName
                        }).ToList(),

                    Reviews = entity.Reviews.Select(
                        reviewEntity => new ReviewListModel()
                        {
                            Id = reviewEntity.Id,
                            Rating = reviewEntity.Rating,
                            TextReview = reviewEntity.TextReview,
                            FilmId = entity.Id
                        }).ToList(),

                    AvgRating = ""
                };

        public static FilmEntity MapToEntity(FilmDetailModel detailModel, IEntityFactory entityFactory)
        {
            var entity = (entityFactory ??= new EntityFactory()).Create<FilmEntity>(detailModel.Id);

            entity.Id = detailModel.Id;
            entity.OriginalName = detailModel.OriginalName;
            entity.CzechName = detailModel.CzechName;
            entity.Genre = (GenreEnum) detailModel.Genre;
            entity.TitleFotoUrl = detailModel.TitleFotoUrl;
            entity.Country = detailModel.Country;
            entity.Duration = detailModel.Duration;
            entity.Description = detailModel.Description;

            if (detailModel.Reviews != null)
                entity.Reviews = detailModel.Reviews
                    .Select(model => ReviewMapper.MapToEntity(model, entityFactory)).ToList();

            if (detailModel.Actors != null)
                entity.Actors = detailModel.Actors
                    .Select(model => ActedInFilmMapper.MapToEntity(model, entityFactory)).ToList();

            if (detailModel.Directors != null)
                entity.Directors = detailModel.Directors
                    .Select(model => DirectedFilmMapper.MapToEntity(model, entityFactory)).ToList();

            return entity;
        }
    }
}