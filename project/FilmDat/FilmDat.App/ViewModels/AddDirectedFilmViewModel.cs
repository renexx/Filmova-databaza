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
    public class AddDirectedFilmViewModel : ViewModelBase, IAddDirectedFilmViewModel
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IDirectedFilmRepository _directedFilmRepository;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMediator _mediator;

        public AddDirectedFilmViewModel(
            IFilmRepository filmRepository,
            IPersonRepository personRepository,
            IDirectedFilmRepository directedFilmRepository,
            IMessageDialogService messageDialogService,
            IMediator mediator)
        {
            _filmRepository = filmRepository;
            _personRepository = personRepository;
            _directedFilmRepository = directedFilmRepository;
            _messageDialogService = messageDialogService;
            _mediator = mediator;

            AddFilmSelectedCommand = new RelayCommand<FilmListModel>(AddDirectedFilmSelected);
            DeleteFilmSelectedCommand = new RelayCommand<FilmListModel>(DeleteDirectedFilmSelected);
            AddDirectedFilm = new RelayCommand(Add, CanAdd);
            DeleteDirectedFilm = new RelayCommand(Delete, CanDelete);

            mediator.Register<NewMessage<PersonWrapper>>(OnPersonNewMessage);
            mediator.Register<SelectedMessage<PersonWrapper>>(PersonSelected);
            mediator.Register<UpdateMessage<PersonWrapper>>(PersonUpdated);
        }

        public Guid AddFilmId { get; set; }
        public Guid DeleteFilmId { get; set; }
        public Guid DirectorId { get; set; }
        public DirectedFilmWrapper Model { get; set; }

        public ObservableCollection<FilmListModel> AddFilms { get; } = new ObservableCollection<FilmListModel>();

        public ObservableCollection<FilmListModel> DeleteFilms { get; } = new ObservableCollection<FilmListModel>();

        public FilmListModel AddSelectedFilm { get; set; }
        public FilmListModel DeleteSelectedFilm { get; set; }

        public ICommand AddFilmSelectedCommand { get; }
        public ICommand DeleteFilmSelectedCommand { get; }
        public ICommand AddDirectedFilm { get; }
        public ICommand DeleteDirectedFilm { get; }

        private void OnPersonNewMessage(NewMessage<PersonWrapper> _)
        {
            DirectorId = Guid.Empty;
            Load();
        }

        private void PersonSelected(SelectedMessage<PersonWrapper> message)
        {
            DirectorId = message.Id;
            Load();
        }

        private void PersonUpdated(UpdateMessage<PersonWrapper> message)
        {
            DirectorId = message.Model.Id;
            Load();
        }

        private void AddDirectedFilmSelected(FilmListModel film) => AddSelectDirectedFilm(film.Id);
        private void DeleteDirectedFilmSelected(FilmListModel film) => DeleteSelectDirectedFilm(film.Id);

        private bool CanAdd() =>
            DirectorId != Guid.Empty
            && AddFilmId != Guid.Empty;

        private void Add()
        {
            var director = _personRepository.GetById(DirectorId);
            var film = _filmRepository.GetById(AddFilmId);

            Model = new DirectedFilmDetailModel()
            {
                DirectorId = DirectorId,
                FirstName = director.FirstName,
                LastName = director.LastName,
                FilmId = AddFilmId,
                OriginalName = film.OriginalName
            };

            Model = _directedFilmRepository.InsertOrUpdate(Model);

            _mediator.Send(new UpdateMessage<FilmWrapper> { Model = film });
            _mediator.Send(new UpdateMessage<PersonWrapper> { Model = director });
        }

        private bool CanDelete() =>
            DirectorId != Guid.Empty
            && DeleteFilmId != Guid.Empty;

        private void Delete()
        {
            var director = _personRepository.GetById(DirectorId);

            Model = new DirectedFilmDetailModel();
            Model = director.DirectedFilms.Single(film => film.FilmId == DeleteFilmId);

            var delete = _messageDialogService.Show(
                $"Delete",
                $"Do you want to delete film {Model?.OriginalName} from director {Model.FirstName} {Model.LastName}?",
                MessageDialogButtonConfiguration.YesNo, MessageDialogResult.No);
            if (delete == MessageDialogResult.No)
            {
                return;
            }

            try
            {
                _directedFilmRepository.Delete(Model.Id);
            }
            catch
            {
                var _ = _messageDialogService.Show(
                    $"Deleting of film {Model?.OriginalName} from director {Model.FirstName} {Model.LastName} failed!",
                    "Deleting failed",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
                return;
            }

            _mediator.Send(new UpdateMessage<FilmWrapper> { Model = _filmRepository.GetById(DeleteFilmId) });
            _mediator.Send(new UpdateMessage<PersonWrapper> { Model = director });
        }

        private void AddSelectDirectedFilm(Guid Id)
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

        private void DeleteSelectDirectedFilm(Guid Id)
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
            if (DirectorId == Guid.Empty)
            {
                AddFilms.AddRange(films);
                AddSelectDirectedFilm(AddFilmId);
                DeleteSelectDirectedFilm(DeleteFilmId);
                return;
            }

            var directorListOfDirectedFilms = _personRepository.GetById(DirectorId).DirectedFilms;
            var directedFilms = new List<FilmListModel>();
            var notDirectedFilms = new List<FilmListModel>();

            foreach (var film in films)
            {
                var canAdd = true;
                foreach (var directedFilm in directorListOfDirectedFilms)
                    if (film.Id == directedFilm.FilmId)
                    {
                        directedFilms.Add(film);
                        canAdd = false;
                        break;
                    }

                if (canAdd)
                    notDirectedFilms.Add(film);
            }

            AddFilms.AddRange(notDirectedFilms);
            DeleteFilms.AddRange(directedFilms);
            AddSelectDirectedFilm(AddFilmId);
            DeleteSelectDirectedFilm(DeleteFilmId);
        }
    }
}