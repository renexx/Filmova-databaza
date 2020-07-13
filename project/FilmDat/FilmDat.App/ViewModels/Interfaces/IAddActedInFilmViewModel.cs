using System;

namespace FilmDat.App.ViewModels.Interfaces
{
    public interface IAddActedInFilmViewModel : IListViewModel
    {
        public Guid AddFilmId { get; set; }
        public Guid DeleteFilmId { get; set; }
        public Guid ActorId { get; set; }
    }
}