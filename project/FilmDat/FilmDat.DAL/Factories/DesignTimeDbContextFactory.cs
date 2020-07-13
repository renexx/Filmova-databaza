using FilmDat.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FilmDat.DAL.Factories
{
    public class DesignTimeDbContextFactory : IDbContextFactory
    {
        public FilmDatDbContext CreateDbContext()
            => new SqlServerDbContextFactory(
                    @"Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog = FilmDat2;MultipleActiveResultSets = True;Integrated Security = True;")
                .CreateDbContext();
    }

    public class FilmDatDbContextFactory : IDesignTimeDbContextFactory<FilmDatDbContext>
    {
        public FilmDatDbContext CreateDbContext(string[] args)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<FilmDatDbContext>();
            dbContextOptionsBuilder.UseSqlServer(
                @"Data Source = (LocalDB)\MSSQLLocalDB;Initial Catalog = FilmDat2;MultipleActiveResultSets = True;Integrated Security = True;");
            return new FilmDatDbContext(dbContextOptionsBuilder.Options);
        }
    }
}