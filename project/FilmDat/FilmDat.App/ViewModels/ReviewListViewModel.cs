using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using FilmDat.App.Commands;
using FilmDat.App.ViewModels.Interfaces;
using FilmDat.App.Wrappers;
using FilmDat.BL.Extensions;
using FilmDat.BL.Interfaces;
using FilmDat.BL.Messages;
using FilmDat.BL.Models.ListModels;
using FilmDat.BL.Services;

namespace FilmDat.App.ViewModels
{
    public class ReviewListViewModel : ViewModelBase, IReviewListViewModel
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMediator _mediator;

        public ReviewListViewModel(IReviewRepository reviewRepository, IMediator mediator)
        {
            _reviewRepository = reviewRepository;
            _mediator = mediator;

            NewReviewCommand = new RelayCommand(ReviewNew, CanClick);
            ReviewSelectedCommand = new RelayCommand<ReviewListModel>(ReviewSelected);

            mediator.Register<NewMessage<FilmWrapper>>(OnFilmNewMessage);
            mediator.Register<SelectedMessage<FilmWrapper>>(FilmSelected);
        }

        public ICommand NewReviewCommand { get; set; }
        public ICommand ReviewSelectedCommand { get; }

        public Guid FilmId { get; set; }
        public ObservableCollection<ReviewListModel> Reviews { get; } = new ObservableCollection<ReviewListModel>();

        private void OnFilmNewMessage(NewMessage<FilmWrapper> _)
        {
            FilmId = Guid.Empty;
            Load();
        }

        private void FilmSelected(SelectedMessage<FilmWrapper> message)
        {
            FilmId = message.Id;
            Load();
        }

        private bool CanClick() => FilmId != Guid.Empty;
        private void ReviewNew() => _mediator.Send(new NewMessage<ReviewDetailWrapper> { TargetId = FilmId });

        private void ReviewSelected(ReviewListModel review) =>
            _mediator.Send(new SelectedMessage<ReviewDetailWrapper> { Id = review.Id });

        public void Load()
        {
            Reviews.Clear();
            var reviews = _reviewRepository.GetAll().Where(r => r.FilmId == FilmId);
            Reviews.AddRange(reviews);
        }

        public override void LoadInDesignMode()
        {
            Reviews.Add(new ReviewListModel() { Rating = 80, TextReview = "Vyborneee!" });
        }
    }
}