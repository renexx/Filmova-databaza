using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class PersonListViewModel : ViewModelBase, IPersonListViewModel
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMediator _mediator;

        // Constructor
        public PersonListViewModel(IPersonRepository personRepository, IMediator mediator)
        {
            _personRepository = personRepository;
            _mediator = mediator;

            PersonNewCommand = new RelayCommand(PersonNew);
            PersonSelectedCommand = new RelayCommand<PersonListModel>(PersonSelected);

            mediator.Register<UpdateMessage<PersonWrapper>>(PersonUpdated);
            mediator.Register<DeleteMessage<PersonWrapper>>(PersonDeleted);
        }

        public ICommand PersonNewCommand { get; }
        public ICommand PersonSelectedCommand { get; }
        public ObservableCollection<PersonListModel> Persons { get; } = new ObservableCollection<PersonListModel>();

        private void PersonUpdated(UpdateMessage<PersonWrapper> _) => Load();
        private void PersonDeleted(DeleteMessage<PersonWrapper> _) => Load();

        private void PersonNew() => _mediator.Send(new NewMessage<PersonWrapper>());

        private void PersonSelected(PersonListModel person) =>
            _mediator.Send(new SelectedMessage<PersonWrapper> {Id = person.Id});

        public void Load()
        {
            Persons.Clear();
            var persons = _personRepository.GetAll();
            Persons.AddRange(persons);
        }

        public override void LoadInDesignMode()
        {
            Persons.Add(new PersonListModel {FirstName = "Mark", LastName = "Wahlberg"});
        }
    }
}