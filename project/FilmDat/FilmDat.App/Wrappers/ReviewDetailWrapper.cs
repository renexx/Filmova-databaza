using System;
using System.Collections.Generic;
using System.Text;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;

namespace FilmDat.App.Wrappers
{
    public class ReviewDetailWrapper : ModelWrapper<ReviewDetailModel>
    {
        public ReviewDetailWrapper(ReviewDetailModel model) : base(model)
        {
        }

        public string NickName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateTime Date
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
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

        public static implicit operator ReviewDetailWrapper(ReviewDetailModel model) => new ReviewDetailWrapper(model);
        public static implicit operator ReviewDetailModel(ReviewDetailWrapper wrapper) => wrapper.Model;
    }
}