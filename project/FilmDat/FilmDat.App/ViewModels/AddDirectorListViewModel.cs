using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using FilmDat.App.Commands;
using FilmDat.App.Services;
using FilmDat.App.Wrappers;
using FilmDat.BL.Extensions;
using FilmDat.BL.Interfaces;
using FilmDat.BL.Messages;
using FilmDat.BL.Models.ListModels;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Services;
using FilmDat.App.ViewModels.Interfaces;

namespace FilmDat.App.ViewModels
{
    public class AddDirectorListViewModel : ViewModelBase, IAddDirectorListViewModel
    {
        private readonly IPersonRepository _personRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly IDirectedFilmRepository _directedFilmRepository;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMediator _mediator;

        public AddDirectorListViewModel(
            IPersonRepository personRepository,
            IFilmRepository filmRepository,
            IDirectedFilmRepository directedFilmRepository,
            IMessageDialogService messageDialogService,
            IMediator mediator)
        {
            _personRepository = personRepository;
            _filmRepository = filmRepository;
            _directedFilmRepository = directedFilmRepository;
            _messageDialogService = messageDialogService;
            _mediator = mediator;

            AddDirectorSelectedCommand = new RelayCommand<PersonListModel>(AddDirectorSelected);
            DeleteDirectorSelectedCommand = new RelayCommand<PersonListModel>(DeleteDirectorSelected);
            AddDirector = new RelayCommand(Add, CanAdd);
            DeleteDirector = new RelayCommand(Delete, CanDelete);

            mediator.Register<NewMessage<FilmWrapper>>(OnFilmNewMessage);
            mediator.Register<SelectedMessage<FilmWrapper>>(FilmSelected);
            mediator.Register<UpdateMessage<FilmWrapper>>(FilmUpdated);
        }

        public Guid FilmId { get; set; }
        public Guid AddDirectorId { get; set; }
        public Guid DeleteDirectorId { get; set; }
        public DirectedFilmWrapper Model { get; set; }

        public ObservableCollection<PersonListModel> AddDirectors { get; } = new ObservableCollection<PersonListModel>();
        public ObservableCollection<PersonListModel> DeleteDirectors { get; } = new ObservableCollection<PersonListModel>();
        public PersonListModel AddSelectedDirector { get; set; }
        public PersonListModel DeleteSelectedDirector { get; set; }

        public ICommand AddDirectorSelectedCommand { get; }
        public ICommand DeleteDirectorSelectedCommand { get; }
        public ICommand AddDirector { get; }
        public ICommand DeleteDirector { get; }

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

        private void FilmUpdated(UpdateMessage<FilmWrapper> message)
        {
            FilmId = message.Model.Id;
            Load();
        }

        private void AddDirectorSelected(PersonListModel director) => AddSelectDirector(director.Id);
        private void DeleteDirectorSelected(PersonListModel director) => DeleteSelectDirector(director.Id);

        private bool CanAdd() =>
            FilmId != Guid.Empty
            && AddDirectorId != Guid.Empty;
        private void Add()
        {
            var film = _filmRepository.GetById(FilmId);
            var director = _personRepository.GetById(AddDirectorId);

            Model = new DirectedFilmDetailModel()
            {
                DirectorId = AddDirectorId,
                FirstName = director.FirstName,
                LastName = director.LastName,
                FilmId = FilmId,
                OriginalName = film.OriginalName
            };

            Model = _directedFilmRepository.InsertOrUpdate(Model);

            _mediator.Send(new UpdateMessage<PersonWrapper> { Model = director });
            _mediator.Send(new UpdateMessage<FilmWrapper> { Model = film });
        }

        private bool CanDelete() =>
            FilmId != Guid.Empty
            && DeleteDirectorId != Guid.Empty;
        private void Delete()
        {
            var film = _filmRepository.GetById(FilmId);

            Model = new DirectedFilmDetailModel();
            Model = film.Directors.Single(director => director.DirectorId == DeleteDirectorId);

            var delete = _messageDialogService.Show(
                $"Delete",
                $"Do you want to delete director {Model.FirstName} {Model.LastName} from film {Model?.OriginalName}?",
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
                    $"Deleting of director {Model.FirstName} {Model.LastName} from film {Model?.OriginalName} failed!",
                    "Deleting failed",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
                return;
            }

            _mediator.Send(new UpdateMessage<PersonWrapper> { Model = _personRepository.GetById(DeleteDirectorId) });
            _mediator.Send(new UpdateMessage<FilmWrapper> { Model = film });
        }

        private void AddSelectDirector(Guid Id)
        {
            if (AddDirectors.Any())
            {
                if (Id != Guid.Empty)
                    AddSelectedDirector = AddDirectors.SingleOrDefault(director => director.Id == Id);

                if (AddSelectedDirector == null)
                    AddDirectorId = Guid.Empty;
                else
                    AddDirectorId = AddSelectedDirector.Id;
            }
            else
                AddDirectorId = Guid.Empty;
        }
        private void DeleteSelectDirector(Guid Id)
        {
            if (DeleteDirectors.Any())
            {
                if (Id != Guid.Empty)
                    DeleteSelectedDirector = DeleteDirectors.SingleOrDefault(director => director.Id == Id);

                if (DeleteSelectedDirector == null)
                    DeleteDirectorId = Guid.Empty;
                else
                    DeleteDirectorId = DeleteSelectedDirector.Id;
            }
            else
                DeleteDirectorId = Guid.Empty;
        }

        public void Load()
        {
            AddDirectors.Clear();
            DeleteDirectors.Clear();
            var persons = _personRepository.GetAll();
            if (FilmId == Guid.Empty)
            {
                AddDirectors.AddRange(persons);
                AddSelectDirector(AddDirectorId);
                DeleteSelectDirector(DeleteDirectorId);
                return;
            }

            var filmListOfDirectors = _filmRepository.GetById(FilmId).Directors;
            var directorsInFilm = new List<PersonListModel>();
            var directorsNotInFilm = new List<PersonListModel>();

            foreach (var person in persons)
            {
                var canAdd = true;
                foreach (var director in filmListOfDirectors)
                    if (person.Id == director.DirectorId)
                    {
                        directorsInFilm.Add(person);
                        canAdd = false;
                        break;
                    }
                if (canAdd)
                    directorsNotInFilm.Add(person);
            }
            AddDirectors.AddRange(directorsNotInFilm);
            DeleteDirectors.AddRange(directorsInFilm);
            AddSelectDirector(AddDirectorId);
            DeleteSelectDirector(DeleteDirectorId);
        }
    }
}