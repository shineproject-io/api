using ListsAPI.Features.Notifications.Enums;
using System;

namespace ListsAPI.Features.Notifications.ResponseModels
{
    public class NotificationResponse
    {
        public int Id { get; set; }

        public NotificationType Type { get; set; }

        public NotificationState State { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string Metadata { get; set; }
    }
}