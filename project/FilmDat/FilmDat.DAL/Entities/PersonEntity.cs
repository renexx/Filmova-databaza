using System;
using System.Collections.Generic;
using System.Linq;

namespace FilmDat.DAL.Entities
{
    public class PersonEntity : EntityBase
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public String FotoUrl { get; set; }
        public ICollection<DirectedFilmEntity> DirectedFilms { get; set; } = new List<DirectedFilmEntity>();
        public ICollection<ActedInFilmEntity> ActedInFilms { get; set; } = new List<ActedInFilmEntity>();

        private sealed class PersonEntityEqualityComparer : IEqualityComparer<PersonEntity>
        {
            public bool Equals(PersonEntity x, PersonEntity y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.FirstName == y.FirstName && x.LastName == y.LastName && x.BirthDate.Equals(y.BirthDate) &&
                       x.FotoUrl == y.FotoUrl &&
                       x.ActedInFilms.OrderBy(actor => actor.Id).SequenceEqual(
                           y.ActedInFilms.OrderBy(actor => actor.Id), ActedInFilmEntity.ActedInFilmEntityComparer) &&
                       x.DirectedFilms.OrderBy(director => director.Id).SequenceEqual(
                           y.DirectedFilms.OrderBy(director => director.Id),
                           DirectedFilmEntity.DirectedFilmEntityComparer);
            }


            public int GetHashCode(PersonEntity obj)
            {
                return HashCode.Combine(obj.FirstName, obj.LastName, obj.BirthDate, obj.FotoUrl);
            }
        }

        public static IEqualityComparer<PersonEntity> PersonEntityComparer { get; } =
            new PersonEntityEqualityComparer();

        private sealed class PersonEntityAloneEqualityComparer : IEqualityComparer<PersonEntity>
        {
            public bool Equals(PersonEntity x, PersonEntity y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.FirstName == y.FirstName && x.LastName == y.LastName && x.BirthDate.Equals(y.BirthDate) &&
                       x.FotoUrl == y.FotoUrl;
            }


            public int GetHashCode(PersonEntity obj)
            {
                return HashCode.Combine(obj.FirstName, obj.LastName, obj.BirthDate, obj.FotoUrl);
            }
        }

        public static IEqualityComparer<PersonEntity> PersonEntityAloneComparer { get; } =
            new PersonEntityAloneEqualityComparer();
    }
}