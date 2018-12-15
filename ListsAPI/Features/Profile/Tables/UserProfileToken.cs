using ListsAPI.Features.Profile.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ListsAPI.Features.Profile.Tables
{
    public class UserProfileToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserProfileId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public UserProfileTokenType TokenType { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime ExpirationTime { get; set; }

        public DateTime? DateUsed { get; set; }
    }
}