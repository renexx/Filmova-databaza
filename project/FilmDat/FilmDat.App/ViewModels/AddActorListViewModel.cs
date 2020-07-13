using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using FilmDat.App.Commands;
using FilmDat.App.Services;
using FilmDat.App.ViewModels.Interfaces;
using FilmDat.App.Wrappers;
using FilmDat.BL.Extensions;
using FilmDat.BL.Interfaces;
using FilmDat.BL.Messages;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;
using FilmDat.BL.Services;
using Microsoft.EntityFrameworkCore;

namespace FilmDat.App.ViewModels
{
    public class AddActorListViewModel : ViewModelBase, IAddActorListViewModel
    {
        private readonly IPersonRepository _personRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly IActedInFilmRepository _actedInFilmRepository;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMediator _mediator;

        public AddActorListViewModel(
            IPersonRepository personRepository,
            IFilmRepository filmRepository,
            IActedInFilmRepository actedInFilmRepository,
            IMessageDialogService messageDialogService,
            IMediator mediator)
        {
            _personRepository = personRepository;
            _filmRepository = filmRepository;
            _actedInFilmRepository = actedInFilmRepository;
            _messageDialogService = messageDialogService;
            _mediator = mediator;

            AddActorSelectedCommand = new RelayCommand<PersonListModel>(AddActorSelected);
            DeleteActorSelectedCommand = new RelayCommand<PersonListModel>(DeleteActorSelected);
            AddActor = new RelayCommand(Add, CanAdd);
            DeleteActor = new RelayCommand(Delete, CanDelete);

            mediator.Register<NewMessage<FilmWrapper>>(OnFilmNewMessage);
            mediator.Register<SelectedMessage<FilmWrapper>>(FilmSelected);
            mediator.Register<UpdateMessage<FilmWrapper>>(FilmUpdated);
        }

        public Guid FilmId { get; set; }
        public Guid AddActorId { get; set; }
        public Guid DeleteActorId { get; set; }
        public ActedInFilmWrapper Model { get; set; }

        public ObservableCollection<PersonListModel> AddActors { get; } = new ObservableCollection<PersonListModel>();

        public ObservableCollection<PersonListModel> DeleteActors { get; } = new ObservableCollection<PersonListModel>();

        public PersonListModel AddSelectedActor { get; set; }
        public PersonListModel DeleteSelectedActor { get; set; }

        public ICommand AddActorSelectedCommand { get; }
        public ICommand DeleteActorSelectedCommand { get; }
        public ICommand AddActor { get; }
        public ICommand DeleteActor { get; }

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

        private void AddActorSelected(PersonListModel actor) => AddSelectActor(actor.Id);
        private void DeleteActorSelected(PersonListModel actor) => DeleteSelectActor(actor.Id);

        private bool CanAdd() =>
            FilmId != Guid.Empty
            && AddActorId != Guid.Empty;

        private void Add()
        {
            var film = _filmRepository.GetById(FilmId);
            var actor = _personRepository.GetById(AddActorId);

            Model = new ActedInFilmDetailModel()
            {
                ActorId = AddActorId,
                FirstName = actor.FirstName,
                LastName = actor.LastName,
                FilmId = FilmId,
                OriginalName = film.OriginalName
            };

            Model = _actedInFilmRepository.InsertOrUpdate(Model);

            _mediator.Send(new UpdateMessage<PersonWrapper> { Model = actor });
            _mediator.Send(new UpdateMessage<FilmWrapper> { Model = film });
        }

        private bool CanDelete() =>
            FilmId != Guid.Empty
            && DeleteActorId != Guid.Empty;

        private void Delete()
        {
            var film = _filmRepository.GetById(FilmId);

            Model = new ActedInFilmDetailModel();
            Model = film.Actors.Single(actor => actor.ActorId == DeleteActorId);

            var delete = _messageDialogService.Show(
                $"Delete",
                $"Do you want to delete actor {Model.FirstName} {Model.LastName} from film {Model?.OriginalName}?",
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
                    $"Deleting of actor {Model.FirstName} {Model.LastName} from film {Model?.OriginalName} failed!",
                    "Deleting failed",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
                return;
            }

            _mediator.Send(new UpdateMessage<PersonWrapper> { Model = _personRepository.GetById(DeleteActorId) });
            _mediator.Send(new UpdateMessage<FilmWrapper> { Model = film });
        }

        private void AddSelectActor(Guid Id)
        {
            if (AddActors.Any())
            {
                if (Id != Guid.Empty)
                    AddSelectedActor = AddActors.SingleOrDefault(actor => actor.Id == Id);

                if (AddSelectedActor == null)
                    AddActorId = Guid.Empty;
                else
                    AddActorId = AddSelectedActor.Id;
            }
            else
                AddActorId = Guid.Empty;
        }

        private void DeleteSelectActor(Guid Id)
        {
            if (DeleteActors.Any())
            {
                if (Id != Guid.Empty)
                    DeleteSelectedActor = DeleteActors.SingleOrDefault(actor => actor.Id == Id);

                if (DeleteSelectedActor == null)
                    DeleteActorId = Guid.Empty;
                else
                    DeleteActorId = DeleteSelectedActor.Id;
            }
            else
                DeleteActorId = Guid.Empty;
        }

        public void Load()
        {
            AddActors.Clear();
            DeleteActors.Clear();
            var persons = _personRepository.GetAll();
            if (FilmId == Guid.Empty)
            {
                AddActors.AddRange(persons);
                AddSelectActor(AddActorId);
                DeleteSelectActor(DeleteActorId);
                return;
            }

            var filmListOfActors = _filmRepository.GetById(FilmId).Actors;
            var actorsInFilm = new List<PersonListModel>();
            var actorsNotInFilm = new List<PersonListModel>();

            foreach (var person in persons)
            {
                var canAdd = true;
                foreach (var actor in filmListOfActors)
                    if (person.Id == actor.ActorId)
                    {
                        actorsInFilm.Add(person);
                        canAdd = false;
                        break;
                    }

                if (canAdd)
                    actorsNotInFilm.Add(person);
            }

            AddActors.AddRange(actorsNotInFilm);
            DeleteActors.AddRange(actorsInFilm);
            AddSelectActor(AddActorId);
            DeleteSelectActor(DeleteActorId);
        }
    }
}