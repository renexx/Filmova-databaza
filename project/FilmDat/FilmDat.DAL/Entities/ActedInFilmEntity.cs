using System;
using System.Collections.Generic;

namespace FilmDat.DAL.Entities
{
    public class ActedInFilmEntity : EntityBase
    {
        public Guid FilmId { get; set; }
        public Guid ActorId { get; set; }
        public FilmEntity Film { get; set; }
        public PersonEntity Actor { get; set; }

        private sealed class ActedInFilmEntityEqualityComparer : IEqualityComparer<ActedInFilmEntity>
        {
            public bool Equals(ActedInFilmEntity x, ActedInFilmEntity y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) &&
                       FilmEntity.FilmAloneComparer.Equals(x.Film, y.Film) &&
                       PersonEntity.PersonEntityAloneComparer.Equals(x.Actor, y.Actor);
            }

            public int GetHashCode(ActedInFilmEntity obj)
            {
                return HashCode.Combine(obj.FilmId, obj.ActorId, obj.Film, obj.Actor);
            }
        }

        public static IEqualityComparer<ActedInFilmEntity> ActedInFilmEntityComparer { get; } =
            new ActedInFilmEntityEqualityComparer();
    }
}