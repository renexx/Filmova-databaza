using FilmDat.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;
using FilmDat.DAL.Enums;
using FilmDat.DAL.Factories;

namespace FilmDat.DAL.Tests
{
    public class FilmDatDbContextTests : IDisposable

    {
        private readonly DbContextInMemoryFactory _dbContextFactory;
        private readonly FilmDatDbContext _filmDatDbContext;

        public FilmDatDbContextTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(FilmDatDbContext));
            _filmDatDbContext = _dbContextFactory.CreateDbContext();
            _filmDatDbContext.Database.EnsureCreated();
        }

        [Fact]
        public void AddNew_Film_Persisted()
        {
            var film = new FilmEntity()
            {
                OriginalName = "Lord of the Rings",
                Country = "USA",
                CzechName = "Pan Prstenu",
                Description = "Something",
                Genre = GenreEnum.Fantasy,
                TitleFotoUrl = "path"
            };

            _filmDatDbContext.Films.Add((film));
            _filmDatDbContext.SaveChanges();

            using var dbx = _dbContextFactory.CreateDbContext();

            var fromDb = dbx.Films
                .Single(i => i.Id == film.Id);
            Assert.Equal(film, fromDb, FilmEntity.FilmAloneComparer);
        }

        [Fact]
        public void AddNew_Review_Persisted()
        {
            var review = new ReviewEntity()
            {
                TextReview = "Not so bad",
                Rating = 100,
                NickName = "Peter"
            };

            _filmDatDbContext.Reviews.Add(review);
            _filmDatDbContext.SaveChanges();

            using var dbx = _dbContextFactory.CreateDbContext();
            var fromDb = dbx.Reviews
                .Single(i => i.Id == review.Id);
            Assert.Equal(review, fromDb, ReviewEntity.ReviewEntityComparer);
        }

        [Fact]
        public void AddNew_Person_Persisted()
        {
            var person = new PersonEntity()
            {
                FirstName = "Jackie",
                LastName = "Chan"
            };

            _filmDatDbContext.Persons.Add(person);
            _filmDatDbContext.SaveChanges();

            using var dbx = _dbContextFactory.CreateDbContext();
            var fromDb = dbx.Persons
                .Single(i => i.Id == person.Id);
            Assert.Equal(person, fromDb, PersonEntity.PersonEntityComparer);
        }


        [Fact]
        public void addNew_FilmWithPerson_Persisted()
        {
            var film = new FilmEntity()
            {
                OriginalName = "The Avengers",
                Country = "USA",
                CzechName = "Avengers",
                Description = "Marvel...",
                Genre = GenreEnum.SciFi,
                TitleFotoUrl = "path",
                Actors =
                {
                    new ActedInFilmEntity()
                    {
                        Actor = new PersonEntity()
                        {
                            FirstName = "Robert",
                            LastName = "Downey ",
                        }
                    }
                },
                Directors =
                {
                    new DirectedFilmEntity()
                    {
                        Director = new PersonEntity()
                        {
                            FirstName = "Joss",
                            LastName = "Whedon"
                        }
                    }
                }
            };

            _filmDatDbContext.Films.Add((film));
            _filmDatDbContext.SaveChanges();

            using var dbx = _dbContextFactory.CreateDbContext();

            var fromDb = dbx.Films
                .Include(entity => entity.Actors)
                .ThenInclude(actor => actor.Actor)
                .Include(entity => entity.Directors)
                .ThenInclude(director => director.Director)
                .Single(i => i.Id == film.Id);
            Assert.Equal(film, fromDb, FilmEntity.FilmComparer);
        }


        [Fact]
        public void addNew_PersonWithFilm_Persisted()
        {
            var person = new PersonEntity()
            {
                FirstName = "Brad",
                LastName = "Pitt",

                ActedInFilms =
                {
                    new ActedInFilmEntity()
                    {
                        Film = new FilmEntity()
                        {
                            OriginalName = "The Avengers",
                            Country = "USA",
                            CzechName = "Avengers",
                            Description = "Marvel...",
                            Genre = GenreEnum.SciFi,
                            TitleFotoUrl = "path",
                        }
                    }
                },
                DirectedFilms =
                {
                    new DirectedFilmEntity()
                    {
                        Film = new FilmEntity()
                        {
                            OriginalName = "The Avengers",
                            Country = "USA",
                            CzechName = "Avengers",
                            Description = "Marvel...",
                            Genre = GenreEnum.SciFi,
                            TitleFotoUrl = "path",
                        }
                    }
                }
            };

            _filmDatDbContext.Persons.Add((person));
            _filmDatDbContext.SaveChanges();

            using var dbx = _dbContextFactory.CreateDbContext();

            var fromDb = dbx.Persons
                .Include(entity => entity.ActedInFilms)
                .ThenInclude(film => film.Film)
                .Include(entity => entity.DirectedFilms)
                .ThenInclude(director => director.Film)
                .Single(i => i.Id == person.Id);
            Assert.Equal(person, fromDb, PersonEntity.PersonEntityComparer);
        }

        [Fact]
        public void addNew_FilmWithReview_Persisted()
        {
            var film = new FilmEntity()
            {
                OriginalName = "The",
                Country = "Best",
                CzechName = "Film",
                Description = "in",
                TitleFotoUrl = "history",
                Genre = GenreEnum.Documentary,
                Reviews =
                {
                    new ReviewEntity()
                    {
                        TextReview = "Something",
                        NickName = "Someone",
                        Rating = 99
                    }
                }
            };

            _filmDatDbContext.Films.Add((film));
            _filmDatDbContext.SaveChanges();

            using var dbx = _dbContextFactory.CreateDbContext();

            var fromDb = dbx.Films
                .Include(entity => entity.Reviews)
                .Single(i => i.Id == film.Id);
            Assert.Equal(film, fromDb, FilmEntity.FilmComparer);
        }

        public void Dispose() => _filmDatDbContext?.Dispose();
    }
}