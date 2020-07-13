using System;
using System.Collections.Generic;
using System.Text;

namespace FilmDat.BL.Models.ListModels
{
    public class DirectedFilmListModel : BaseModel
    {
        public Guid DirectorId { get; set; }
        public Guid FilmId { get; set; }

        private sealed class IdDirectorIdFilmIdEqualityComparer : IEqualityComparer<DirectedFilmListModel>
        {
            public bool Equals(DirectedFilmListModel x, DirectedFilmListModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) && x.DirectorId.Equals(y.DirectorId) && x.FilmId.Equals(y.FilmId);
            }

            public int GetHashCode(DirectedFilmListModel obj)
            {
                return HashCode.Combine(obj.Id, obj.DirectorId, obj.FilmId);
            }
        }

        public static IEqualityComparer<DirectedFilmListModel> IdDirectorIdFilmIdComparer { get; } =
            new IdDirectorIdFilmIdEqualityComparer();
    }
}