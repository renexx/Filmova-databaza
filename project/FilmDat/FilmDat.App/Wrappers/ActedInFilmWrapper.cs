using System;
using System.Collections.Generic;
using System.Text;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;

namespace FilmDat.App.Wrappers
{
    public class ActedInFilmWrapper : ModelWrapper<ActedInFilmDetailModel>
    {
        public ActedInFilmWrapper(ActedInFilmDetailModel model) : base(model)
        {
        }

        public Guid ActorId
        {
            get => GetValue<Guid>();
            set => SetValue(value);
        }

        public string FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public Guid FilmId
        {
            get => GetValue<Guid>();
            set => SetValue(value);
        }

        public string OriginalName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public static implicit operator ActedInFilmWrapper(ActedInFilmDetailModel model) =>
            new ActedInFilmWrapper(model);

        public static implicit operator ActedInFilmDetailModel(ActedInFilmWrapper wrapper) => wrapper.Model;
    }
}