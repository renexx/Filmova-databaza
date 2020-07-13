namespace FilmDat.DAL.Interfaces
{
    public interface IDbContextFactory
    {
        FilmDatDbContext CreateDbContext();
    }
}