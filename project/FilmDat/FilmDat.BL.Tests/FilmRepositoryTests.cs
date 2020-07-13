using System;
using System.Collections.Generic;
using System.Linq;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Repositories;
using FilmDat.BL.Mapper;
using FilmDat.BL.Models.ListModels;
using FilmDat.DAL.Enums;
using FilmDat.DAL.Factories;
using FilmDat.DAL.Seeds;
using Xunit;

namespace FilmDat.BL.Tests
{
    public class FilmRepositoryTests : IDisposable
    {
        private readonly FilmRepository _filmRepositorySUT;
        private readonly DbContextInMemoryFactory _dbContextFactory;

        public FilmRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(FilmRepositoryTests));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();

            _filmRepositorySUT = new FilmRepository(_dbContextFactory);
        }

        [Fact]
        public void Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new FilmDetailModel()
            {
                Id = Guid.Empty,
                OriginalName = "The Avengers",
                Country = "USA",
                CzechName = "Avengers",
                Description = "Marvelovka",
                Genre = GenreEnum.SciFi,
                TitleFotoUrl = "avangers",
                Reviews = new List<ReviewListModel>(),
                Actors = new List<ActedInFilmDetailModel>(),
                Directors = new List<DirectedFilmDetailModel>()
            };
            var returnedModel = _filmRepositorySUT.InsertOrUpdate(model);
            Assert.NotNull(returnedModel);
        }

        [Fact]
        public void GetById_SeedFilm()
        {
            var film2 = _filmRepositorySUT.GetById(Seed.GreaseFilm.Id);
            var film = FilmMapper.MapToDetailModel(Seed.GreaseFilm);

            Assert.Equal(film, film2, FilmDetailModel.FilmDetailModelComparer);
            Assert.Equal(film.Reviews, film2.Reviews, ReviewListModel.ReviewListModelComparer);
            Assert.Equal(film.Actors, film2.Actors, ActedInFilmDetailModel.ActedInFilmDetailModelComparer);
            Assert.Equal(film.Directors, film2.Directors, DirectedFilmDetailModel.DirectedFilmDetailModelComparer);
        }

        [Fact]
        public void SeedFilm_DeleteByID_Deleted()
        {
            _filmRepositorySUT.Delete(Seed.GreaseFilm.Id);
            using var dbxAssert = _dbContextFactory.CreateDbContext();
            Assert.False(dbxAssert.Films.Any(i => i.Id == Seed.GreaseFilm.Id));
        }

        [Fact]
        public void NewFilm_InsertOrUpdate_FilmAdded()
        {
            var film = new FilmDetailModel()
            {
                Id = Guid.Empty,
                OriginalName = "A",
                Genre = GenreEnum.Action,
                CzechName = "B",
                TitleFotoUrl = "titlefoto.png",
                Description = "A film!",
                Country = "Murica",
                Reviews = new List<ReviewListModel>(),
                Actors = new List<ActedInFilmDetailModel>(),
                Directors = new List<DirectedFilmDetailModel>()
            };

            using var dbxAssert = _dbContextFactory.CreateDbContext();

            film = _filmRepositorySUT.InsertOrUpdate(film);

            var filmFromDb = dbxAssert.Films.Single(i => i.Id == film.Id);
            var film2 = FilmMapper.MapToDetailModel(filmFromDb);

            Assert.Equal(film, film2, FilmDetailModel.FilmDetailModelComparer);
            Assert.Equal(film.Reviews, film2.Reviews, ReviewListModel.ReviewListModelComparer);
            Assert.Equal(film.Actors, film2.Actors, ActedInFilmDetailModel.ActedInFilmDetailModelComparer);
            Assert.Equal(film.Directors, film2.Directors, DirectedFilmDetailModel.DirectedFilmDetailModelComparer);
        }

        [Fact]
        public void SeedFilm_InsertOrUpdate_FilmUpdated()
        {
            var film = new FilmDetailModel()
            {
                Id = Seed.InterstellarFilm.Id,
                OriginalName = Seed.InterstellarFilm.OriginalName + "updated",
            };
            film = _filmRepositorySUT.InsertOrUpdate(film);

            var film2 = _filmRepositorySUT.GetById(film.Id);

            Assert.Equal(film, film2, FilmDetailModel.FilmDetailModelComparer);
            Assert.Equal(film.Reviews, film2.Reviews, ReviewListModel.ReviewListModelComparer);
            Assert.Equal(film.Actors, film2.Actors, ActedInFilmDetailModel.ActedInFilmDetailModelComparer);
            Assert.Equal(film.Directors, film2.Directors, DirectedFilmDetailModel.DirectedFilmDetailModelComparer);
        }

        [Fact]
        public void GetAll_Single_SeedFilm()
        {
            var film = _filmRepositorySUT.GetAll().Single(i => i.Id == Seed.GreaseFilm.Id);

            Assert.Equal(FilmMapper.MapToListModel(Seed.GreaseFilm), film, FilmListModel.IdOriginalNameComparer);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();
        }
    }
}