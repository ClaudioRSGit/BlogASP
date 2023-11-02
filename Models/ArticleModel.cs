using System.Collections.Generic;
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
        public string? Picture { get; set; }

        public bool? isDisabled { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        [ForeignKey("UserId")]
        public UserModel? User { get; set; }
        public ICollection<CommentModel>? Comments { get; set; }
    }
}