using System;
using System.Collections.Generic;
using System.Text;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;

namespace FilmDat.App.Wrappers
{
    public class ReviewListWrapper : ModelWrapper<ReviewListModel>
    {
        public ReviewListWrapper(ReviewListModel model) : base(model)
        {
        }

        public uint Rating
        {
            get => GetValue<uint>();
            set => SetValue(value);
        }

        public string TextReview
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public Guid FilmId
        {
            get => GetValue<Guid>();
            set => SetValue(value);
        }

        public static implicit operator ReviewListWrapper(ReviewListModel model) => new ReviewListWrapper(model);
        public static implicit operator ReviewListModel(ReviewListWrapper wrapper) => wrapper.Model;
    }
}