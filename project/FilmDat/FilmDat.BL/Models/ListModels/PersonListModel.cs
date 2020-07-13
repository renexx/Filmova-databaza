using System;
using System.Collections.Generic;

namespace FilmDat.BL.Models.ListModels
{
    public class PersonListModel : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private sealed class IdFirstNameLastNameEqualityComparer : IEqualityComparer<PersonListModel>
        {
            public bool Equals(PersonListModel x, PersonListModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) && x.FirstName == y.FirstName && x.LastName == y.LastName;
            }

            public int GetHashCode(PersonListModel obj)
            {
                return HashCode.Combine(obj.Id, obj.FirstName, obj.LastName);
            }
        }

        public static IEqualityComparer<PersonListModel> IdFirstNameLastNameComparer { get; } =
            new IdFirstNameLastNameEqualityComparer();
    }
}