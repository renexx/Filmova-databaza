using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FilmDat.App.Commands;
using FilmDat.App.Services;
using FilmDat.App.ViewModels.Interfaces;
using FilmDat.App.Wrappers;
using FilmDat.BL.Interfaces;
using FilmDat.BL.Messages;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;
using FilmDat.BL.Services;

namespace FilmDat.App.ViewModels
{
    public class PersonDetailViewModel : ViewModelBase, IPersonDetailViewModel
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMediator _mediator;

        public PersonDetailViewModel(
            IPersonRepository personRepository,
            IMessageDialogService messageDialogService,
            IAddActedInFilmViewModel addActedInFilmViewModel,
            IAddDirectedFilmViewModel addDirectedFilmViewModel,
            IMediator mediator)
        {
            _personRepository = personRepository;
            _mediator = mediator;
            _messageDialogService = messageDialogService;

            AddActedInFilmViewModel = addActedInFilmViewModel;
            AddDirectedFilmViewModel = addDirectedFilmViewModel;

            SaveCommand = new RelayCommand(Save, CanSave);
            DeleteCommand = new RelayCommand(Delete);
            ActedInFilmSelectedCommand = new RelayCommand<ActedInFilmWrapper>(OnActedInFilmSelected);
            DirectedFilmSelectedCommand = new RelayCommand<DirectedFilmWrapper>(OnDirectedFilmSelected);
        }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ActedInFilmSelectedCommand { get; set; }
        public ICommand DirectedFilmSelectedCommand { get; set; }

        public IAddActedInFilmViewModel AddActedInFilmViewModel { get; }
        public IAddDirectedFilmViewModel AddDirectedFilmViewModel { get; }

        public PersonWrapper Model { get; set; }

        private void OnActedInFilmSelected(ActedInFilmWrapper film) =>
            _mediator.Send(new SelectedMessage<FilmWrapper> { Id = film.Model.FilmId });

        private void OnDirectedFilmSelected(DirectedFilmWrapper film) =>
            _mediator.Send(new SelectedMessage<FilmWrapper> { Id = film.Model.FilmId });

        public void Load(Guid id)
        {
            Model = _personRepository.GetById(id) ?? new PersonDetailModel()
            {
                BirthDate = DateTime.Now,
                ActedInFilms = new List<ActedInFilmDetailModel>(),
                DirectedFilms = new List<DirectedFilmDetailModel>()
            };
        }

        private bool CanSave() =>
            Model != null
            && !string.IsNullOrWhiteSpace(Model.FirstName)
            && !string.IsNullOrWhiteSpace(Model.LastName);

        public void Save()
        {
            Model = _personRepository.InsertOrUpdate(Model.Model);
            _mediator.Send(new UpdateMessage<PersonWrapper> { Model = Model });
            Load(Model.Id);
        }

        public void Delete()
        {
            if (Model.Id != Guid.Empty)
            {
                var delete = _messageDialogService.Show(
                    $"Delete",
                    $"Do you want to delete {Model?.FirstName}?",
                    MessageDialogButtonConfiguration.YesNo,
                    MessageDialogResult.No);

                if (delete == MessageDialogResult.No)
                    return;

                try
                {
                    _personRepository.Delete(Model.Id);
                }
                catch
                {
                    var _ = _messageDialogService.Show(
                        $"Deleting of {Model?.FirstName} failed!",
                        "Deleting failed",
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
                    return;
                }

                _mediator.Send(new DeleteMessage<PersonWrapper>
                {
                    Model = Model
                });
            }
        }

        public override void LoadInDesignMode()
        {
            base.LoadInDesignMode();
            Model = new PersonWrapper(new PersonDetailModel()
            {
                FirstName = "Janko",
                LastName = "Lehotský",
                BirthDate = new DateTime(1980, 12, 5),
                FotoUrl = "https://www.pngitem.com/pimgs/m/40-406527_cartoon-glass-of-water-png-glass-of-water.png"
            });
        }
    }
}