using Dapper;
using ListsAPI.Features.Lists.Enums;
using ListsAPI.Infrastructure;
using ListsAPI.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ListsAPI.Features.Lists.DataAccess
{
    public interface IListWriter
    {
        Task<int> Add(int userId, string name, string description, ListState state, string backgroundImageFilePath);

        Task DeleteList(int listId, ListState state);

        Task ChangeState(int listId, ListState state);

        Task ChangePicture(int listId, string backgroundImageFilePath, string backgroundImageFileName);

        Task ChangeName(int listId, string name);

        Task ChangeDescription(int listId, string description);

        Task ChangeOrder(List<int> orderedListIds, int userProfileId);

        Task DeleteListsByUserProfileId(int userProfileId, IDbTransaction transaction);
    }

    public class ListWriter : IListWriter
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;

        public ListWriter(ListContext context, IDatabaseConnectionProvider databaseConnectionProvider)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
        }

        public async Task<int> Add(int userId, string name, string description, ListState state, string backgroundImageFilePath)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var createdDate = DateTime.UtcNow;

                var listId = Convert.ToInt32(await con.ExecuteScalarAsync(@"
                    INSERT INTO
                        Lists
                            (UserId, Name, Description, State, DateCreated, DateUpdated, BackgroundImageFilePath)
                        VALUES
                            (@userId, @name, @description, @state, @createdDate, @createdDate, @backgroundImageFilePath)

                    SELECT SCOPE_IDENTITY()", new
                {
                    userId,
                    name,
                    description,
                    state,
                    createdDate,
                    backgroundImageFilePath
                }));

                return listId;
            }
        }

        public async Task DeleteList(int listId, ListState state)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        Lists
                    SET
                        State = @state,
                        BackgroundImageFileName = '',
                        BackgroundImageFilePath = '',
                        DateUpdated = updateDate
                    WHERE
                        Id = @listId", new
                {
                    listId,
                    state,
                    updateDate
                });
            }
        }

        public async Task ChangeState(int listId, ListState state)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        Lists
                    SET
                        State = @state,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @listId", new
                {
                    listId,
                    state,
                    updateDate
                });
            }
        }

        public async Task ChangePicture(int listId, string backgroundImageFilePath, string backgroundImageFileName)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        Lists
                    SET
                        BackgroundImageFilePath = @backgroundImageFilePath,
                        BackgroundImageFileName = @backgroundImageFileName,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @listId", new
                {
                    listId,
                    backgroundImageFilePath,
                    backgroundImageFileName,
                    updateDate
                });
            }
        }

        public async Task ChangeName(int listId, string name)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        Lists
                    SET
                        Name = @name,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @listId", new
                {
                    listId,
                    name,
                    updateDate
                });
            }
        }

        public async Task ChangeDescription(int listId, string description)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        Lists
                    SET
                        Description = @description,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @listId", new
                {
                    listId,
                    description,
                    updateDate
                });
            }
        }

        public async Task ChangeOrder(List<int> orderedListIds, int userProfileId)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                for (int loop = 0; loop < orderedListIds.Count; loop += 1)
                {
                    var listId = orderedListIds[loop];

                    await con.ExecuteAsync(@"
                    UPDATE
                        Lists
                    SET
                        Position = @position,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @listId
                            AND UserId = @userProfileId
                            AND State = @state", new
                    {
                        position = loop + 1,
                        updateDate,
                        listId,
                        userProfileId,
                        state = ListState.Open
                    });
                }
            }
        }

        public async Task DeleteListsByUserProfileId(int userId, IDbTransaction transaction)
        {
            await transaction.Connection.ExecuteAsync(@"
                DELETE FROM
                    Lists
                WHERE
                    UserId = @userId", new
            {
                userId
            },
            transaction);
        }
    }
}