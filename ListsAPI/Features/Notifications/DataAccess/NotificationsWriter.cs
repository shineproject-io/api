using Dapper;
using ListsAPI.Features.Notifications.Enums;
using ListsAPI.Infrastructure.Database;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ListsAPI.Features.Notifications.DataAccess
{
    public interface INotificationsWriter
    {
        Task<int> Write(int userProfileId, string title, NotificationType type, string message, Object metadata);
    }

    public class NotificationsWriter : INotificationsWriter
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;

        public NotificationsWriter(IDatabaseConnectionProvider databaseConnectionProvider)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
        }

        public async Task<int> Write(int userProfileId, string title, NotificationType type, string message, Object metadata)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var dateCreated = DateTime.UtcNow;

                var notificationId = await con.ExecuteScalarAsync<int>(@"
                    INSERT INTO Notifications
                        (UserProfileId, Title, Type, State, Message, DateCreated, DateUpdated, Metadata)
                    VALUES
                        (@userProfileId, @title, @type, @state, @message, @dateCreated, @dateCreated, @metadata)

                    SELECT SCOPE_IDENTITY()", new
                {
                    userProfileId,
                    title,
                    type,
                    state = NotificationState.Unread,
                    message,
                    dateCreated,
                    metadata = SerialiseMetadata(metadata)
                });

                return notificationId;
            }
        }

        private string SerialiseMetadata(Object metadata)
        {
            var metadataString = string.Empty;

            if (metadata != null)
            {
                metadataString = JsonConvert.SerializeObject(metadata);
            }

            return metadataString;
        }
    }
}