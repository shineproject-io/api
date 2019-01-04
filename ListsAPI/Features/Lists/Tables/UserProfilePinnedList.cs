using System.ComponentModel.DataAnnotations;

namespace ListsAPI.Features.Lists.Tables
{
    public class UserProfilePinnedList
    {
        [Key]
        public int UserProfilePinnedListId { get; set; }

        [Required]
        public int UserProfileId { get; set; }

        [Required]
        public int ListId { get; set; }
    }
}