using System;
using System.Collections.Generic;
using FilmDat.DAL.Entities;
using FilmDat.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace FilmDat.DAL.Seeds
{
    public static class Seed
    {
        public static readonly PersonEntity JohnTravolta = new PersonEntity()
        {
            Id = Guid.Parse("e1e20085-1ce4-4612-be57-285b8c76d76a"),
            FirstName = "John",
            LastName = "Travolta",
            BirthDate = new DateTime(1972, 11, 12),
            FotoUrl = "johntravolta.jpg",
            DirectedFilms = new List<DirectedFilmEntity>(),
            ActedInFilms = new List<ActedInFilmEntity>()
        };

        public static readonly PersonEntity RandalKleiser = new PersonEntity()
        {
            Id = Guid.Parse("6d372469-af50-4cfe-9582-8789bf598b2b"),
            FirstName = "Randal",
            LastName = "Kleiser",
            BirthDate = new DateTime(1972, 11, 12),
            FotoUrl = "randalkleiser.jpg",
            DirectedFilms = new List<DirectedFilmEntity>(),
            ActedInFilms = new List<ActedInFilmEntity>()
        };

        public static readonly PersonEntity MatthewMcConaughey = new PersonEntity()
        {
            Id = Guid.Parse("0a816848-99a5-4aae-8449-487d0847998a"),
            FirstName = "Matthew",
            LastName = "McConaughey",
            BirthDate = new DateTime(1979, 11, 4),
            FotoUrl = "mato.jpg",
            DirectedFilms = new List<DirectedFilmEntity>(),
            ActedInFilms = new List<ActedInFilmEntity>()
        };

        public static readonly PersonEntity ChristopherNolan = new PersonEntity()
        {
            Id = Guid.Parse("0ae10491-658f-4fa8-860b-215ebb29cba2"),
            FirstName = "Christopher",
            LastName = "Nolan",
            BirthDate = new DateTime(1970, 7, 30),
            FotoUrl = "chris.jpg",
            DirectedFilms = new List<DirectedFilmEntity>(),
            ActedInFilms = new List<ActedInFilmEntity>()
        };

        public static readonly FilmEntity GreaseFilm = new FilmEntity()
        {
            Id = Guid.Parse("088e40b8-63f6-4089-bfa9-4146e36e888c"),
            OriginalName = "Grease",
            CzechName = "Pomada",
            Genre = GenreEnum.Romance,
            TitleFotoUrl = "pomada.jpg",
            Country = "USA",
            Duration = new TimeSpan(2, 0, 0),
            Description = "Romanticky muzikal",
            Reviews = new List<ReviewEntity>(),
            Directors = new List<DirectedFilmEntity>(),
            Actors = new List<ActedInFilmEntity>()
        };

        public static readonly FilmEntity InterstellarFilm = new FilmEntity()
        {
            Id = Guid.Parse("16d3e5e1-a52a-4fbc-ac16-305491fe0b8e"),
            OriginalName = "Interstellar",
            CzechName = "Intergalakticky",
            Genre = GenreEnum.SciFi,
            TitleFotoUrl = "gargantua.jpg",
            Country = "USA",
            Duration = new TimeSpan(2, 0, 0),
            Description = "Scifi mindfuck...",
            Reviews = new List<ReviewEntity>(),
            Directors = new List<DirectedFilmEntity>(),
            Actors = new List<ActedInFilmEntity>()
        };

        public static readonly ReviewEntity Review1 = new ReviewEntity()
        {
            Id = Guid.Parse("585b8ad0-aa06-49dd-94fd-8ab6c93f7e57"),
            NickName = "Alan232",
            Date = new DateTime(2013, 6, 5),
            Rating = 82,
            TextReview = "Skvely film plny tanca a zabavy.",
            FilmId = GreaseFilm.Id,
            Film = GreaseFilm
        };

        public static readonly ReviewEntity Review2 = new ReviewEntity()
        {
            Id = Guid.Parse("7d71a9ec-8633-4a42-b929-194863c2fe9d"),
            NickName = "Branimir",
            Date = new DateTime(2003, 8, 15),
            Rating = 69,
            TextReview = "John je skvely.",
            FilmId = GreaseFilm.Id,
            Film = GreaseFilm
        };

        public static readonly ReviewEntity Review3 = new ReviewEntity()
        {
            Id = Guid.Parse("0ec5146a-6564-4285-ae12-b7b5621ab852"),
            NickName = "Jozef",
            Date = new DateTime(1999, 8, 5),
            Rating = 74,
            TextReview = "To bol mindfuck.",
            FilmId = InterstellarFilm.Id,
            Film = InterstellarFilm
        };

        public static readonly ReviewEntity Review4 = new ReviewEntity()
        {
            Id = Guid.Parse("54caf507-8046-4587-a93d-9f1bf5cf1f91"),
            NickName = "Brano",
            Date = new DateTime(2002, 7, 15),
            Rating = 99,
            TextReview = "Gargantua je riadne delo.",
            FilmId = InterstellarFilm.Id,
            Film = InterstellarFilm
        };

        public static readonly ActedInFilmEntity JohnTravoltaFilmA = new ActedInFilmEntity()
        {
            Id = Guid.Parse("501744f2-4fc1-494b-8b84-5fecb9f7903d"),
            FilmId = GreaseFilm.Id,
            ActorId = JohnTravolta.Id,
            Film = GreaseFilm,
            Actor = JohnTravolta
        };

        public static readonly DirectedFilmEntity RandalKleiserFilmD = new DirectedFilmEntity()
        {
            Id = Guid.Parse("75cb065e-643a-4b6f-807f-b3add4cf0eca"),
            FilmId = GreaseFilm.Id,
            DirectorId = RandalKleiser.Id,
            Film = GreaseFilm,
            Director = RandalKleiser
        };

        public static readonly ActedInFilmEntity MatthewMcConaugheyFilmA = new ActedInFilmEntity()
        {
            Id = Guid.Parse("a81ae9bd-3a1d-4612-b27e-a6a3d5cbaf9b"),
            FilmId = InterstellarFilm.Id,
            ActorId = MatthewMcConaughey.Id,
            Film = InterstellarFilm,
            Actor = MatthewMcConaughey
        };

        public static readonly DirectedFilmEntity ChristopherNolanFilmD = new DirectedFilmEntity()
        {
            Id = Guid.Parse("5a4d3189-5daa-420e-9360-1146505a3d4d"),
            FilmId = InterstellarFilm.Id,
            DirectorId = ChristopherNolan.Id,
            Film = InterstellarFilm,
            Director = ChristopherNolan
        };

        static Seed()
        {
            JohnTravolta.ActedInFilms.Add(JohnTravoltaFilmA);
            GreaseFilm.Reviews.Add(Review1);
            GreaseFilm.Reviews.Add(Review2);
            GreaseFilm.Actors.Add(JohnTravoltaFilmA);
            GreaseFilm.Directors.Add(RandalKleiserFilmD);
            RandalKleiser.DirectedFilms.Add(RandalKleiserFilmD);

            MatthewMcConaughey.ActedInFilms.Add(MatthewMcConaugheyFilmA);
            InterstellarFilm.Reviews.Add(Review3);
            InterstellarFilm.Reviews.Add(Review4);
            InterstellarFilm.Actors.Add(MatthewMcConaugheyFilmA);
            InterstellarFilm.Directors.Add(ChristopherNolanFilmD);
            ChristopherNolan.DirectedFilms.Add(ChristopherNolanFilmD);
        }

        public static void SeedPerson(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonEntity>()
                .HasData(
                    new PersonEntity()
                    {
                        Id = JohnTravolta.Id,
                        FirstName = JohnTravolta.FirstName,
                        LastName = JohnTravolta.LastName,
                        BirthDate = JohnTravolta.BirthDate,
                        FotoUrl = JohnTravolta.FotoUrl
                    },
                    new PersonEntity()
                    {
                        Id = RandalKleiser.Id,
                        FirstName = RandalKleiser.FirstName,
                        LastName = RandalKleiser.LastName,
                        BirthDate = RandalKleiser.BirthDate,
                        FotoUrl = RandalKleiser.FotoUrl
                    },
                    new PersonEntity()
                    {
                        Id = MatthewMcConaughey.Id,
                        FirstName = MatthewMcConaughey.FirstName,
                        LastName = MatthewMcConaughey.LastName,
                        BirthDate = MatthewMcConaughey.BirthDate,
                        FotoUrl = MatthewMcConaughey.FotoUrl
                    },
                    new PersonEntity()
                    {
                        Id = ChristopherNolan.Id,
                        FirstName = ChristopherNolan.FirstName,
                        LastName = ChristopherNolan.LastName,
                        BirthDate = ChristopherNolan.BirthDate,
                        FotoUrl = ChristopherNolan.FotoUrl
                    }
                );
        }

        public static void SeedFilm(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilmEntity>()
                .HasData(
                    new FilmEntity()
                    {
                        Id = GreaseFilm.Id,
                        OriginalName = GreaseFilm.OriginalName,
                        CzechName = GreaseFilm.CzechName,
                        Genre = GreaseFilm.Genre,
                        TitleFotoUrl = GreaseFilm.TitleFotoUrl,
                        Country = GreaseFilm.Country,
                        Duration = GreaseFilm.Duration,
                        Description = GreaseFilm.Description
                    },
                    new FilmEntity()
                    {
                        Id = InterstellarFilm.Id,
                        OriginalName = InterstellarFilm.OriginalName,
                        CzechName = InterstellarFilm.CzechName,
                        Genre = InterstellarFilm.Genre,
                        TitleFotoUrl = InterstellarFilm.TitleFotoUrl,
                        Country = InterstellarFilm.Country,
                        Duration = InterstellarFilm.Duration,
                        Description = InterstellarFilm.Description
                    }
                );
        }

        public static void SeedReview(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReviewEntity>()
                .HasData(
                    new ReviewEntity()
                    {
                        Id = Review1.Id,
                        NickName = Review1.NickName,
                        Date = Review1.Date,
                        Rating = Review1.Rating,
                        TextReview = Review1.TextReview,
                        FilmId = Review1.FilmId
                    },
                    new ReviewEntity()
                    {
                        Id = Review2.Id,
                        NickName = Review2.NickName,
                        Date = Review2.Date,
                        Rating = Review2.Rating,
                        TextReview = Review2.TextReview,
                        FilmId = Review2.FilmId
                    },
                    new ReviewEntity()
                    {
                        Id = Review3.Id,
                        NickName = Review3.NickName,
                        Date = Review3.Date,
                        Rating = Review3.Rating,
                        TextReview = Review3.TextReview,
                        FilmId = Review3.FilmId
                    },
                    new ReviewEntity()
                    {
                        Id = Review4.Id,
                        NickName = Review4.NickName,
                        Date = Review4.Date,
                        Rating = Review4.Rating,
                        TextReview = Review4.TextReview,
                        FilmId = Review4.FilmId
                    }
                );
        }

        public static void SeedActedInFilm(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActedInFilmEntity>()
                .HasData(
                    new ActedInFilmEntity()
                    {
                        Id = JohnTravoltaFilmA.Id,
                        FilmId = JohnTravoltaFilmA.FilmId,
                        ActorId = JohnTravoltaFilmA.ActorId
                    },
                    new ActedInFilmEntity()
                    {
                        Id = MatthewMcConaugheyFilmA.Id,
                        FilmId = MatthewMcConaugheyFilmA.FilmId,
                        ActorId = MatthewMcConaugheyFilmA.ActorId
                    }
                );
        }

        public static void SeedDirectedFilm(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DirectedFilmEntity>()
                .HasData(
                    new DirectedFilmEntity()
                    {
                        Id = RandalKleiserFilmD.Id,
                        FilmId = RandalKleiserFilmD.FilmId,
                        DirectorId = RandalKleiserFilmD.DirectorId
                    },
                    new DirectedFilmEntity()
                    {
                        Id = ChristopherNolanFilmD.Id,
                        FilmId = ChristopherNolanFilmD.FilmId,
                        DirectorId = ChristopherNolanFilmD.DirectorId
                    }
                );
        }
    }
}