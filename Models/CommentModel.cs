using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogASP.Models
{
    public class CommentModel
    {
        [Key]
        public int CommentId { get; set; }
        public string? Description{ get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public UserModel? User { get; set; }

        public int? ArticleId { get; set; }

        
        [ForeignKey("ArticleId")]
        public ArticleModel? Article { get; set; }
    }
}
