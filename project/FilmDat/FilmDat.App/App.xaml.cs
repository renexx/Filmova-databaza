using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FilmDat.App.Factories;
using FilmDat.App.Services;
using FilmDat.App.ViewModels;
using FilmDat.App.ViewModels.Interfaces;
using FilmDat.App.Views;
using FilmDat.BL.Interfaces;
using FilmDat.BL.Repositories;
using FilmDat.BL.Services;
using FilmDat.DAL.Factories;
using FilmDat.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace FilmDat.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices((context, services) => { ConfigureServices(context.Configuration, services); })
                .Build();
        }

        private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
        {
            builder.AddJsonFile(@"AppSettings.json", false, true);
        }

        private static void ConfigureServices(IConfiguration configuration,
            IServiceCollection services)
        {
            services.AddSingleton<IMediator, Mediator>();
            services.AddSingleton<IMessageDialogService, MessageDialogService>();
            services.AddSingleton<MainWindow>();

            services.AddSingleton<IFilmRepository, FilmRepository>();
            services.AddSingleton<IPersonRepository, PersonRepository>();
            services.AddSingleton<IReviewRepository, ReviewRepository>();
            services.AddSingleton<IActedInFilmRepository, ActedInFilmRepository>();
            services.AddSingleton<IDirectedFilmRepository, DirectedFilmRepository>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<ISearchViewModel, SearchViewModel>();

            services.AddSingleton<IFilmListViewModel, FilmListViewModel>();
            services.AddSingleton<IPersonListViewModel, PersonListViewModel>();
            services.AddSingleton<IReviewListViewModel, ReviewListViewModel>();

            services.AddSingleton<IAddActedInFilmViewModel, AddActedInFilmViewModel>();
            services.AddSingleton<IAddActorListViewModel, AddActorListViewModel>();
            services.AddSingleton<IAddDirectedFilmViewModel, AddDirectedFilmViewModel>();
            services.AddSingleton<IAddDirectorListViewModel, AddDirectorListViewModel>();

            services.AddFactory<IFilmDetailViewModel, FilmDetailViewModel>();
            services.AddFactory<IPersonDetailViewModel, PersonDetailViewModel>();
            services.AddFactory<IReviewDetailViewModel, ReviewDetailViewModel>();

            services.AddSingleton<IDbContextFactory>(provider =>
                new SqlServerDbContextFactory(configuration.GetConnectionString("DefaultConnection")));
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var dbContextFactory = _host.Services.GetRequiredService<IDbContextFactory>();

#if DEBUG
            await using (var dbx = dbContextFactory.CreateDbContext())
            {
                await dbx.Database.MigrateAsync();
            }
#endif

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}