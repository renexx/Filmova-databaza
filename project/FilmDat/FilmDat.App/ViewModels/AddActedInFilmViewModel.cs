using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using FilmDat.App.Commands;
using FilmDat.App.Services;
using FilmDat.App.ViewModels.Interfaces;
using FilmDat.App.Wrappers;
using FilmDat.BL.Extensions;
using FilmDat.BL.Interfaces;
using FilmDat.BL.Messages;
using FilmDat.BL.Models.ListModels;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Services;

namespace FilmDat.App.ViewModels
{
    public class AddActedInFilmViewModel : ViewModelBase, IAddActedInFilmViewModel
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IActedInFilmRepository _actedInFilmRepository;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMediator _mediator;

        public AddActedInFilmViewModel(
            IFilmRepository filmRepository,
            IPersonRepository personRepository,
            IActedInFilmRepository actedInFilmRepository,
            IMessageDialogService messageDialogService,
            IMediator mediator)
        {
            _filmRepository = filmRepository;
            _personRepository = personRepository;
            _actedInFilmRepository = actedInFilmRepository;
            _messageDialogService = messageDialogService;
            _mediator = mediator;

            AddFilmSelectedCommand = new RelayCommand<FilmListModel>(AddActedInFilmSelected);
            DeleteFilmSelectedCommand = new RelayCommand<FilmListModel>(DeleteActedInFilmSelected);
            AddActedFilm = new RelayCommand(Add, CanAdd);
            DeleteActedFilm = new RelayCommand(Delete, CanDelete);

            mediator.Register<NewMessage<PersonWrapper>>(OnPersonNewMessage);
            mediator.Register<SelectedMessage<PersonWrapper>>(PersonSelected);
            mediator.Register<UpdateMessage<PersonWrapper>>(PersonUpdated);
        }

        public Guid AddFilmId { get; set; }
        public Guid DeleteFilmId { get; set; }
        public Guid ActorId { get; set; }
        public ActedInFilmWrapper Model { get; set; }

        public ObservableCollection<FilmListModel> AddFilms { get; } = new ObservableCollection<FilmListModel>();

        public ObservableCollection<FilmListModel> DeleteFilms { get; } = new ObservableCollection<FilmListModel>();

        public FilmListModel AddSelectedFilm { get; set; }
        public FilmListModel DeleteSelectedFilm { get; set; }

        public ICommand AddFilmSelectedCommand { get; }
        public ICommand DeleteFilmSelectedCommand { get; }
        public ICommand AddActedFilm { get; }
        public ICommand DeleteActedFilm { get; }

        private void OnPersonNewMessage(NewMessage<PersonWrapper> _)
        {
            ActorId = Guid.Empty;
            Load();
        }

        private void PersonSelected(SelectedMessage<PersonWrapper> message)
        {
            ActorId = message.Id;
            Load();
        }

        private void PersonUpdated(UpdateMessage<PersonWrapper> message)
        {
            ActorId = message.Model.Id;
            Load();
        }

        private void AddActedInFilmSelected(FilmListModel film) => AddSelectActedInFilm(film.Id);
        private void DeleteActedInFilmSelected(FilmListModel film) => DeleteSelectActedInFilm(film.Id);

        private bool CanAdd() =>
            ActorId != Guid.Empty
            && AddFilmId != Guid.Empty;

        private void Add()
        {
            var actor = _personRepository.GetById(ActorId);
            var film = _filmRepository.GetById(AddFilmId);

            Model = new ActedInFilmDetailModel()
            {
                ActorId = ActorId,
                FirstName = actor.FirstName,
                LastName = actor.LastName,
                FilmId = AddFilmId,
                OriginalName = film.OriginalName
            };

            Model = _actedInFilmRepository.InsertOrUpdate(Model);

            _mediator.Send(new UpdateMessage<FilmWrapper> { Model = film });
            _mediator.Send(new UpdateMessage<PersonWrapper> { Model = actor });
        }

        private bool CanDelete() =>
            ActorId != Guid.Empty
            && DeleteFilmId != Guid.Empty;

        private void Delete()
        {
            var actor = _personRepository.GetById(ActorId);

            Model = new ActedInFilmDetailModel();
            Model = actor.ActedInFilms.Single(film => film.FilmId == DeleteFilmId);

            var delete = _messageDialogService.Show(
                $"Delete",
                $"Do you want to delete film {Model?.OriginalName} from actor {Model.FirstName} {Model.LastName}?",
                MessageDialogButtonConfiguration.YesNo, MessageDialogResult.No);
            if (delete == MessageDialogResult.No)
            {
                return;
            }

            try
            {
                _actedInFilmRepository.Delete(Model.Id);
            }
            catch
            {
                var _ = _messageDialogService.Show(
                    $"Deleting of film {Model?.OriginalName} from actor {Model.FirstName} {Model.LastName} failed!",
                    "Deleting failed",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
                return;
            }

            _mediator.Send(new UpdateMessage<FilmWrapper> { Model = _filmRepository.GetById(DeleteFilmId) });
            _mediator.Send(new UpdateMessage<PersonWrapper> { Model = actor });
        }

        private void AddSelectActedInFilm(Guid Id)
        {
            if (AddFilms.Any())
            {
                if (Id != Guid.Empty)
                    AddSelectedFilm = AddFilms.SingleOrDefault(film => film.Id == Id);

                if (AddSelectedFilm == null)
                    AddFilmId = Guid.Empty;
                else
                    AddFilmId = AddSelectedFilm.Id;
            }
            else
                AddFilmId = Guid.Empty;
        }

        private void DeleteSelectActedInFilm(Guid Id)
        {
            if (DeleteFilms.Any())
            {
                if (Id != Guid.Empty)
                    DeleteSelectedFilm = DeleteFilms.SingleOrDefault(film => film.Id == Id);

                if (DeleteSelectedFilm == null)
                    DeleteFilmId = Guid.Empty;
                else
                    DeleteFilmId = DeleteSelectedFilm.Id;
            }
            else
                DeleteFilmId = Guid.Empty;
        }

        public void Load()
        {
            AddFilms.Clear();
            DeleteFilms.Clear();
            var films = _filmRepository.GetAll();
            if (ActorId == Guid.Empty)
            {
                AddFilms.AddRange(films);
                AddSelectActedInFilm(AddFilmId);
                DeleteSelectActedInFilm(DeleteFilmId);
                return;
            }

            var actorListOfActedInFilms = _personRepository.GetById(ActorId).ActedInFilms;
            var actedInFilms = new List<FilmListModel>();
            var notActedInFilms = new List<FilmListModel>();

            foreach (var film in films)
            {
                var canAdd = true;
                foreach (var actedInFilm in actorListOfActedInFilms)
                    if (film.Id == actedInFilm.FilmId)
                    {
                        actedInFilms.Add(film);
                        canAdd = false;
                        break;
                    }

                if (canAdd)
                    notActedInFilms.Add(film);
            }

            AddFilms.AddRange(notActedInFilms);
            DeleteFilms.AddRange(actedInFilms);
            AddSelectActedInFilm(AddFilmId);
            DeleteSelectActedInFilm(DeleteFilmId);
        }
    }
}