using Dapper;
using ListsAPI.Features.Lists.Enums;
using ListsAPI.Features.Lists.Tables;
using ListsAPI.Infrastructure;
using ListsAPI.Infrastructure.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListsAPI.Features.Lists.DataAccess
{
    public interface IListReader
    {
        Task<IEnumerable<List>> GetByState(int userProfileId, ListState state);

        Task<List> GetByListId(int listId);

        Task<IEnumerable<List>> Search(string searchQuery, int userProfileId);
    }

    public class ListReader : IListReader
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;

        public ListReader(ListContext context, IDatabaseConnectionProvider databaseConnectionProvider)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
        }

        public async Task<IEnumerable<List>> GetByState(int userProfileId, ListState state)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var lists = await con.QueryAsync<List>(@"SELECT * FROM Lists WHERE UserId = @userProfileId AND State = @state", new
                {
                    userProfileId,
                    state
                });

                return lists;
            }
        }

        public async Task<List> GetByListId(int listId)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var list = await con.QuerySingleOrDefaultAsync<List>(@"SELECT * FROM Lists WHERE Id = @listId", new
                {
                    listId
                });

                return list;
            }
        }

        public async Task<IEnumerable<List>> Search(string searchQuery, int userProfileId)
        {
            using (var con = _databaseConnectionProvider.New())
            {
                var searchResult = await con.QueryAsync<List>(@"
                    SELECT
	                    *
                    FROM
	                    Lists AS LST
                    WHERE
	                    LST.UserId = @userProfileId
		                    AND LST.State != 4
		                    AND (LST.Name LIKE '%' + @searchQuery + '%' OR LST.Description LIKE '%' + @searchQuery + '%')", new
                {
                    userProfileId,
                    searchQuery
                });

                return searchResult;
            }
        }
    }
}