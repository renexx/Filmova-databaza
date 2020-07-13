using System;
using System.Collections.Generic;
using System.Text;

namespace FilmDat.BL.Models.ListModels
{
    public class ActedInFilmListModel : BaseModel
    {
        public Guid ActorId { get; set; }
        public Guid FilmId { get; set; }

        private sealed class IdActorIdFilmIdEqualityComparer : IEqualityComparer<ActedInFilmListModel>
        {
            public bool Equals(ActedInFilmListModel x, ActedInFilmListModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) && x.ActorId.Equals(y.ActorId) && x.FilmId.Equals(y.FilmId);
            }

            public int GetHashCode(ActedInFilmListModel obj)
            {
                return HashCode.Combine(obj.Id, obj.ActorId, obj.FilmId);
            }
        }

        public static IEqualityComparer<ActedInFilmListModel> IdActorIdFilmIdComparer { get; } =
            new IdActorIdFilmIdEqualityComparer();
    }
}