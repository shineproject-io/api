using Dapper;
using ListsAPI.Features.TodoItems.Enums;
using ListsAPI.Features.TodoItems.Tables;
using ListsAPI.Infrastructure.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListsAPI.Features.TodoItems.DataAccess
{
    public interface ITodoItemsReader
    {
        Task<IEnumerable<TodoItem>> GetByListId(int listId);

        Task<TodoItem> GetByTodoItemId(int todoItemId);

        Task<IEnumerable<TodoItem>> Search(string searchQuery, int userProfileId);
    }

    public class TodoItemsReader : ITodoItemsReader
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;

        public TodoItemsReader(IDatabaseConnectionProvider databaseConnectionProvider)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
        }

        public async Task<IEnumerable<TodoItem>> GetByListId(int listId)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var todoItems = await con.QueryAsync<TodoItem>(@"SELECT * FROM TodoItems WHERE ListId = @listId AND State != @deletedState", new
                {
                    listId,
                    deletedState = TodoItemState.Deleted
                });

                return todoItems;
            }
        }

        public async Task<TodoItem> GetByTodoItemId(int todoItemId)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var todoItem = await con.QuerySingleOrDefaultAsync<TodoItem>(@"SELECT * FROM TodoItems WHERE Id = @todoItemId", new
                {
                    todoItemId
                });

                return todoItem;
            }
        }

        public async Task<IEnumerable<TodoItem>> Search(string searchQuery, int userProfileId)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var searchResult = await con.QueryAsync<TodoItem>(@"
                    SELECT
	                    *
                    FROM
	                    TodoItems AS TDO
                    WHERE
	                    TDO.UserProfileId = @userProfileId
		                    AND TDO.State != 3
		                    AND TDO.Title LIKE '%' + @searchQuery + '%'", new
                {
                    userProfileId,
                    searchQuery
                });

                return searchResult;
            }
        }
    }
}