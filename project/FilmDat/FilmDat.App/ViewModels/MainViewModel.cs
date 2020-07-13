using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using FilmDat.App.Commands;
using FilmDat.App.Factories;
using FilmDat.App.ViewModels.Interfaces;
using FilmDat.App.Wrappers;
using FilmDat.BL.Mapper;
using FilmDat.BL.Messages;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Services;

namespace FilmDat.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IMediator _mediator;
        public MainViewModel(
            IMediator mediator,
            IFilmListViewModel filmListViewModel,
            IPersonListViewModel personListViewModel,
            ISearchViewModel searchViewModel,
            IFactory<IFilmDetailViewModel> filmDetailViewModelFactory,
            IFactory<IPersonDetailViewModel> personDetailViewModelFactory)
        {
            _mediator = mediator;

            FilmListViewModel = filmListViewModel;
            PersonListViewModel = personListViewModel;
            SearchViewModel = searchViewModel;

            _filmDetailViewModelFactory = filmDetailViewModelFactory;
            _personDetailViewModelFactory = personDetailViewModelFactory;

            FilmDetailViewModel = _filmDetailViewModelFactory.Create();
            PersonDetailViewModel = _personDetailViewModelFactory.Create();

            CloseFilmDetailTabCommand = new RelayCommand(OnCloseFilmDetailTabExecute);
            ClosePersonDetailTabCommand = new RelayCommand(OnClosePersonDetailTabExecute);
            FilmDetailTabSelectionChangedCommand = new RelayCommand<FilmDetailViewModel>(FilmDetailTabChanged);
            PersonDetailTabSelectionChangedCommand = new RelayCommand<PersonDetailViewModel>(PersonDetailTabChanged);

            mediator.Register<NewMessage<FilmWrapper>>(OnFilmNewMessage);
            mediator.Register<NewMessage<PersonWrapper>>(OnPersonNewMessage);

            mediator.Register<SelectedMessage<FilmWrapper>>(OnFilmSelected);
            mediator.Register<SelectedMessage<PersonWrapper>>(OnPersonSelected);

            mediator.Register<UpdateMessage<FilmWrapper>>(OnFilmUpdated);
            mediator.Register<UpdateMessage<PersonWrapper>>(OnPersonUpdated);

            mediator.Register<DeleteMessage<FilmWrapper>>(OnFilmDeleted);
            mediator.Register<DeleteMessage<PersonWrapper>>(OnPersonDeleted);
        }

        // Commands
        public ICommand CloseFilmDetailTabCommand { get; }
        public ICommand ClosePersonDetailTabCommand { get; }
        public ICommand FilmDetailTabSelectionChangedCommand { get; }
        public ICommand PersonDetailTabSelectionChangedCommand { get; }

        // List view models
        public IFilmListViewModel FilmListViewModel { get; }
        public IPersonListViewModel PersonListViewModel { get; }
        public  ISearchViewModel SearchViewModel { get; }

        // Detail view models
        public IFilmDetailViewModel FilmDetailViewModel { get; }
        public IPersonDetailViewModel PersonDetailViewModel { get; }
        public IFilmDetailViewModel SelectedFilmDetailViewModel { get; set; }
        public IPersonDetailViewModel SelectedPersonDetailViewModel { get; set; }
        private readonly IFactory<IFilmDetailViewModel> _filmDetailViewModelFactory;
        private readonly IFactory<IPersonDetailViewModel> _personDetailViewModelFactory;

        public ObservableCollection<IFilmDetailViewModel> FilmDetailViewModels { get; } =
            new ObservableCollection<IFilmDetailViewModel>();

        public ObservableCollection<IPersonDetailViewModel> PersonDetailViewModels { get; } =
            new ObservableCollection<IPersonDetailViewModel>();

        private void FilmDetailTabChanged(FilmDetailViewModel filmViewModel) =>
            _mediator.Send(new SelectedMessage<FilmWrapper> { Id = filmViewModel.Model.Id });

        private void PersonDetailTabChanged(PersonDetailViewModel personViewModel) =>
            _mediator.Send(new SelectedMessage<PersonWrapper> { Id = personViewModel.Model.Id });

        private void OnFilmNewMessage(NewMessage<FilmWrapper> _)
        {
            SelectFilm(Guid.Empty);
        }

        private void OnPersonNewMessage(NewMessage<PersonWrapper> _)
        {
            SelectPerson(Guid.Empty);
        }

        private void OnFilmSelected(SelectedMessage<FilmWrapper> message)
        {
            SelectFilm(message.Id);
        }

        private void OnPersonSelected(SelectedMessage<PersonWrapper> message)
        {
            SelectPerson(message.Id);
        }

        private void OnFilmUpdated(UpdateMessage<FilmWrapper> message)
        {
            var filmDetailViewModel = FilmDetailViewModels.SingleOrDefault(vm => vm.Model.Id == message.Model.Id);
            if (filmDetailViewModel != null)
            {
                filmDetailViewModel.Load(message.Model.Id);
            }
            if (SelectedPersonDetailViewModel != null)
                SelectedPersonDetailViewModel.Load(SelectedPersonDetailViewModel.Model.Id);
        }

        private void OnPersonUpdated(UpdateMessage<PersonWrapper> message)
        {
            var personDetailViewModel = PersonDetailViewModels.SingleOrDefault(vm => vm.Model.Id == message.Model.Id);
            if (personDetailViewModel != null)
            {
                personDetailViewModel.Load(message.Model.Id);
            }
            if (SelectedFilmDetailViewModel != null)
                SelectedFilmDetailViewModel.Load(SelectedFilmDetailViewModel.Model.Id);
        }

        private void SelectFilm(Guid id)
        {
            var filmDetailViewModel = FilmDetailViewModels.SingleOrDefault(vm => vm.Model.Id == id);
            if (filmDetailViewModel == null)
            {
                filmDetailViewModel = _filmDetailViewModelFactory.Create();
                FilmDetailViewModels.Add(filmDetailViewModel);
            }
            filmDetailViewModel.Load(id);

            SelectedFilmDetailViewModel = filmDetailViewModel;
        }

        private void SelectPerson(Guid id)
        {
            var personDetailViewModel = PersonDetailViewModels.SingleOrDefault(vm => vm.Model.Id == id);
            if (personDetailViewModel == null)
            {
                personDetailViewModel = _personDetailViewModelFactory.Create();
                PersonDetailViewModels.Add(personDetailViewModel);
            }
            personDetailViewModel.Load(id);

            SelectedPersonDetailViewModel = personDetailViewModel;
        }

        private void OnFilmDeleted(DeleteMessage<FilmWrapper> message)
        {
            var film = FilmDetailViewModels.SingleOrDefault(i => i.Model.Id == message.Id);
            if (film != null)
            {
                FilmDetailViewModels.Remove(film);
                if (FilmDetailViewModels.Any())
                    _mediator.Send(new SelectedMessage<FilmWrapper> { Id = FilmDetailViewModels.Last().Model.Id });
            }
        }

        private void OnPersonDeleted(DeleteMessage<PersonWrapper> message)
        {
            var person = PersonDetailViewModels.SingleOrDefault(i => i.Model.Id == message.Id);
            if (person != null)
            {
                PersonDetailViewModels.Remove(person);
                if (PersonDetailViewModels.Any())
                    _mediator.Send(new SelectedMessage<PersonWrapper> { Id = PersonDetailViewModels.Last().Model.Id });
            }
        }

        private void OnCloseFilmDetailTabExecute(object parameter)
        {
            if (parameter is IFilmDetailViewModel filmDetailViewModel)
            {
                FilmDetailViewModels.Remove(filmDetailViewModel);
                if (FilmDetailViewModels.Any())
                    _mediator.Send(new SelectedMessage<FilmWrapper> { Id = FilmDetailViewModels.Last().Model.Id });
            }
        }

        private void OnClosePersonDetailTabExecute(object parameter)
        {
            if (parameter is IPersonDetailViewModel personDetailViewModel)
            {
                PersonDetailViewModels.Remove(personDetailViewModel);
                if (PersonDetailViewModels.Any())
                    _mediator.Send(new SelectedMessage<PersonWrapper> { Id = PersonDetailViewModels.Last().Model.Id });
            }
        }
    }
}