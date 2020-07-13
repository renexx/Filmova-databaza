using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using FilmDat.BL.Models.DetailModels;

namespace FilmDat.App.Wrappers
{
    public class PersonWrapper : ModelWrapper<PersonDetailModel>
    {
        public PersonWrapper(PersonDetailModel model) : base(model)
        {
            InitializeCollectionProperties(model);
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

        public DateTime BirthDate
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public string FotoUrl
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        private void InitializeCollectionProperties(PersonDetailModel model)
        {
            if (model.ActedInFilms == null)
                throw new ArgumentException("List of films cannot be null");
            ActedInFilms = new ObservableCollection<ActedInFilmWrapper>(
                model.ActedInFilms.Select(e => new ActedInFilmWrapper(e)));
            RegisterCollection(ActedInFilms, model.ActedInFilms);

            if (model.DirectedFilms == null)
                throw new ArgumentException("List of films cannot be null");
            DirectedFilms = new ObservableCollection<DirectedFilmWrapper>(
                model.DirectedFilms.Select(e => new DirectedFilmWrapper(e)));
            RegisterCollection(DirectedFilms, model.DirectedFilms);
        }

        public ObservableCollection<ActedInFilmWrapper> ActedInFilms { get; set; }
        public ObservableCollection<DirectedFilmWrapper> DirectedFilms { get; set; }

        public static implicit operator PersonWrapper(PersonDetailModel model) => new PersonWrapper(model);
        public static implicit operator PersonDetailModel(PersonWrapper wrapper) => wrapper.Model;
    }
}