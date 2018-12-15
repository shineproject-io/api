using Dapper;
using ListsAPI.Features.Profile.Tables;
using ListsAPI.Infrastructure;
using ListsAPI.Infrastructure.Database;
using System.Threading.Tasks;

namespace ListsAPI.Features.Profile.DataAccess
{
    public interface IUserProfileReader
    {
        Task<UserProfile> GetByUserProfileId(int userProfileId);

        Task<UserProfile> GetByEmailAddress(string emailAddress);
    }

    public class UserProfileReader : IUserProfileReader
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;

        public UserProfileReader(ListContext context, IDatabaseConnectionProvider databaseConnectionProvider)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
        }

        public async Task<UserProfile> GetByUserProfileId(int userProfileId)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var userProfile = await con.QuerySingleOrDefaultAsync<UserProfile>(@"SELECT * FROM UserProfiles WHERE Id = @userProfileId", new
                {
                    userProfileId
                });

                return userProfile;
            }
        }

        public async Task<UserProfile> GetByEmailAddress(string emailAddress)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var userProfile = await con.QuerySingleOrDefaultAsync<UserProfile>(@"SELECT * FROM UserProfiles WHERE EmailAddress = @emailAddress", new
                {
                    emailAddress
                });

                return userProfile;
            }
        }
    }
}