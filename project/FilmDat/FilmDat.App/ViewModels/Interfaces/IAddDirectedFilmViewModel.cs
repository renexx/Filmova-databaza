using System;

namespace FilmDat.App.ViewModels.Interfaces
{
    public interface IAddDirectedFilmViewModel : IListViewModel
    {
        public Guid AddFilmId { get; set; }
        public Guid DeleteFilmId { get; set; }
        public Guid DirectorId { get; set; }
    }
}