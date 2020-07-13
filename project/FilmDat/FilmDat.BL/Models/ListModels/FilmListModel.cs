using System;
using System.Collections.Generic;

namespace FilmDat.BL.Models.ListModels
{
    public class FilmListModel : BaseModel
    {
        public string OriginalName { get; set; }

        private sealed class IdOriginalNameEqualityComparer : IEqualityComparer<FilmListModel>
        {
            public bool Equals(FilmListModel x, FilmListModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) && x.OriginalName == y.OriginalName;
            }

            public int GetHashCode(FilmListModel obj)
            {
                return HashCode.Combine(obj.Id, obj.OriginalName);
            }
        }

        public static IEqualityComparer<FilmListModel> IdOriginalNameComparer { get; } =
            new IdOriginalNameEqualityComparer();
    }
}