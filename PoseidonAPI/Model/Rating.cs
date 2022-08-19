using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoseidonAPI.Model
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RatingId { get; set; }
        public string MoodysRating { get; set; }
        public string SandPrating { get; set; }
        public string FitchRating { get; set; }
        public int OrderNumber { get; set; }
    }
}
