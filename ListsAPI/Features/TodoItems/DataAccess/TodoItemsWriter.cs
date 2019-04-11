using Dapper;
using ListsAPI.Features.TodoItems.Enums;
using ListsAPI.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ListsAPI.Features.TodoItems.DataAccess
{
    public interface ITodoItemsWriter
    {
        Task<int> Add(string title, TodoItemState state, int listId, int userProfileId);

        Task DeleteAllTodoItemsByListId(int listId);

        Task ChangeState(int todoItemId, TodoItemState state);

        Task ChangeImportance(int todoItemId, int isImportant);

        Task ChangeTitle(int todoItemId, string title);

        Task ChangeOrder(int listId, List<int> orderedTodoItems);

        Task MigrateOpenTodoItemsToListById(int fromListId, int toListId);

        Task DeleteTodoItemsByUserProfileId(int userProfileId, IDbTransaction transaction);
    }

    public class TodoItemsWriter : ITodoItemsWriter
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;

        public TodoItemsWriter(IDatabaseConnectionProvider databaseConnectionProvider)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
        }

        public async Task<int> Add(string title, TodoItemState state, int listId, int userProfileId)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var createdDate = DateTime.UtcNow;

                var todoItemId = Convert.ToInt32(await con.ExecuteScalarAsync(@"
                    INSERT INTO
                        TodoItems
                            (Title, State, DateCreated, DateUpdated, ListId, UserProfileId)
                        VALUES
                            (@title, @state, @createdDate, @createdDate, @listId, @userProfileId)

                    SELECT SCOPE_IDENTITY()", new
                {
                    title,
                    state,
                    createdDate,
                    listId,
                    userProfileId
                }));

                return todoItemId;
            }
        }

        public async Task DeleteAllTodoItemsByListId(int listId)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        TodoItems
                    SET
                        State = @state,
                        DateUpdated = @updateDate
                    WHERE
                        ListId = @listId", new
                {
                    state = TodoItemState.Deleted,
                    updateDate,
                    listId
                });
            }
        }

        public async Task ChangeState(int todoItemId, TodoItemState state)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        TodoItems
                    SET
                        State = @state,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @todoItemId", new
                {
                    state,
                    updateDate,
                    todoItemId
                });
            }
        }

        public async Task ChangeImportance(int todoItemId, int isImportant)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;
                try
                {
                    await con.ExecuteAsync(@"
                    UPDATE
                        TodoItems
                    SET
                        IsImportant = @isImportant,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @todoItemId", new
                    {
                        isImportant,
                        updateDate,
                        todoItemId
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task ChangeTitle(int todoItemId, string title)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        TodoItems
                    SET
                        Title = @title,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @todoItemId", new
                {
                    title,
                    updateDate,
                    todoItemId
                });
            }
        }

        public async Task ChangeOrder(int listId, List<int> orderedTodoItems)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                for (int loop = 0; loop < orderedTodoItems.Count; loop += 1)
                {
                    var todoItemId = orderedTodoItems[loop];

                    await con.ExecuteAsync(@"
                    UPDATE
                        TodoItems
                    SET
                        Position = @position,
                        DateUpdated = @updateDate
                    WHERE
                        Id = @todoItemId
                            AND ListId = @listId", new
                    {
                        position = loop + 1,
                        updateDate,
                        todoItemId,
                        listId
                    });
                }
            }
        }

        public async Task MigrateOpenTodoItemsToListById(int fromListId, int toListId)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var updateDate = DateTime.UtcNow;

                await con.ExecuteAsync(@"
                    UPDATE
                        TodoItems
                    SET
                        ListId = @toListId,
                        DateUpdated = @updateDate
                    WHERE
                        ListId = @fromListId
                            AND State = @openState", new
                {
                    toListId,
                    fromListId,
                    updateDate,
                    openState = TodoItemState.Open
                });
            }
        }

        public async Task DeleteTodoItemsByUserProfileId(int userProfileId, IDbTransaction transaction)
        {
            await transaction.Connection.ExecuteAsync(@"
                DELETE FROM
                    TodoItems
                WHERE
                    UserProfileId = @userProfileId", new
            {
                userProfileId
            },
            transaction);
        }
    }
}