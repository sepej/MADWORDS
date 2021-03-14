using System.ComponentModel.DataAnnotations;

namespace madwords.Models
{
    public class Rating
    {
        [Key]
        public int RatingID { get; set; }
        public int RatingScore { get; set; }
        public AppUser Rater { get; set; }
    }
}