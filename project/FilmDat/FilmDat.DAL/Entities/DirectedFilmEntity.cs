using System;
using System.Collections.Generic;

namespace FilmDat.DAL.Entities
{
    public class DirectedFilmEntity : EntityBase
    {
        public Guid FilmId { get; set; }
        public Guid DirectorId { get; set; }
        public FilmEntity Film { get; set; }
        public PersonEntity Director { get; set; }

        private sealed class DirectedFilmEntityEqualityComparer : IEqualityComparer<DirectedFilmEntity>
        {
            public bool Equals(DirectedFilmEntity x, DirectedFilmEntity y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) &&
                       FilmEntity.FilmAloneComparer.Equals(x.Film, y.Film) &&
                       PersonEntity.PersonEntityAloneComparer.Equals(x.Director, y.Director);
            }

            public int GetHashCode(DirectedFilmEntity obj)
            {
                return HashCode.Combine(obj.FilmId, obj.DirectorId, obj.Film, obj.Director);
            }
        }

        public static IEqualityComparer<DirectedFilmEntity> DirectedFilmEntityComparer { get; } =
            new DirectedFilmEntityEqualityComparer();
    }
}