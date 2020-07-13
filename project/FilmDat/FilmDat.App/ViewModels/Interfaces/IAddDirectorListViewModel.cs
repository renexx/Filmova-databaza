using System;

namespace FilmDat.App.ViewModels.Interfaces
{
    public interface IAddDirectorListViewModel : IListViewModel
    {
        public Guid FilmId { get; set; }
        public Guid AddDirectorId { get; set; }
        public Guid DeleteDirectorId { get; set; }
    }
}