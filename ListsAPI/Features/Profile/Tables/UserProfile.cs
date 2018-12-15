using System;
using System.ComponentModel.DataAnnotations;

namespace ListsAPI.Features.Profile.Tables
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string GivenName { get; set; }

        [Required]
        public string FamilyName { get; set; }

        [Required]
        public string ProfilePicturePath { get; set; }

        [Required]
        public string ProfilePictureFileName { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }
    }
}