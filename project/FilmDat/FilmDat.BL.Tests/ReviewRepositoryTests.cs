using System;
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
    public class ReviewRepositoryTests : IDisposable
    {
        private readonly ReviewRepository _reviewRepositorySUT;
        private readonly DbContextInMemoryFactory _dbContextFactory;

        public ReviewRepositoryTests()
        {
            _dbContextFactory = new DbContextInMemoryFactory(nameof(ReviewRepositoryTests));
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureCreated();

            _reviewRepositorySUT = new ReviewRepository(_dbContextFactory);
        }

        [Fact]
        public void Create_WithNonExistingItem_DoesNotThrow()
        {
            var model = new ReviewDetailModel()
            {
                NickName = "Basa",
                Date = new DateTime(2020, 01, 14),
                Rating = 54,
                TextReview = "V pohode film"
            };

            var returnedModel = _reviewRepositorySUT.InsertOrUpdate(model);
            Assert.NotNull(returnedModel);
        }

        [Fact]
        public void GetAll_Single_SeededReview()
        {
            var review = _reviewRepositorySUT.GetAll().Single(i => i.Id == Seed.Review1.Id);

            Assert.Equal(ReviewMapper.MapToListModel(Seed.Review1), review, ReviewListModel.ReviewListModelComparer);
        }

        [Fact]
        public void GetById_SeededReview()
        {
            var review = _reviewRepositorySUT.GetById(Seed.Review1.Id);

            Assert.Equal(ReviewMapper.MapToDetailModel(Seed.Review1), review,
                ReviewDetailModel.ReviewDetailModelComparer);
        }

        [Fact]
        public void SeededReview_DeleteById_Deleted()
        {
            _reviewRepositorySUT.Delete(Seed.Review1.Id);

            using var dbxAssert = _dbContextFactory.CreateDbContext();
            Assert.False(dbxAssert.Reviews.Any(i => i.Id == Seed.Review1.Id));
        }

        [Fact]
        public void NewReview_InsertOrUpdate_ReviewAdded()
        {
            var review = new ReviewDetailModel()
            {
                NickName = "Peter99",
                Date = new DateTime(2002, 11, 06),
                Rating = 95,
                TextReview = "Paradny film"
            };

            review = _reviewRepositorySUT.InsertOrUpdate(review);

            using var dbxAssert = _dbContextFactory.CreateDbContext();
            var reviewFromDb = dbxAssert.Reviews.Single(i => i.Id == review.Id);
            Assert.Equal(review, ReviewMapper.MapToDetailModel(reviewFromDb),
                ReviewDetailModel.ReviewDetailModelComparer);
        }

        [Fact]
        public void SeededReview_InsertOrUpdate_ReviewUpdated()
        {
            var review = new ReviewDetailModel()
            {
                Id = Seed.Review1.Id,
                NickName = Seed.Review1.NickName,
                Date = Seed.Review1.Date,
                Rating = Seed.Review1.Rating,
                TextReview = Seed.Review1.TextReview,
            };

            review.NickName += "updated";
            review.TextReview += "updated";

            _reviewRepositorySUT.InsertOrUpdate(review);

            using var dbxAssert = _dbContextFactory.CreateDbContext();
            var reviewFromDb = dbxAssert.Reviews.Single(i => i.Id == review.Id);

            Assert.Equal(review, ReviewMapper.MapToDetailModel(reviewFromDb),
                ReviewDetailModel.ReviewDetailModelComparer);
        }

        public void Dispose()
        {
            using var dbx = _dbContextFactory.CreateDbContext();
            dbx.Database.EnsureDeleted();
        }
    }
}