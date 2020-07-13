using System;
using System.Collections.Generic;
using FilmDat.App.ViewModels;
using FilmDat.BL.Interfaces;
using FilmDat.BL.Models.ListModels;
using FilmDat.BL.Services;
using Xunit;
using Moq;

namespace FilmDat.View.Tests
{
    public class ReviewListViewModelTests
    {
        private readonly Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly Mock<Mediator> _mediatorMock;
        private readonly ReviewListViewModel _reviewListViewModelSUT;

        public ReviewListViewModelTests()
        {
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _mediatorMock = new Mock<Mediator> { CallBase = true };

            _reviewRepositoryMock.Setup(repository => repository.GetAll())
                .Returns(() => new List<ReviewListModel>());

            _reviewListViewModelSUT = new ReviewListViewModel(_reviewRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public void Load_Calls_ReviewRepository_GetAll()
        {
            _reviewListViewModelSUT.Load();


            _reviewRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
        }

        [Fact]
        public void Load_FromEmptyRepository_GetAll()
        {
            _reviewListViewModelSUT.Load();


            _reviewRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);
            Assert.Empty(_reviewListViewModelSUT.Reviews);
        }

        [Fact]
        public void Load_OneReviewFromRepository_GetAll()
        {
            _reviewRepositoryMock.Setup(repository => repository.GetAll())
                .Returns(() => new List<ReviewListModel> { new ReviewListModel() });


            _reviewListViewModelSUT.Load();


            Assert.True(_reviewListViewModelSUT.Reviews.Count == 1);
        }

    }
}