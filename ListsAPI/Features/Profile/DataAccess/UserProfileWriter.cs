using Dapper;
using ListsAPI.Infrastructure;
using ListsAPI.Infrastructure.Database;
using System;
using System.Threading.Tasks;

namespace ListsAPI.Features.Profile.DataAccess
{
    public interface IUserProfileWriter
    {
        Task<int> Add(string emailAddress, string password, string givenName, string familyName, string profilePicturePath);

        Task SetName(int userProfileId, string givenName, string familyName);

        Task SetPassword(int userProfileId, string password);

        Task SetEmailAddress(int userProfileId, string emailAddress);

        Task SetProfilePicturePath(int userProfileId, string profilePicturePath, string profilePictureFileName);
    }

    public class UserProfileWriter : IUserProfileWriter
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;

        public UserProfileWriter(ListContext context, IDatabaseConnectionProvider databaseConnectionProvider)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
        }

        public async Task<int> Add(string emailAddress, string password, string givenName, string familyName, string profilePicturePath)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var createdDate = DateTime.UtcNow;
                var profilePictureFileName = string.Empty;

                var userProfileId = Convert.ToInt32(await con.ExecuteScalarAsync(@"
                    INSERT INTO
                        UserProfiles
                            (EmailAddress, Password, GivenName, FamilyName, DateCreated, DateUpdated, ProfilePicturePath, ProfilePictureFileName)
                        VALUES
                            (@emailAddress, @password, @givenName, @familyName, @createdDate, @createdDate, @profilePicturePath, @profilePictureFileName)

                    SELECT SCOPE_IDENTITY()", new
                {
                    emailAddress,
                    password,
                    givenName,
                    familyName,
                    createdDate,
                    profilePicturePath,
                    profilePictureFileName
                }));

                return userProfileId;
            }
        }

        public async Task SetName(int userProfileId, string givenName, string familyName)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        UserProfiles
                    SET
                        GivenName = @givenName,
                        FamilyName = @familyName,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @userProfileId", new
                {
                    givenName,
                    familyName,
                    updateDate,
                    userProfileId
                });
            }
        }

        public async Task SetPassword(int userProfileId, string password)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        UserProfiles
                    SET
                        Password = @password,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @userProfileId", new
                {
                    password,
                    updateDate,
                    userProfileId
                });
            }
        }

        public async Task SetEmailAddress(int userProfileId, string emailAddress)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        UserProfiles
                    SET
                        EmailAddress = @emailAddress,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @userProfileId", new
                {
                    emailAddress,
                    updateDate,
                    userProfileId
                });
            }
        }

        public async Task SetProfilePicturePath(int userProfileId, string profilePicturePath, string profilePictureFileName)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        UserProfiles
                    SET
                        ProfilePicturePath = @profilePicturePath,
                        ProfilePictureFileName = @profilePictureFileName,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @userProfileId", new
                {
                    profilePicturePath,
                    profilePictureFileName,
                    updateDate,
                    userProfileId
                });
            }
        }
    }
}