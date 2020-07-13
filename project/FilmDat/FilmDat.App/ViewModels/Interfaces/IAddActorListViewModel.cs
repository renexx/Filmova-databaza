using System;

namespace FilmDat.App.ViewModels.Interfaces
{
    public interface IAddActorListViewModel : IListViewModel
    {
        public Guid FilmId { get; set; }
        public Guid AddActorId { get; set; }
        public Guid DeleteActorId { get; set; }
    }
}