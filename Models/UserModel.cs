﻿namespace BlogASP.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }

        public int? CommentId { get; set; }

        public int? ArticletId { get; set; }

    }
}
