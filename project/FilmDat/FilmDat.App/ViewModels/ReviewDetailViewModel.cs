using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using FilmDat.App.Commands;
using FilmDat.App.Services;
using FilmDat.App.ViewModels.Interfaces;
using FilmDat.App.Wrappers;
using FilmDat.BL.Interfaces;
using FilmDat.BL.Messages;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Services;
using FilmDat.DAL.Seeds;

namespace FilmDat.App.ViewModels
{
    public class ReviewDetailViewModel : ViewModelBase, IReviewDetailViewModel
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMediator _mediator;

        public ReviewDetailViewModel(IReviewRepository reviewRepository, IMediator mediator)
        {
            _reviewRepository = reviewRepository;
            _mediator = mediator;

            SaveCommand = new RelayCommand(Save, CanSave);
        }

        public ICommand SaveCommand { get; set; }
        public Guid FilmId { get; set; }
        public ReviewDetailWrapper Model { get; set; }

        public void Load(Guid id)
        {
            Model = _reviewRepository.GetById(id) ?? new ReviewDetailModel()
            {
                Date = DateTime.Now,
                FilmId = FilmId
            };
        }

        private bool CanSave() =>
            Model != null
            && !string.IsNullOrWhiteSpace(Model.NickName)
            && !string.IsNullOrWhiteSpace(Model.TextReview)
            && Model.Rating >= 0
            && Model.Rating <= 100
            && FilmId != Guid.Empty;

        private void Save()
        {
            Model = _reviewRepository.InsertOrUpdate(Model.Model);
            _mediator.Send(new SelectedMessage<FilmWrapper> {Id = FilmId});
        }

        public override void LoadInDesignMode()
        {
            base.LoadInDesignMode();
            Model = new ReviewDetailWrapper(new ReviewDetailModel()
            {
                NickName = "Johnnie",
                Date = DateTime.Now,
                Rating = 89,
                TextReview = "Paradicka!",
                FilmId = Seed.InterstellarFilm.Id
            });
        }
    }
}