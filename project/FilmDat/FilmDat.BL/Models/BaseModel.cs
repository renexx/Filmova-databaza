using System;
using FilmDat.Common;

namespace FilmDat.BL.Models
{
    public abstract class BaseModel : IId
    {
        public Guid Id { get; set; }
    }
}