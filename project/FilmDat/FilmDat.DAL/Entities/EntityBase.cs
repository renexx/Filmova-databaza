using System;
using FilmDat.DAL.Interfaces;

namespace FilmDat.DAL.Entities
{
    public class EntityBase : IEntity
    {
        public Guid Id { get; set; }
    }
}