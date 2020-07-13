using System;
using System.Collections.Generic;

namespace FilmDat.BL.Models.ListModels
{
    public class ReviewListModel : BaseModel
    {
        public uint Rating { get; set; }
        public String TextReview { get; set; }
        public Guid FilmId { get; set; }

        private sealed class ReviewListModelEqualityComparer : IEqualityComparer<ReviewListModel>
        {
            public bool Equals(ReviewListModel x, ReviewListModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) && x.Rating == y.Rating && x.TextReview == y.TextReview &&
                       x.FilmId.Equals(y.FilmId);
            }

            public int GetHashCode(ReviewListModel obj)
            {
                return HashCode.Combine(obj.Id, obj.Rating, obj.TextReview, obj.FilmId);
            }
        }

        public static IEqualityComparer<ReviewListModel> ReviewListModelComparer { get; } =
            new ReviewListModelEqualityComparer();
    }
}