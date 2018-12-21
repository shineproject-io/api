using Dapper;
using ListsAPI.Features.Notifications.Enums;
using ListsAPI.Features.Notifications.Tables;
using ListsAPI.Infrastructure.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListsAPI.Features.Notifications.DataAccess
{
    public interface INotificationsReader
    {
        Task<IEnumerable<Notification>> Get(int userProfileId);
    }

    public class NotificationsReader : INotificationsReader
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;

        public NotificationsReader(IDatabaseConnectionProvider databaseConnectionProvider)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
        }

        public async Task<IEnumerable<Notification>> Get(int userProfileId)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var lists = await con.QueryAsync<Notification>(@"SELECT * FROM Notifications WHERE UserProfileId = @userProfileId AND State = @state", new
                {
                    userProfileId,
                    state = NotificationState.Unread
                });

                return lists;
            }
        }
    }
}