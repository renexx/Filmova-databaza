using System;
using System.Collections.Generic;
using System.Text;
using FilmDat.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FilmDat.DAL.Factories
{
    public class SqlServerDbContextFactory : IDbContextFactory
    {
        private readonly string _connectionString;

        public SqlServerDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public FilmDatDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<FilmDatDbContext>();
            optionsBuilder.UseSqlServer(_connectionString);
            return new FilmDatDbContext(optionsBuilder.Options);
        }
    }
}