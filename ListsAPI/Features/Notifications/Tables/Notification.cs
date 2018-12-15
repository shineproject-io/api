using ListsAPI.Features.Notifications.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ListsAPI.Features.Notifications.Tables
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserProfileId { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        [Required]
        public NotificationState State { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }

        public string Metadata { get; set; }
    }
}