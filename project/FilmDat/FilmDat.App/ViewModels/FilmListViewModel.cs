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
    public class FilmListViewModel : ViewModelBase, IFilmListViewModel
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IMediator _mediator;

        public FilmListViewModel(IFilmRepository filmRepository, IMediator mediator)
        {
            _filmRepository = filmRepository;
            _mediator = mediator;

            FilmNewCommand = new RelayCommand(FilmNew);
            FilmSelectedCommand = new RelayCommand<FilmListModel>(FilmSelected);

            mediator.Register<UpdateMessage<FilmWrapper>>(FilmUpdated);
            mediator.Register<DeleteMessage<FilmWrapper>>(FilmDeleted);
        }

        public ICommand FilmNewCommand { get; }
        public ICommand FilmSelectedCommand { get; }

        public ObservableCollection<FilmListModel> Films { get; } = new ObservableCollection<FilmListModel>();

        private void FilmUpdated(UpdateMessage<FilmWrapper> _) => Load();
        private void FilmDeleted(DeleteMessage<FilmWrapper> _) => Load();

        private void FilmNew() => _mediator.Send(new NewMessage<FilmWrapper>());

        private void FilmSelected(FilmListModel film) =>
            _mediator.Send(new SelectedMessage<FilmWrapper> {Id = film.Id});

        public void Load()
        {
            Films.Clear();
            var films = _filmRepository.GetAll();
            Films.AddRange(films);
        }

        public override void LoadInDesignMode()
        {
            Films.Add(new FilmListModel {OriginalName = "Interstellar"});
            Films.Add(new FilmListModel {OriginalName = "Aladin"});
        }
    }
}