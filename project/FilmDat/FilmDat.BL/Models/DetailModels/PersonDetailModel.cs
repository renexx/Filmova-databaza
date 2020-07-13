using System;
using System.Collections.Generic;
using FilmDat.BL.Models.ListModels;

namespace FilmDat.BL.Models.DetailModels
{
    public class PersonDetailModel : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string FotoUrl { get; set; }
        public ICollection<ActedInFilmDetailModel> ActedInFilms { get; set; }
        public ICollection<DirectedFilmDetailModel> DirectedFilms { get; set; }

        private sealed class PersonDetailModelEqualityComparer : IEqualityComparer<PersonDetailModel>
        {
            public bool Equals(PersonDetailModel x, PersonDetailModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) && x.FirstName == y.FirstName && x.LastName == y.LastName &&
                       x.BirthDate.Equals(y.BirthDate) && x.FotoUrl == y.FotoUrl;
            }

            public int GetHashCode(PersonDetailModel obj)
            {
                return HashCode.Combine(obj.Id, obj.FirstName, obj.LastName, obj.BirthDate, obj.FotoUrl);
            }
        }

        public static IEqualityComparer<PersonDetailModel> PersonDetailModelComparer { get; } =
            new PersonDetailModelEqualityComparer();
    }
}