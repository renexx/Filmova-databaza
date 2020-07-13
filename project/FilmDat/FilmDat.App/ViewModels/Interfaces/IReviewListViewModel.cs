using System;
using System.Collections.Generic;
using System.Text;

namespace FilmDat.App.ViewModels.Interfaces
{
    public interface IReviewListViewModel : IListViewModel
    {
        Guid FilmId { get; set; }
    }
}