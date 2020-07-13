using FilmDat.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FilmDat.DAL.Factories
{
    public class DbContextInMemoryFactory : IDbContextFactory
    {
        private readonly string _databaseName;

        public DbContextInMemoryFactory(string databaseName)
        {
            _databaseName = databaseName;
        }

        public FilmDatDbContext CreateDbContext()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<FilmDatDbContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase(_databaseName);
            return new FilmDatDbContext(dbContextOptionsBuilder.Options);
        }
    }
}