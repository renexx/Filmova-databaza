using System;
using System.Collections.Generic;
using System.Text;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;

namespace FilmDat.App.Wrappers
{
    public class DirectedFilmWrapper : ModelWrapper<DirectedFilmDetailModel>
    {
        public DirectedFilmWrapper(DirectedFilmDetailModel model) : base(model)
        {
        }

        public Guid DirectorId
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

        public static implicit operator DirectedFilmWrapper(DirectedFilmDetailModel model) =>
            new DirectedFilmWrapper(model);

        public static implicit operator DirectedFilmDetailModel(DirectedFilmWrapper wrapper) => wrapper.Model;
    }
}