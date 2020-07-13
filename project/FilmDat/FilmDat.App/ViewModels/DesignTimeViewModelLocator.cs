using System.Security.Cryptography.Pkcs;
using FilmDat.App.Factories;
using FilmDat.App.Services;
using FilmDat.App.ViewModels.Interfaces;
using FilmDat.BL.Repositories;
using FilmDat.BL.Services;
using FilmDat.DAL.Factories;

namespace FilmDat.App.ViewModels
{
    public class DesignTimeViewModelLocator
    {
        public SearchViewModel SearchViewModel { get; }
        public FilmListViewModel FilmListViewModel { get; }
        public PersonListViewModel PersonListViewModel { get; }
        public ReviewListViewModel ReviewListViewModel { get; }
        public AddActedInFilmViewModel AddActedInFilmViewModel { get; }
        public AddDirectedFilmViewModel AddDirectedFilmViewModel { get; }
        public AddActorListViewModel AddActorListViewModel { get; }
        public AddDirectorListViewModel AddDirectorListViewModel { get; }

        public ReviewDetailViewModel ReviewDetailViewModel { get; set; }

        public DesignTimeViewModelLocator()
        {
            var filmRepository = new FilmRepository(new DesignTimeDbContextFactory());
            var personRepository = new PersonRepository(new DesignTimeDbContextFactory());
            var reviewRepository = new ReviewRepository(new DesignTimeDbContextFactory());
            var actedInFilmRepository = new ActedInFilmRepository(new DesignTimeDbContextFactory());
            var directedFilmRepository = new DirectedFilmRepository(new DesignTimeDbContextFactory());

            var mediator = new Mediator();
            var messageDialogService = new MessageDialogService();

            SearchViewModel = new SearchViewModel(mediator, filmRepository, personRepository, reviewRepository);

            FilmListViewModel = new FilmListViewModel(filmRepository, mediator);
            PersonListViewModel = new PersonListViewModel(personRepository, mediator);
            ReviewListViewModel = new ReviewListViewModel(reviewRepository, mediator);

            AddActedInFilmViewModel = new AddActedInFilmViewModel(filmRepository, personRepository, actedInFilmRepository, messageDialogService, mediator);
            AddActorListViewModel = new AddActorListViewModel(personRepository, filmRepository, actedInFilmRepository, messageDialogService, mediator);
            AddDirectedFilmViewModel = new AddDirectedFilmViewModel(filmRepository, personRepository, directedFilmRepository, messageDialogService, mediator);
            AddDirectorListViewModel = new AddDirectorListViewModel(personRepository, filmRepository, directedFilmRepository, messageDialogService, mediator);

            ReviewDetailViewModel = new ReviewDetailViewModel(reviewRepository, mediator);
        }
    }
}