using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogASP.Models
{
    public class ArticleModel
    {
        [Key]
        public int ArticleId { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public int? Stars { get; set; }
        public bool isPrivate { get; set; }
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public UserModel? User { get; set; }

        public ICollection<CommentModel>? Comments { get; set; }

    }
}
