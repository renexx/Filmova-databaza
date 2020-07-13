using System;
using System.Collections.Generic;
using System.Text;
using FilmDat.App.Wrappers;

namespace FilmDat.App.ViewModels.Interfaces
{
    public interface IReviewDetailViewModel : IDetailViewModel<ReviewDetailWrapper>
    {
        Guid FilmId { get; set; }
    }
}