using System;
using System.Collections.Generic;
using FilmDat.DAL.Enums;
using FilmDat.BL.Models.ListModels;

namespace FilmDat.BL.Models.DetailModels
{
    public class FilmDetailModel : BaseModel
    {
        public String OriginalName { get; set; }
        public String CzechName { get; set; }
        public GenreEnum Genre { get; set; }
        public String TitleFotoUrl { get; set; }
        public String Country { get; set; }
        public TimeSpan Duration { get; set; }
        public String Description { get; set; }
        public string AvgRating { get; set; }
        public ICollection<ActedInFilmDetailModel> Actors { get; set; }
        public ICollection<DirectedFilmDetailModel> Directors { get; set; }
        public ICollection<ReviewListModel> Reviews { get; set; }

        private sealed class FilmDetailModelEqualityComparer : IEqualityComparer<FilmDetailModel>
        {
            public bool Equals(FilmDetailModel x, FilmDetailModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id) && x.OriginalName == y.OriginalName && x.CzechName == y.CzechName &&
                       x.Genre == y.Genre && x.TitleFotoUrl == y.TitleFotoUrl && x.Country == y.Country &&
                       x.Duration.Equals(y.Duration) && x.Description == y.Description;
            }

            public int GetHashCode(FilmDetailModel obj)
            {
                return HashCode.Combine(obj.Id, obj.OriginalName, obj.CzechName, (int) obj.Genre, obj.TitleFotoUrl,
                    obj.Country, obj.Duration, obj.Description);
            }
        }

        public static IEqualityComparer<FilmDetailModel> FilmDetailModelComparer { get; } =
            new FilmDetailModelEqualityComparer();
    }
}