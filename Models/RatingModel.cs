using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlogASP.Models
{
    public class RatingModel
    {
        [Key]
        public int RatingId { get; set; }

        [Required]
        public int ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public ArticleModel Article { get; set; }

        [Required]
        public string Username { get; set; }
        
        public int Stars { get; set; }
    }
}
