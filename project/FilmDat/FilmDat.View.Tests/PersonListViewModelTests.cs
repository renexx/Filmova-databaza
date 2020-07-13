using System;
using System.Collections.Generic;
using FilmDat.App.ViewModels;
using FilmDat.App.Wrappers;
using FilmDat.BL.Interfaces;
using FilmDat.BL.Messages;
using FilmDat.BL.Models.ListModels;
using FilmDat.BL.Services;
using Xunit;
using Moq;

namespace FilmDat.View.Tests
{
    public class PersonListViewModelTests
    {
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly Mock<Mediator> _mediatorMock;
        private readonly PersonListViewModel _personListViewModelSUT;

        public PersonListViewModelTests()
        {
            _personRepositoryMock = new Mock<IPersonRepository>();
            _mediatorMock = new Mock<Mediator> {CallBase = true};

            _personRepositoryMock.Setup(repository => repository.GetAll())
                .Returns(() => new List<PersonListModel>());

            _personListViewModelSUT = new PersonListViewModel(_personRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public void Load_Calls_PersonRepository_GetAll()
        {
            _personListViewModelSUT.Load();


            _personRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
        }

        [Fact]
        public void Load_FromEmptyRepository_GetAll()
        {
            _personListViewModelSUT.Load();


            _personRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
            Assert.Empty(_personListViewModelSUT.Persons);
        }

        [Fact]
        public void Load_OnePersonFromRepository_GetAll()
        {
            _personRepositoryMock.Setup(repository => repository.GetAll())
                .Returns(() => new List<PersonListModel> {new PersonListModel()});


            _personListViewModelSUT.Load();


            Assert.True(_personListViewModelSUT.Persons.Count == 1);
        }

        [Fact]
        public void PersonUpdated_Calls_GetAll_FromRepository()
        {
            _mediatorMock.Object.Send(new UpdateMessage<PersonWrapper>());


            _personRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
        }

        [Fact]
        public void PersonDeleted_Calls_GetAll_FromRepository()
        {
            _mediatorMock.Object.Send(new DeleteMessage<PersonWrapper>());


            _personRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
        }

        [Fact]
        public void PersonSelectedCommand__Sends_PersonSelectedMessage()
        {
            _personListViewModelSUT.PersonSelectedCommand.Execute(new PersonListModel());


            _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<SelectedMessage<PersonWrapper>>()), Times.Once);
        }

        [Fact]
        public void PersonNewCommand__Sends_PersonSelectedMessage()
        {
            _personListViewModelSUT.PersonNewCommand.Execute(null);


            _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<NewMessage<PersonWrapper>>()), Times.Once);
        }
    }
}