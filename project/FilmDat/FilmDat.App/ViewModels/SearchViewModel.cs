using System;
using System.Windows.Input;
using FilmDat.App.ViewModels.Interfaces;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using FilmDat.App.Commands;
using FilmDat.App.Views;
using FilmDat.App.Wrappers;
using FilmDat.BL.Extensions;
using FilmDat.BL.Interfaces;
using FilmDat.BL.Messages;
using FilmDat.BL.Models.ListModels;
using FilmDat.BL.Services;

namespace FilmDat.App.ViewModels
{
    public class SearchViewModel : ISearchViewModel
    {
        private readonly IMediator _mediator;
        private readonly IFilmRepository _filmRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IReviewRepository _reviewRepository;

        public SearchViewModel(IMediator mediator, IFilmRepository filmRepository, IPersonRepository personRepository,
            IReviewRepository reviewRepository)
        {
            _mediator = mediator;
            _filmRepository = filmRepository;
            _personRepository = personRepository;
            _reviewRepository = reviewRepository;

            Search = new RelayCommand<string>(SearchDatabase);
            Clear = new RelayCommand(ClearSearch);
            FilmSelectedCommand = new RelayCommand<FilmListModel>(OnFilmSelected);
            PersonSelectedCommand = new RelayCommand<PersonListModel>(OnPersonSelected);
            ReviewSelectedCommand = new RelayCommand<ReviewListModel>(OnReviewSelected);
        }

        public ICommand Search { get; set; }
        public ICommand Clear { get; set; }
        public ICommand FilmSelectedCommand { get; set; }
        public ICommand PersonSelectedCommand { get; set; }
        public ICommand ReviewSelectedCommand { get; set; }

        public ObservableCollection<FilmListModel> Films { get; } = new ObservableCollection<FilmListModel>();
        public ObservableCollection<PersonListModel> Persons { get; } = new ObservableCollection<PersonListModel>();
        public ObservableCollection<ReviewListModel> Reviews { get; } = new ObservableCollection<ReviewListModel>();

        // Source: http://www.levibotelho.com/development/c-remove-diacritics-accents-from-a-string/
        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        // Source: https://stackoverflow.com/a/14591148
        private string RemoveWhitespace(string text) => new string(text.Where(c => !char.IsWhiteSpace(c)).ToArray());

        private string PrepareString(string text) => RemoveDiacritics(RemoveWhitespace(text));

        private void SearchDatabase(string text)
        {
            Films.Clear();
            Persons.Clear();
            Reviews.Clear();

            if (String.IsNullOrWhiteSpace(text)) return;
            text = PrepareString(text);

            var films = _filmRepository.GetAll();
            foreach (var film in films)
            {
                var detail = _filmRepository.GetById(film.Id);

                if (PrepareString(detail.OriginalName).Contains(text, StringComparison.CurrentCultureIgnoreCase)
                    || PrepareString(detail.CzechName).Contains(text, StringComparison.CurrentCultureIgnoreCase)
                    || PrepareString(detail.Country).Contains(text, StringComparison.CurrentCultureIgnoreCase)
                    || PrepareString(detail.Description).Contains(text, StringComparison.CurrentCultureIgnoreCase))
                    Films.Add(film);
            }

            var persons = _personRepository.GetAll();
            foreach (var person in persons)
            {
                var detail = _personRepository.GetById(person.Id);
                var name = detail.FirstName + detail.LastName;

                if (PrepareString(name).Contains(text, StringComparison.CurrentCultureIgnoreCase))
                    Persons.Add(person);
            }

            var reviews = _reviewRepository.GetAll();
            foreach (var review in reviews)
            {
                var detail = _reviewRepository.GetById(review.Id);

                if (PrepareString(detail.TextReview).Contains(text, StringComparison.CurrentCultureIgnoreCase))
                    Reviews.Add(review);
            }
        }

        private void ClearSearch()
        {
            Films.Clear();
            Persons.Clear();
            Reviews.Clear();
        }

        private void OnFilmSelected(FilmListModel film) =>
            _mediator.Send(new SelectedMessage<FilmWrapper> {Id = film.Id});

        private void OnPersonSelected(PersonListModel person) =>
            _mediator.Send(new SelectedMessage<PersonWrapper> {Id = person.Id});

        private void OnReviewSelected(ReviewListModel review)
        {
            _mediator.Send(new SelectedMessage<FilmWrapper> {Id = review.FilmId});
            _mediator.Send(new SelectedMessage<ReviewDetailWrapper> {Id = review.Id});
        }

        public void Load()
        {
            return;
        }

        public void LoadInDesignMode()
        {
            return;
        }
    }
}