using System;
using System.Collections.Generic;
using FilmDat.BL.Interfaces;
using FilmDat.BL.Mapper;
using FilmDat.BL.Models.DetailModels;
using FilmDat.BL.Models.ListModels;
using FilmDat.DAL.Entities;
using FilmDat.DAL.Interfaces;
using FilmDat.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FilmDat.BL.Repositories
{
    public class FilmRepository : RepositoryBase<FilmEntity, FilmListModel, FilmDetailModel>, IFilmRepository
    {
        public FilmRepository(IDbContextFactory dbContextFactory)
            : base(dbContextFactory,
                FilmMapper.MapToEntity,
                FilmMapper.MapToListModel,
                FilmMapper.MapToDetailModel,
                new Func<FilmEntity, IEnumerable<IEntity>>[]
                    {entity => entity.Actors, entity => entity.Directors, entity => entity.Reviews},
                entities => entities
                    .Include(entity => entity.Actors)
                    .ThenInclude(entity => entity.Actor)
                    .Include(entity => entity.Directors)
                    .ThenInclude(entity => entity.Director)
                    .Include(entity => entity.Reviews),
                null)
        {
        }
    }
}