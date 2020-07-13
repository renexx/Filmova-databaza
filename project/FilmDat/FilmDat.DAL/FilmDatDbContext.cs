using FilmDat.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using FilmDat.DAL.Seeds;

namespace FilmDat.DAL
{
    public class FilmDatDbContext : DbContext
    {
        public FilmDatDbContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }

        public DbSet<FilmEntity> Films { get; set; }
        public DbSet<ReviewEntity> Reviews { get; set; }
        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<ActedInFilmEntity> ActedInFilms { get; set; }
        public DbSet<DirectedFilmEntity> DirectedFilms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilmEntity>()
                .HasMany(r => r.Reviews)
                .WithOne(f => f.Film)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ActedInFilmEntity>()
                .HasIndex(af => new {af.FilmId, af.ActorId}).IsUnique();
            modelBuilder.Entity<DirectedFilmEntity>()
                .HasIndex(df => new {df.FilmId, df.DirectorId}).IsUnique();

            modelBuilder.SeedPerson();
            modelBuilder.SeedFilm();
            modelBuilder.SeedReview();
            modelBuilder.SeedActedInFilm();
            modelBuilder.SeedDirectedFilm();
        }
    }
}