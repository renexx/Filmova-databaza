using System;
using System.Collections.Generic;

namespace FilmDat.BL.Models.DetailModels
{
    public class DirectedFilmDetailModel : BaseModel
    {
        public Guid DirectorId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Guid FilmId { get; set; }
        public String OriginalName { get; set; }

        private sealed class DirectedFilmDetailModelEqualityComparer : IEqualityComparer<DirectedFilmDetailModel>
        {
            public bool Equals(DirectedFilmDetailModel x, DirectedFilmDetailModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) && x.DirectorId.Equals(y.DirectorId) && x.FirstName == y.FirstName &&
                       x.LastName == y.LastName && x.FilmId.Equals(y.FilmId) && x.OriginalName == y.OriginalName;
            }

            public int GetHashCode(DirectedFilmDetailModel obj)
            {
                return HashCode.Combine(obj.Id, obj.DirectorId, obj.FirstName, obj.LastName, obj.FilmId,
                    obj.OriginalName);
            }
        }

        public static IEqualityComparer<DirectedFilmDetailModel> DirectedFilmDetailModelComparer { get; } =
            new DirectedFilmDetailModelEqualityComparer();
    }
}