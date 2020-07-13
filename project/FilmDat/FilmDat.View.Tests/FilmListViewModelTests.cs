using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using FilmDat.BL.Interfaces;
using FilmDat.BL.Messages;
using FilmDat.BL.Models.ListModels;
using FilmDat.BL.Services;
using FilmDat.App.ViewModels;
using FilmDat.App.Wrappers;

namespace FilmDat.View.Tests
{
    public class FilmListViewModelTests
    {
        private readonly Mock<IFilmRepository> _filmRepositoryMock;
        private readonly Mock<Mediator> _mediatorMock;
        private readonly FilmListViewModel _filmListViewModelSUT;

        public FilmListViewModelTests()
        {
            _filmRepositoryMock = new Mock<IFilmRepository>();
            _mediatorMock = new Mock<Mediator> {CallBase = true};

            _filmRepositoryMock.Setup(repository => repository.GetAll())
                .Returns(() => new List<FilmListModel>());

            _filmListViewModelSUT = new FilmListViewModel(_filmRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public void Load_Calls_FilmRepository_GetAll()
        {
            _filmListViewModelSUT.Load();


            _filmRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
        }

        [Fact]
        public void Load_FromEmptyRepository_GetAll()
        {
            _filmListViewModelSUT.Load();


            _filmRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
            Assert.Empty(_filmListViewModelSUT.Films);
        }

        [Fact]
        public void Load_OneFilmFromRepository_GetAll()
        {
            _filmRepositoryMock.Setup(repository => repository.GetAll())
                .Returns(() => new List<FilmListModel> {new FilmListModel()});


            _filmListViewModelSUT.Load();


            Assert.True(_filmListViewModelSUT.Films.Count == 1);
        }

        [Fact]
        public void FilmUpdated_Calls_GetAll_FromRepository()
        {
            _mediatorMock.Object.Send(new UpdateMessage<FilmWrapper>());


            _filmRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
        }

        [Fact]
        public void FilmDeleted_Calls_GetAll_FromRepository()
        {
            _mediatorMock.Object.Send(new DeleteMessage<FilmWrapper>());


            _filmRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
        }

        [Fact]
        public void FilmSelectedCommand__Sends_FilmSelectedMessage()
        {
            _filmListViewModelSUT.FilmSelectedCommand.Execute(new FilmListModel());


            _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<SelectedMessage<FilmWrapper>>()), Times.Once);
        }

        [Fact]
        public void FilmNewCommand__Sends_FilmSelectedMessage()
        {
            _filmListViewModelSUT.FilmNewCommand.Execute(null);


            _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<NewMessage<FilmWrapper>>()), Times.Once);
        }
    }
}