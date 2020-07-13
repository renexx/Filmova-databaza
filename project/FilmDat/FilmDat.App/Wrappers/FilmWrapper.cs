using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using FilmDat.BL.Mapper;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;
using FilmDat.DAL.Enums;

namespace FilmDat.App.Wrappers
{
    public class FilmWrapper : ModelWrapper<FilmDetailModel>
    {
        public FilmWrapper(FilmDetailModel model) : base(model)
        {
            InitializeCollectionProperties(model);
        }

        public string OriginalName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string CzechName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public GenreEnum Genre
        {
            get => GetValue<GenreEnum>();
            set => SetValue(value);
        }

        public string TitleFotoUrl
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Country
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public TimeSpan Duration
        {
            get => GetValue<TimeSpan>();
            set => SetValue(value);
        }

        public string Description
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string AvgRating
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        private void InitializeCollectionProperties(FilmDetailModel model)
        {
            if (model.Actors == null)
            {
                throw new ArgumentException("Actors cannot be null");
            }

            Actors = new ObservableCollection<ActedInFilmWrapper>(
                model.Actors.Select(e => new ActedInFilmWrapper(e)));
            RegisterCollection(Actors, model.Actors);

            if (model.Directors == null)
            {
                throw new ArgumentException("Directors cannot be null");
            }

            Directors = new ObservableCollection<DirectedFilmWrapper>(
                model.Directors.Select(e => new DirectedFilmWrapper(e)));
            RegisterCollection(Directors, model.Directors);

            if (model.Reviews == null)
                throw new ArgumentException("Reviews cannot be null");
            Reviews = new ObservableCollection<ReviewListWrapper>(
                model.Reviews.Select(e => new ReviewListWrapper(e)));
            RegisterCollection(Reviews, model.Reviews);
        }

        public ObservableCollection<ActedInFilmWrapper> Actors { get; set; }
        public ObservableCollection<DirectedFilmWrapper> Directors { get; set; }
        public ObservableCollection<ReviewListWrapper> Reviews { get; set; }

        public static implicit operator FilmWrapper(FilmDetailModel model) => new FilmWrapper(model);
        public static implicit operator FilmDetailModel(FilmWrapper wrapper) => wrapper.Model;
    }
}