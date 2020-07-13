using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using FilmDat.App.Commands;
using FilmDat.App.Factories;
using FilmDat.App.Services;
using FilmDat.App.ViewModels.Interfaces;
using FilmDat.App.Wrappers;
using FilmDat.BL.Interfaces;
using FilmDat.BL.Messages;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;
using FilmDat.BL.Services;
using Microsoft.EntityFrameworkCore;

namespace FilmDat.App.ViewModels
{
    public class FilmDetailViewModel : ViewModelBase, IFilmDetailViewModel
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMediator _mediator;

        public FilmDetailViewModel(
            IFilmRepository filmRepository,
            IMessageDialogService messageDialogService,
            IMediator mediator,
            IReviewListViewModel reviewListViewModel,
            IAddActorListViewModel addActorListViewModel,
            IAddDirectorListViewModel addDirectorListViewModel,
            IFactory<IReviewDetailViewModel> reviewDetailViewModelFactory)
        {
            _filmRepository = filmRepository;
            _messageDialogService = messageDialogService;
            _mediator = mediator;

            ReviewListViewModel = reviewListViewModel;
            AddActorListViewModel = addActorListViewModel;
            AddDirectorListViewModel = addDirectorListViewModel;

            _reviewDetailViewModelFactory = reviewDetailViewModelFactory;

            ReviewDetailViewModel = _reviewDetailViewModelFactory.Create();

            SaveCommand = new RelayCommand(Save, CanSave);
            DeleteCommand = new RelayCommand(Delete);
            CloseReviewDetailTabCommand = new RelayCommand(OnCloseReviewDetailTabExecute);
            ActorSelectedCommand = new RelayCommand<ActedInFilmWrapper>(OnActorSelected);
            DirectorSelectedCommand = new RelayCommand<DirectedFilmWrapper>(OnDirectorSelected);

            mediator.Register<NewMessage<ReviewDetailWrapper>>(OnReviewNewMessage);
            mediator.Register<SelectedMessage<ReviewDetailWrapper>>(OnReviewSelected);
        }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CloseReviewDetailTabCommand { get; }
        public ICommand ActorSelectedCommand { get; set; }
        public ICommand DirectorSelectedCommand { get; set; }

        public FilmWrapper Model { get; set; }
        public IReviewListViewModel ReviewListViewModel { get; }
        public IAddActorListViewModel AddActorListViewModel { get; }
        public IAddDirectorListViewModel AddDirectorListViewModel { get; }

        public IReviewDetailViewModel ReviewDetailViewModel { get; }
        public IReviewDetailViewModel SelectedReviewDetailViewModel { get; set; }
        private readonly IFactory<IReviewDetailViewModel> _reviewDetailViewModelFactory;

        public ObservableCollection<IReviewDetailViewModel> ReviewDetailViewModels { get; } =
            new ObservableCollection<IReviewDetailViewModel>();

        private void OnReviewNewMessage(NewMessage<ReviewDetailWrapper> message)
        {
            var reviewDetailViewModel = _reviewDetailViewModelFactory.Create();
            reviewDetailViewModel.FilmId = message.TargetId;

            ReviewDetailViewModels.Add(reviewDetailViewModel);
            reviewDetailViewModel.Load(Guid.Empty);

            SelectedReviewDetailViewModel = reviewDetailViewModel;
        }

        private void OnReviewSelected(SelectedMessage<ReviewDetailWrapper> message)
        {
            var reviewDetailViewModel = ReviewDetailViewModels.SingleOrDefault(vm => vm.Model.Id == message.Id);
            if (reviewDetailViewModel == null)
            {
                reviewDetailViewModel = _reviewDetailViewModelFactory.Create();

                ReviewDetailViewModels.Add(reviewDetailViewModel);
                reviewDetailViewModel.Load(message.Id);
            }

            SelectedReviewDetailViewModel = reviewDetailViewModel;
        }

        private void OnCloseReviewDetailTabExecute(object parameter)
        {
            if (parameter is IReviewDetailViewModel reviewDetailViewModel)
            {
                ReviewDetailViewModels.Remove(reviewDetailViewModel);
            }
        }

        private void OnActorSelected(ActedInFilmWrapper actor) =>
            _mediator.Send(new SelectedMessage<PersonWrapper> { Id = actor.Model.ActorId });

        private void OnDirectorSelected(DirectedFilmWrapper director) =>
            _mediator.Send(new SelectedMessage<PersonWrapper> { Id = director.Model.DirectorId });

        private void CountAvgRating()
        {
            double averageRating = 0;
            var counter = 0;
            foreach (var review in Model.Reviews)
            {
                averageRating += review.Rating;
                counter++;
            }

            if (counter != 0)
                averageRating /= counter;
            if (Math.Abs(averageRating) > 0)
                Model.AvgRating = Math.Round(averageRating, 1, MidpointRounding.AwayFromZero).ToString();
        }

        public void Load(Guid id)
        {
            Model = _filmRepository.GetById(id) ?? new FilmDetailModel()
            {
                Actors = new List<ActedInFilmDetailModel>(),
                Directors = new List<DirectedFilmDetailModel>(),
                Reviews = new List<ReviewListModel>()
            };
            CountAvgRating();
        }

        private bool CanSave() =>
            Model != null
            && !string.IsNullOrWhiteSpace(Model.OriginalName)
            && !string.IsNullOrWhiteSpace(Model.CzechName)
            && !string.IsNullOrWhiteSpace(Model.Country)
            && Model.Duration != default
            && Model.Duration > new TimeSpan(0, 0, 0, 0)
            && !string.IsNullOrWhiteSpace(Model.Description);

        private void Save()
        {
            Model = _filmRepository.InsertOrUpdate(Model);
            _mediator.Send(new UpdateMessage<FilmWrapper> {Model = Model});
            Load(Model.Id); // Average rating plus Lists of Actors and Directors
        }

        private void Delete(object obj)
        {
            if (Model.Id != Guid.Empty)
            {
                var delete = _messageDialogService.Show(
                    $"Delete",
                    $"Do you want to delete {Model?.OriginalName}?",
                    MessageDialogButtonConfiguration.YesNo,
                    MessageDialogResult.No);
                if (delete == MessageDialogResult.No)
                {
                    return;
                }

                try
                {
                    _filmRepository.Delete(Model.Id);
                }
                catch
                {
                    var _ = _messageDialogService.Show(
                        $"Deleting of {Model?.OriginalName} failed!",
                        "Deleting failed",
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
                    return;
                }

                _mediator.Send(new DeleteMessage<FilmWrapper> {Model = Model});
            }
        }

        public override void LoadInDesignMode()
        {
            base.LoadInDesignMode();
            Model = new FilmWrapper(new FilmDetailModel
            {
                OriginalName = "Venom",
                CzechName = "Votrelec",
                Country = "USA",
                Duration = new TimeSpan(0, 2, 20, 0),
                Description = "The best film with Tom Hardy",
                TitleFotoUrl =
                    "https://cleanfoodcrush.com/wp-content/uploads/2019/01/CleanFoodCrush-Super-Easy-Beef-Stir-Fry-Recipe.jpg"
            });
        }
    }
}