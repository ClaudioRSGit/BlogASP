﻿using System.ComponentModel.DataAnnotations;

namespace BlogASP.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }

        public ICollection<ArticleModel> Articles { get; set; }
        public ICollection<CommentModel> Comments { get; set; }

    }
}
