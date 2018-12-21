using ListsAPI.Features.Notifications.DataAccess;
using ListsAPI.Features.Notifications.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ListsAPI.Features.Notifications
{
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsReader _notificationsReader;
        private readonly INotificationsWriter _notificationsWriter;

        public NotificationsController(INotificationsReader notificationsReader, INotificationsWriter notificationsWriter)
        {
            _notificationsReader = notificationsReader;
            _notificationsWriter = notificationsWriter;
        }

        /// <summary>
        /// Get all unread notifications for the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/userprofiles/me/notifications")]
        public async Task<IActionResult> Index()
        {
            var userProfileId = 1;

            var notifications = await _notificationsReader.Get(userProfileId);

            var response = notifications.Select(not => new NotificationResponse
            {
                Id = not.Id,
                Title = not.Title,
                Message = not.Message,
                State = not.State,
                Type = not.Type,
                Metadata = not.Metadata
            });

            return Ok(response);
        }

        /// <summary>
        /// Create a test notification
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/userprofiles/me/notifications")]
        public async Task<IActionResult> Post()
        {
            var notificationId = await _notificationsWriter.Write(1, "Test", Enums.NotificationType.System, "Message", new NotificationResponse { Message = "LOL" });

            return Ok(notificationId);
        }
    }
}