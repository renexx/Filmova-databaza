using System;
using System.Collections.Generic;
using System.Linq;
using FilmDat.BL.Mapper;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;
using FilmDat.BL.Repositories;
using FilmDat.DAL.Factories;
using FilmDat.DAL.Seeds;
using Xunit;

namespace FilmDat.BL.Tests
{
    public class PersonRepositoryTests : IDisposable
    {
        private readonly PersonRepository _personRepositorySUT;
        private readonly DbContextInMemoryFactory _dbContextFactory;

        public PersonRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(PersonRepositoryTests));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();
            _personRepositorySUT = new PersonRepository(_dbContextFactory);
        }

        [Fact]
        public void Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new PersonDetailModel()
            {
                Id = Guid.Empty,
                FirstName = "Jan",
                LastName = "Testovaci",
                BirthDate = new DateTime(1979, 10, 27),
                FotoUrl = "fotka",
                DirectedFilms = new List<DirectedFilmDetailModel>(),
                ActedInFilms = new List<ActedInFilmDetailModel>()
            };
            var returnmodel = _personRepositorySUT.InsertOrUpdate(model);
            Assert.NotNull(returnmodel);
        }

        [Fact]
        public void Create_WithNonExistingItem_DoesNotThrowAndEqualsCreated()
        {
            var person = new PersonDetailModel()
            {
                Id = Guid.Empty,
                FirstName = "Jan",
                LastName = "b",
                BirthDate = new DateTime(1979, 10, 27),
                FotoUrl = "a",
                ActedInFilms = new List<ActedInFilmDetailModel>(),
                DirectedFilms = new List<DirectedFilmDetailModel>()
            };
            person = _personRepositorySUT.InsertOrUpdate(person);
            var person2 = _personRepositorySUT.GetById(person.Id);

            Assert.Equal(person.Id, person2.Id);

            Assert.Equal(person, person2, PersonDetailModel.PersonDetailModelComparer);
            Assert.Equal(person.ActedInFilms, person2.ActedInFilms,
                ActedInFilmDetailModel.ActedInFilmDetailModelComparer);
            Assert.Equal(person.DirectedFilms, person2.DirectedFilms,
                DirectedFilmDetailModel.DirectedFilmDetailModelComparer);
        }

        [Fact]
        public void GetAll_Single_SeedPerson()
        {
            var person = _personRepositorySUT.GetAll().Single(i => i.Id == Seed.MatthewMcConaughey.Id);

            Assert.Equal(PersonMapper.MapToListModel(Seed.MatthewMcConaughey), person,
                PersonListModel.IdFirstNameLastNameComparer);
        }

        [Fact]
        public void GetById_SeededPerson()
        {
            var person2 = _personRepositorySUT.GetById(Seed.JohnTravolta.Id);
            var person = PersonMapper.MapToDetailModel(Seed.JohnTravolta);

            Assert.Equal(person, person2, PersonDetailModel.PersonDetailModelComparer);
            Assert.Equal(person.ActedInFilms, person2.ActedInFilms,
                ActedInFilmDetailModel.ActedInFilmDetailModelComparer);
            Assert.Equal(person.DirectedFilms, person2.DirectedFilms,
                DirectedFilmDetailModel.DirectedFilmDetailModelComparer);
        }

        [Fact]
        public void SeededPerson_DeleteById_Deleted()
        {
            _personRepositorySUT.Delete(Seed.JohnTravolta.Id);
            using var dbxAssert = _dbContextFactory.CreateDbContext();

            Assert.False(dbxAssert.Persons.Any(i => i.Id == Seed.JohnTravolta.Id));
        }

        [Fact]
        public void NewPerson_InsertOrUpdate_PersonAdded()
        {
            var person = new PersonDetailModel()
            {
                Id = Guid.Empty,
                FirstName = "test",
                LastName = "testovaci",
                BirthDate = new DateTime(1989, 12, 12),
                FotoUrl = "foto",
                ActedInFilms = new List<ActedInFilmDetailModel>(),
                DirectedFilms = new List<DirectedFilmDetailModel>()
            };
            person = _personRepositorySUT.InsertOrUpdate(person);
            var person2 = _personRepositorySUT.GetById(person.Id);

            Assert.Equal(person, person2, PersonDetailModel.PersonDetailModelComparer);
            Assert.Equal(person.ActedInFilms, person2.ActedInFilms,
                ActedInFilmDetailModel.ActedInFilmDetailModelComparer);
            Assert.Equal(person.DirectedFilms, person2.DirectedFilms,
                DirectedFilmDetailModel.DirectedFilmDetailModelComparer);
        }

        [Fact]
        public void SeededPerson_InsertOrUpdate_PersonUpdated()
        {
            var person = new PersonDetailModel()
            {
                Id = Seed.ChristopherNolan.Id,
                FirstName = Seed.ChristopherNolan.FirstName + "updated",
                LastName = Seed.ChristopherNolan.LastName + "updated",
            };

            person = _personRepositorySUT.InsertOrUpdate(person);
            var person2 = _personRepositorySUT.GetById(person.Id);

            Assert.Equal(person, person2, PersonDetailModel.PersonDetailModelComparer);
            Assert.Equal(person.ActedInFilms, person2.ActedInFilms,
                ActedInFilmDetailModel.ActedInFilmDetailModelComparer);
            Assert.Equal(person.DirectedFilms, person2.DirectedFilms,
                DirectedFilmDetailModel.DirectedFilmDetailModelComparer);
        }

        [Fact]
        public void Update_FromSeeded_DoesNotThrow()
        {
            //Arrange
            var person = PersonMapper.MapToDetailModel(Seed.RandalKleiser);
            person.FirstName = "Changed first name";
            //Act & Assert
            person = _personRepositorySUT.InsertOrUpdate(person);
        }

        [Fact]
        public void Update_Name_FromSeeded_CheckUpdated()
        {
            //Arrange
            var person = PersonMapper.MapToDetailModel(Seed.RandalKleiser);
            person.FirstName = "Changed first name to joe";
            //Act & Assert
            person = _personRepositorySUT.InsertOrUpdate(person);
            var person2 = _personRepositorySUT.GetById(person.Id);
            Assert.Equal(person, person2, PersonDetailModel.PersonDetailModelComparer);
        }

        [Fact]
        public void Update_RemovePerson_fromSeeded_CheckUpdated()
        {
            //Arrange
            var person = PersonMapper.MapToDetailModel(Seed.JohnTravolta);
            person.ActedInFilms.Clear();
            //Act
            person = _personRepositorySUT.InsertOrUpdate(person);
            //Assert
            var person2 = _personRepositorySUT.GetById(person.Id);
            Assert.Equal(person, person2, PersonDetailModel.PersonDetailModelComparer);
            Assert.Equal(person.ActedInFilms, person2.ActedInFilms,
                ActedInFilmDetailModel.ActedInFilmDetailModelComparer);
            Assert.Equal(person.DirectedFilms, person2.DirectedFilms,
                DirectedFilmDetailModel.DirectedFilmDetailModelComparer);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();
        }
    }
}