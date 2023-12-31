﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogASP.Models
{
    public class CommentModel
    {
        [Key]
        public int CommentId { get; set; }
        public string? Description{ get; set; }

        public int? UserId { get; set; }
        public string? UserName { get; set; }

        [ForeignKey("UserId")]
        public UserModel? User { get; set; }

        public int? ArticleId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
        [ForeignKey("ArticleId")]
        public ArticleModel? Article { get; set; }
    }
}
